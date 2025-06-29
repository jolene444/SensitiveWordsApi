# SensitiveWordsApi

A simple C# .NET Core RESTful API for managing and sanitizing sensitive words, with full CRUD functionality, database integration (ADO.NET, MSSQL), Swagger documentation, and unit test coverage.

---

## 🚀 **Project Overview**

- **Purpose:**  
  - Manage a list of sensitive words (CRUD: Create, Read, Update, Delete)
  - Sanitize any input string, masking all sensitive words with asterisks (`******`)
  - Serve as a microservice for integration with other systems
- **Tech Stack:**  
  - ASP.NET Core Web API (C#)
  - ADO.NET (no EF/Linq)
  - SQL Server
  - Swagger/OpenAPI docs
  - xUnit & Moq for testing

SensitiveWordsApi/
│
├── Controllers/
│ ├── SensitiveWordsController.cs # CRUD endpoints
│ └── SanitizeController.cs # Endpoint for sanitizing input
│
├── Repositories/
│ ├── ISensitiveWordsRepository.cs # Repository interface
│ └── SensitiveWordsRepository.cs # ADO.NET implementation
│
├── Services/
│ └── SensitiveWordSanitizer.cs # Sanitizes text using sensitive words
│
├── Models/ # (if you use separate models)
│
├── SensitiveWordsApi.Tests/ # xUnit test project (controllers, repo, sanitizer)
│
├── appsettings.json # Connection strings etc.
└── README.md

---

## 📁 **Project Structure**


---

## 🛠️ **Setup & Run Locally**

### 1. **Prerequisites**
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or any local SQL Server instance)
- Visual Studio 2022+ (recommended) or VS Code

### 2. **Clone the Repo**

```bash
git clone https://github.com/YOUR_GITHUB_USERNAME/SensitiveWordsApi.git
cd SensitiveWordsApi


3. Configure Database
Ensure you have a database named SENSITIVE_WORDS_DB.

Create the SensitiveWords table (run this in SQL Server Management Studio):

sql
Copy
Edit


CREATE TABLE SensitiveWords (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Word NVARCHAR(100) NOT NULL UNIQUE
);

In appsettings.json, update your connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=SENSITIVE_WORDS_DB;Trusted_Connection=True;"
}

Replace YOUR_SERVER_NAME with your actual SQL Server instance (e.g., localhost\\SQLEXPRESS).

4. Build and Run the API

dotnet build
dotnet run --project SensitiveWordsApi
Or press F5 in Visual Studio.


5. View and Test the API
Open your browser to:
https://localhost:7247/swagger
(URL may differ; see console output)

Available Endpoints:
POST /api/SensitiveWords/Add — Add a sensitive word

GET /api/SensitiveWords/GetAll — List all words

PUT /api/SensitiveWords/Update/{id} — Update a word

DELETE /api/SensitiveWords/Delete/{id} — Delete by ID

DELETE /api/SensitiveWords/DeleteByWord/{word} — Delete by word (if implemented)

POST /api/Sanitize/SanitizeText — Sanitize an input string (returns string with sensitive words masked)

Run Unit Tests
Navigate to the test project and run:

dotnet test

Tests cover: Sanitizer logic, CRUD operations, and API controller responses.

How the API Works (For Novices)
SensitiveWordsController:

Add, get, update, or delete sensitive words stored in SQL Server.

SanitizeController:

Send any text. It will automatically replace all sensitive words with the same number of asterisks (***).

Sanitizer Service:

Does the actual text masking, using Regex for whole-word, case-insensitive replacement.


Enhancements & Senior Features:

Exception handling: 409 Conflict for duplicates, 404 for missing records, 500 for unexpected errors.

Dependency injection: All logic is modular and testable.

Swagger/OpenAPI: Every endpoint documented, easy to try out.

Testable: xUnit & Moq for all logic layers.

Ready for team handover or CI/CD integration.

