using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Tests;

#nullable disable
public class TodoItemsControllerTests
{
    private DbContextOptions<TodoContext> DatabaseContextOptions()
    {
        return new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    private void PopulateDatabaseWithTodoList(TodoContext context)
    {
        context.TodoList.Add(new TodoList { Id = 1, Name = "Task 1" });
        context.TodoList.Add(new TodoList { Id = 2, Name = "Task 2" });
        context.SaveChanges();
    }

    [Fact]
    public async Task CreateItem_WhenListExists_ReturnsCreatedItem()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseWithTodoList(context);
        var controller = new TodoItemsController(context);

        var payload = new CreateTodoItem { Description = "New Item" };
        var result = await controller.CreateItem(1, payload);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var item = Assert.IsType<TodoItem>(created.Value);
        Assert.Equal("New Item", item.Description);
        Assert.False(item.IsCompleted);
        Assert.Equal(1, item.TodoListId);
    }

    [Fact]
    public async Task CreateItem_WhenListDoesNotExist_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        var controller = new TodoItemsController(context);

        var payload = new CreateTodoItem { Description = "Should Fail" };
        var result = await controller.CreateItem(999, payload);

        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("TodoList not found.", notFound.Value);
    }

    [Fact]
    public async Task GetItem_WhenItemExists_ReturnsItem()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseWithTodoList(context);
        var item = new TodoItem { Description = "Item 1", TodoListId = 1 };
        context.TodoItem.Add(item);
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);
        var result = await controller.GetItem(1, item.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedItem = Assert.IsType<TodoItem>(okResult.Value);
        Assert.Equal(item.Id, returnedItem.Id);
    }

    [Fact]
    public async Task GetItem_WhenItemDoesNotExist_ReturnsNotFound()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseWithTodoList(context);
        var controller = new TodoItemsController(context);

        var result = await controller.GetItem(1, 999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateItem_WhenItemExists_UpdatesDescription()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseWithTodoList(context);
        var item = new TodoItem { Description = "Before", TodoListId = 1 };
        context.TodoItem.Add(item);
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);
        var result = await controller.UpdateItem(
            1,
            item.Id,
            new UpdateTodoItem { Description = "After" }
        );

        var ok = Assert.IsType<OkObjectResult>(result);
        var updated = Assert.IsType<TodoItem>(ok.Value);
        Assert.Equal("After", updated.Description);
    }

    [Fact]
    public async Task CompleteItem_WhenItemExists_MarksAsCompleted()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseWithTodoList(context);
        var item = new TodoItem
        {
            Description = "To Complete",
            IsCompleted = false,
            TodoListId = 1,
        };
        context.TodoItem.Add(item);
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);
        var result = await controller.CompleteItem(1, item.Id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var updated = Assert.IsType<TodoItem>(ok.Value);
        Assert.True(updated.IsCompleted);
    }

    [Fact]
    public async Task DeleteItem_WhenItemExists_RemovesIt()
    {
        using var context = new TodoContext(DatabaseContextOptions());
        PopulateDatabaseWithTodoList(context);
        var item = new TodoItem { Description = "To Delete", TodoListId = 1 };
        context.TodoItem.Add(item);
        await context.SaveChangesAsync();

        var controller = new TodoItemsController(context);
        var result = await controller.DeleteItem(1, item.Id);

        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.TodoItem.ToList());
    }
}
