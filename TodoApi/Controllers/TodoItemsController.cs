.using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers;

[Route("api/todolists/{todoListId}/items")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    // POST: api/todolists/1/items
    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateItem(long todoListId, CreateTodoItem payload)
    {
        var list = await _context.TodoList.FindAsync(todoListId);
        if (list == null)
        {
            return NotFound("TodoList not found.");
        }

        var item = new TodoItem
        {
            Description = payload.Description,
            IsCompleted = false,
            TodoListId = todoListId
        };

        _context.TodoItem.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetItem), new { todoListId, itemId = item.Id }, item);
    }

    // GET: api/todolists/1/items/5
    [HttpGet("{itemId}")]
    public async Task<ActionResult<TodoItem>> GetItem(long todoListId, long itemId)
    {
        var item = await _context.TodoItem
            .FirstOrDefaultAsync(i => i.TodoListId == todoListId && i.Id == itemId);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    // PUT: api/todolists/1/items/5
    [HttpPut("{itemId}")]
    public async Task<IActionResult> UpdateItem(long todoListId, long itemId, UpdateTodoItem payload)
    {
        var item = await _context.TodoItem
            .FirstOrDefaultAsync(i => i.TodoListId == todoListId && i.Id == itemId);

        if (item == null)
        {
            return NotFound();
        }

        item.Description = payload.Description;
        await _context.SaveChangesAsync();

        return Ok(item);
    }

    // PATCH: api/todolists/1/items/5/complete
    [HttpPatch("{itemId}/complete")]
    public async Task<IActionResult> CompleteItem(long todoListId, long itemId)
    {
        var item = await _context.TodoItem
            .FirstOrDefaultAsync(i => i.TodoListId == todoListId && i.Id == itemId);

        if (item == null)
        {
            return NotFound();
        }

        item.IsCompleted = true;
        await _context.SaveChangesAsync();

        return Ok(item);
    }


    // DELETE: api/todolists/1/items/5
    [HttpDelete("{itemId}")]
    public async Task<IActionResult> DeleteItem(long todoListId, long itemId)
    {
        var item = await _context.TodoItem
            .FirstOrDefaultAsync(i => i.TodoListId == todoListId && i.Id == itemId);

        if (item == null)
        {
            return NotFound();
        }

        _context.TodoItem.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
