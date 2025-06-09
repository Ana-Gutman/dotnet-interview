using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;

namespace McpServer.Tools;

internal static class HttpClientExt
{
    public static async Task<JsonDocument> ReadJsonDocumentAsync(
        this HttpClient client,
        string requestUri
    )
    {
        using var response = await client.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync();
        return await JsonDocument.ParseAsync(stream);
    }
}

[McpServerToolType]
public static class TodoItemTools
{
    private static string FormatTodoItem(JsonElement item)
    {
        var id = item.GetProperty("id").GetInt64();
        var description = item.GetProperty("description").GetString();
        var isCompleted = item.GetProperty("isCompleted").GetBoolean();
        return $"Item {id}: \"{description}\" (Status: {(isCompleted ? "Completed" : "Pending")})";
    }

    [McpServerTool, Description("Create a new todo item in a specified list.")]
    public static async Task<string> CreateTodoItem(
        HttpClient client,
        [Description("ID of the todo list")] long todoListId,
        [Description("Description of the new item")] string description
    )
    {
        var url = $"/api/todolists/{todoListId}/items";
        var payload = new { description };
        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        using var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        return FormatTodoItem(doc.RootElement);
    }

    [McpServerTool, Description("Get details of a specific todo item.")]
    public static async Task<string> GetTodoItem(
        HttpClient client,
        [Description("ID of the todo list")] long todoListId,
        [Description("ID of the todo item")] long itemId
    )
    {
        var url = $"/api/todolists/{todoListId}/items/{itemId}";
        using var doc = await client.ReadJsonDocumentAsync(url);
        return FormatTodoItem(doc.RootElement);
    }

    [McpServerTool, Description("Update the description of a todo item.")]
    public static async Task<string> UpdateTodoItem(
        HttpClient client,
        [Description("ID of the todo list")] long todoListId,
        [Description("ID of the todo item")] long itemId,
        [Description("New description for the item")] string newDescription
    )
    {
        var url = $"/api/todolists/{todoListId}/items/{itemId}";
        var payload = new { description = newDescription };
        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        using var response = await client.PutAsync(url, content);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        return FormatTodoItem(doc.RootElement);
    }

    [McpServerTool, Description("Mark a todo item as completed.")]
    public static async Task<string> CompleteTodoItem(
        HttpClient client,
        [Description("ID of the todo list")] long todoListId,
        [Description("ID of the todo item")] long itemId
    )
    {
        var url = $"/api/todolists/{todoListId}/items/{itemId}/complete";
        using var response = await client.PatchAsync(url, null);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var doc = await JsonDocument.ParseAsync(stream);
        return FormatTodoItem(doc.RootElement);
    }

    [McpServerTool, Description("Delete a todo item.")]
    public static async Task<string> DeleteTodoItem(
        HttpClient client,
        [Description("ID of the todo list")] long todoListId,
        [Description("ID of the todo item")] long itemId
    )
    {
        var url = $"/api/todolists/{todoListId}/items/{itemId}";
        using var response = await client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
        return $" Item {itemId} in list {todoListId} was successfully deleted.";
    }
}
