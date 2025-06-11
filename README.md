# dotnet-interview / TodoApi

[![Open in Coder](https://dev.crunchloop.io/open-in-coder.svg)](https://dev.crunchloop.io/templates/fly-containers/workspace?param.Git%20Repository=git@github.com:crunchloop/dotnet-interview.git)

This repository contains an **enhanced .NET 8** Todo List API extended with:
- **RESTful endpoints** (Create, Read, Update, Delete)
- **Comprehensive xUnit tests** for all endpoints
- **A Model Context Protocol (MCP) server** that exposes “tools” to enable natural-language prompts via IA clients (e.g. Claude Desktop).

---

## ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for DevContainer)  
- An IA client with MCP support (e.g., Claude Desktop) 

---

## ⚙️ Local Setup & Execution

### 1. Clone & DevContainer (optional)

```bash
git clone https://github.com/Ana-Gutman/dotnet-interview.git
cd dotnet-interview
```
**NOTE**: If you are not using the DevContainer, ensure a local SQL Server instance is running and update the `ConnectionStrings:Default` in `appsettings.json`.

---

### 2. Build all projects and restore tools

```bash
# Restore .NET tools (for EF migrations)
dotnet tool restore
# Build all solution projects
dotnet build
```

---


### 3. Database Migration

```bash
# Apply migrations to update or create the database
cd TodoApi
dotnet ef database update
cd ..
```

---

### 4. Run the API locally

```bash
dotnet run --project TodoApi
```
The API will start at: http://localhost:5083


---


### 4. Run the test suite

```bash
dotnet test
```

Check integration tests at: [https://github.com/crunchloop/interview-tests](https://github.com/crunchloop/interview-tests)

---

### 5. Run the MCP Server

```bash
dotnet run --project McpServer/McpServer.csproj

```
Press Ctrl + C to stop the server.

---

This enables AI-driven clients to send natural-language commands because it generates a MCPServer.exe file.  

## 💻 Configure Claude Desktop

1. Open your Claude Desktop settings file:

For Windows:  
`%APPDATA%\Claude Desktop\claude_desktop_config.json`

2. Add or update the mcpServers section:

```json
{
  "mcpServers": {
    "TodoListItems": {
       "command": "dotnet",
        "args": [
          "run",
          "--no-build",
          "--project",
          "C:\\ABSOLUTE\\PATH\\TO\\dotnet-interview\\McpServer\\McpServer.csproj",
        ]
    }
  }
}
```
My example of absolute path: "C:\\dotnet-interview\\MCPServer\\MCPServer.csproj"

**Important** : 
- The --no-build flag must appear before --project, otherwise Claude will attempt to execute a missing .exe and crash.
- Ensure the path has no trailing commas and is an absolute Windows path.
- Place the project outside of synced folders (e.g., OneDrive) to avoid file-lock issues.


---

3. Save and **restart** Claude Desktop


## 🧠 Natural Language Prompt Examples

Use these examples in Claude Desktop (or any MCP-enabled client) once the MCP server is running:
- Create: Create a todo item in list '1' with description 'Finish report'
- Get: Get details of item in '1' in list '1'
- Update: Update the description of item 1 in list 1 to "Buy milk".
- Complete: Mark the todo item with ID 1 in list 1 as completed.
- Delete: Remove item ID '1' from '1'

The MCP server will interpret these commands and perform the corresponding API calls.

---

## 📞 Contact

- Martín Fernández — [mfernandez@crunchloop.io](mailto:mfernandez@crunchloop.io)

---

## 🌀 About Crunchloop

![crunchloop](https://crunchloop.io/logo-blue.png)

We strongly believe in giving back 🚀.  
Let’s work together → [`Get in touch`](https://crunchloop.io/contact)
