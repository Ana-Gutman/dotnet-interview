# dotnet-interview / TodoApi

[![Open in Coder](https://dev.crunchloop.io/open-in-coder.svg)](https://dev.crunchloop.io/templates/fly-containers/workspace?param.Git%20Repository=git@github.com:crunchloop/dotnet-interview.git)

This repository contains an enhanced **.NET 8** Todo List API extended with:
- **RESTful endpoints** (Create, Read, Update, Delete)
- **xUnit tests** for all endpoints
- **A Model Context Protocol (MCP) server** that exposes “tools” to enable natural-language prompts via IA clients (e.g. Claude Desktop).

---

## ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for DevContainer)  
- IA client with MCP support (e.g., Claude Desktop) 

---

## ⚙️ Local Setup & Execution

### 1. Clone & DevContainer (optional)

```bash
git clone https://github.com/Ana-Gutman/dotnet-interview.git
cd dotnet-interview
```

If you prefer not to use the DevContainer, ensure you have a local SQL Server instance and update the `ConnectionStrings:Default` in `appsettings.json`.

---

### 2. Build all projects

```bash
dotnet build
```

---

### 3. Run the TodoAPI locally

```bash
dotnet run --project TodoApi
```

---

### 4. Execute the test suite

```bash
dotnet test
```

Check integration tests at: [https://github.com/crunchloop/interview-tests](https://github.com/crunchloop/interview-tests)

---

### 5. Run the MCP Server

```bash
dotnet run --project McpServer/McpServer.csproj
```

---

## 💻 Configure Claude Desktop

Place and save your MCP server configuration in your Claude Desktop settings file.  
Then, restart Claude Desktop.

For Windows:  
`%APPDATA%\Claude Desktop\claude_desktop_config.json`

```json
{
  "mcpServers": {
    "TodoListItems": {
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "C:\\ABSOLUTE\\PATH\\TO\\dotnet-interview\\McpServer\\McpServer.csproj",
        "--no-build"
      ]
    }
  }
}
```

---

## 🧠 Natural Language Prompt Examples

- Create a todo item in list "Work" with description "Finish report"
- List all items in "Personal"
- Mark item ID 3 as completed

The MCP server will interpret these commands and perform the corresponding API calls.

---

## 📞 Contact

- Martín Fernández — [mfernandez@crunchloop.io](mailto:mfernandez@crunchloop.io)

---

## 🌀 About Crunchloop

![crunchloop](https://crunchloop.io/logo-blue.png)

We strongly believe in giving back 🚀.  
Let’s work together → [`Get in touch`](https://crunchloop.io/contact)
