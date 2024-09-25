using Microsoft.AspNetCore.Mvc;
using Todos.Api.Mapping;
using Todos.Application.Repositories;
using Todos.Contracts.Requests;

namespace Todos.Api.Controllers;

[ApiController]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;

    public TodosController(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    [HttpPost(ApiEndpoints.Todos.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequest request)
    {
        var todo = request.MapToTodo();
        await _todoRepository.CreateAsync(todo);
        var todoResponse = todo.MapToResponse();
        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todoResponse);
    }

    [HttpGet(ApiEndpoints.Todos.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        var response = todo.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Todos.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var todos = await _todoRepository.GetAllAsync();

        var response = todos.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Todos.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTodoRequest request)
    {
        var todo = request.MapToTodo(id);
        var updated = await _todoRepository.UpdateAsync(todo);
        if (!updated)
        {
            return NotFound();
        }

        var response = todo.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Todos.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _todoRepository.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}