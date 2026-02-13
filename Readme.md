# CityInfo API Documentation 🏙️

## Project Overview
**CityInfo API** is a robust, RESTful web service developed using **ASP.NET Core 8.0**. It is designed to manage hierarchical data structures representing cities and their associated points of interest. This project serves as a comprehensive example of modern .NET backend development, focusing on clean architecture, scalability, and industry-standard patterns before integrating a persistent database.

## 🏗️ Architectural Foundations

### 1. Dependency Injection (IoC) & Service Lifetimes
The core architecture is built upon the **Inversion of Control (IoC)** principle using the built-in Dependency Injection (DI) container. This ensures loose coupling between components and improves testability.

*   **Constructor Injection:** We explicitly utilized constructor injection across all Controllers and Services. This makes dependencies explicit and avoids hidden dependencies.
*   **Service Lifetimes:**
    *   **Singleton:** The `CitiesDataStore` is registered as a Singleton. This mimics a persistent data store by keeping data in memory for the entire lifecycle of the application.
    *   **Transient:** Lightweight services, such as the `LocalMailService`, are registered as Transient to ensure a fresh instance is provided for every request, preventing state leakage.
*   **Interface-Based Design:** Services (like the Mail Service) are decoupled using interfaces (`IMailService`). This allows for seamless swapping of implementations (e.g., switching from a Local Mock Service to a Cloud Service) without modifying the consuming code.

### 2. Configuration Management
Hardcoding values is strictly avoided in favor of a flexible configuration model.

*   **IConfiguration Integration:** The `IConfiguration` interface is injected into services to access settings dynamically.
*   **Externalized Settings:** Key operational parameters—specifically email settings (`mailTo`, `mailFrom`)—are stored in `appsettings.json`.
*   **Hierarchical Configuration:** The system supports environment-specific overrides (e.g., `appsettings.Development.json`), allowing different configurations for local development versus production environments.

### 3. Data Persistence Strategy (Current State)
*   **In-Memory Store:** Currently, the application utilizes a thread-safe, in-memory data collection to simulate database operations.
*   **Decoupled Access:** Although strictly in-memory, the data access logic is separated from the controller logic, preparing the architecture for a smooth transition to Entity Framework Core (EF Core) and a relational database in the next phase.

## 🛡️ Operational Excellence

### 1. Advanced Logging Strategy (Serilog)
To overcome the limitations of the default console logger, we integrated **Serilog** as the primary logging provider.

*   **Structured Logging:** Unlike simple text logging, Serilog records logs as structured data, making it easier to query and analyze in the future.
*   **Multiple Sinks:**
    *   **Console:** Configured for immediate feedback during development.
    *   **File System:** configured with **Rolling Intervals** (daily). This ensures that log files are automatically archived and do not grow indefinitely, which is critical for long-running production applications.
*   **System-Wide Capture:** The logger is initialized at the very start of the application lifecycle (inside `Program.cs`), ensuring that even startup failures and configuration errors are captured.

### 2. Global Error Handling
The API implements a resilient error-handling mechanism that adapts to the environment.

*   **Production Safety:** In production environments, the generic "Developer Exception Page" is disabled to prevent leaking sensitive stack traces.
*   **Standardized Responses (RFC 7807):** We utilize the `ProblemDetails` specification. This ensures that when an error occurs (e.g., HTTP 500), the API returns a standard, machine-readable JSON format containing the error type, title, status, and detail. This allows client applications to handle errors consistent regardless of the error type.

## 🚀 API Capabilities & Features

### Content Negotiation
The API is not limited to JSON. It is configured to respect the `Accept` header from the client.
*   **JSON:** The default format for all responses.
*   **XML:** Full support for XML responses has been enabled for clients that require legacy formats.

### RESTful Maturity
*   **Full CRUD Operations:** Support for `GET`, `POST`, `PUT`, and `DELETE` HTTP verbs.
*   **Partial Updates (PATCH):** The API implements the **JSON Patch (RFC 6902)** standard. This allows clients to send a "patch document" describing changes (e.g., `replace`, `copy`, `remove`) rather than sending the entire resource, optimizing bandwidth for large objects.
*   **Status Codes:** Correct implementation of HTTP status codes (e.g., `201 Created` for inserts, `204 No Content` for updates/deletes, `404 Not Found` for invalid IDs).

### Input Validation
*   **Data Annotations:** Data Transfer Objects (DTOs) are decorated with validation attributes (e.g., `[Required]`, `[MaxLength]`).
*   **Automatic Validation:** The `ApiController` attribute automatically validates the model state and returns a `400 Bad Request` if the input does not meet the defined criteria, reducing boilerplate code in controllers.

## 🔮 Roadmap
The foundation is now set. The next immediate steps for the project include:
1.  **ORM Integration:** replacing the In-Memory store with **Entity Framework Core**.
2.  **Database Connectivity:** Connecting to a real SQL Server or SQLite database.
3.  **Migrations:** Managing database schema changes via code.
