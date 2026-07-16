# Lead To Opportunity Management

A comprehensive web application designed to manage the lifecycle of leads converting into opportunities. The system features a modern, responsive Angular frontend coupled with a robust .NET 10 backend API.

---

## 🏗️ Architecture & Technologies

- **Frontend:** Angular v21, Bootstrap 5
- **Backend:** .NET 10 (ASP.NET Core Web API, Entity Framework Core)
- **Database:** MySQL
- **Containerization:** Docker & Docker Compose

---

## 🐳 Docker Compose (Recommended)

The easiest way to build and run the entire application (frontend + backend) in an isolated environment is using Docker Compose.

1. Make sure [Docker Desktop](https://www.docker.com/products/docker-desktop) is running.
2. From the root directory of the project (`CapStoneProject`), run:
   ```bash
   docker-compose up --build
   ```

### Docker Services:
- **Frontend Container:** Runs the Angular app on port `4200`.
- **Backend Container:** Runs the .NET API on port `5295`.

> **Note:** The backend Docker container is configured to connect to your host machine's MySQL instance using `host.docker.internal`. Ensure your local MySQL server is running and accessible on port `3306` with credentials `root`/`root`.

---

## ⚙️ Backend Setup (.NET 10 API)

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [MySQL Server](https://dev.mysql.com/downloads/installer/)

### Database Configuration
By default, the backend expects a local MySQL instance with the following credentials:
- **Server:** `localhost`
- **Port:** `3306`
- **Database:** `LeadToOpportunityDB`
- **User:** `root`
- **Password:** `root`

*(You can modify these settings in `LeadToOpportunityManagement/LeadToOpportunity.API/appsettings.json`)*

### Build & Run
1. Navigate to the API project directory:
   ```bash
   cd LeadToOpportunityManagement/LeadToOpportunity.API
   ```
2. Build the project:
   ```bash
   dotnet build
   ```
3. Apply database migrations (if applicable):
   ```bash
   dotnet ef database update
   ```
4. Run the API:
   ```bash
   dotnet run
   ```
   The backend will be available at `http://localhost:5295`.

### Testing
Navigate to the test project directory and run:
```bash
cd ../LeadToOpportunity.Tests
dotnet test
```
