using Microsoft.EntityFrameworkCore;

public class TodoContext : DbContext
{
  public TodoContext(DbContextOptions<TodoContext> options)
      : base(options)
  {
  }

  public DbSet<TodoApi.Models.TodoItem> TodoItem { get; set; } = default!;
}
