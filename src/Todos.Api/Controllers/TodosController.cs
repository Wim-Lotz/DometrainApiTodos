using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Todos.Api.Auth;
using Todos.Api.Mapping;
using Todos.Application.Services;
using Todos.Contracts.Requests;

namespace Todos.Api.Controllers;

[ApiController]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPost(ApiEndpoints.Todos.Create)]
    public async Task<IActionResult> Create([FromBody] CreateTodoRequest request, CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        
        var todo = request.MapToTodo(userId!.Value);
        await _todoService.CreateAsync(todo, token);
        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
    }

    [HttpGet(ApiEndpoints.Todos.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken token)
    {
        var todo = await _todoService.GetByIdAsync(id, token);
        if (todo is null)
        {
            return NotFound();
        }

        var response = todo.MapToResponse();
        return Ok(response);
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Todos.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var todos = await _todoService.GetAllAsync(token);

        var response = todos.MapToResponse();
        return Ok(response);
    }
    
    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpGet(ApiEndpoints.Todos.GetMyTodos)]
    public async Task<IActionResult> GetAllMine(CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        
        var todos = await _todoService.GetAllMineAsync(userId!.Value, token);

        var response = todos.MapToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.TrustedMemberPolicyName)]
    [HttpPut(ApiEndpoints.Todos.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTodoRequest request, 
        CancellationToken token)
    {
        var userId = HttpContext.GetUserId();
        
        var todo = request.MapToTodo(id, userId!.Value);
        var updatedTodo = await _todoService.UpdateAsync(todo, token);
        if (updatedTodo is null)
        {
            return NotFound();
        }

        var response = updatedTodo.MapToResponse();
        return Ok(response);
    }

    [Authorize(AuthConstants.AdminUserPolicyName)]
    [HttpDelete(ApiEndpoints.Todos.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _todoService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}