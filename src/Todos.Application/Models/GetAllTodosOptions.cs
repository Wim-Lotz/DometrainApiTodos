﻿namespace Todos.Application.Models;

public class GetAllTodosOptions
{
    public string? Description { get; set; }
    public Guid? UserId { get; set; }
    public string? SortField { get; set; }
    public SortOrder? SortOrder { get; set; }

    public int Page { get; set; }
    public int PageSize { get; set; }
}

public enum SortOrder
{
    Unsorted,
    Ascending,
    Descending
}