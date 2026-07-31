# Sports Facility Management System

A comprehensive, full-stack .NET 10 solution designed to manage sports facilities, bookings, memberships, and class schedules.

## Architecture
This project is divided into two main components:
1. **Frontend (`/frontend/SportsFacility.Frontend`)**: Built using Blazor WebAssembly / Auto mode. It uses `MudBlazor` for a clean, rich, and responsive user interface, featuring sortable, filterable grids for all data management.
2. **Backend (`/Backend`)**: A clean-architecture .NET 10 Web API. It includes:
   - **SportsFacility.API**: The presentation layer containing controllers (`ActivitiesController`, `PlansController`, etc.) and `AutoMapper` profiles.
   - **SportsFacility.Entity**: The core domain models (Entities).
   - **SportsFacility.DTO**: Data Transfer Objects used to shape data for the Blazor frontend.
   - **SportsFacility.Infrastructure**: EF Core `ApplicationDbContext` and database configurations.
   - **SportsFacility.Domain**: Core services and business logic interfaces.

## Prerequisites
- .NET 10.0 SDK
- SQL Server (LocalDB `(localdb)\mssqllocaldb` is configured by default)

## Getting Started Locally

### 1. Database Setup & Seeding
The application is pre-configured to use SQL Server LocalDB. To set up the database and apply migrations:
```powershell
cd Backend
dotnet ef database update --project SportsFacility.Infrastructure --startup-project SportsFacility.API
```

If you wish to seed the database with initial test data (Facilities, Plans, Users), you can run the SQL script provided in `seedata.txt` directly in SQL Server Management Studio (SSMS), Azure Data Studio, or Visual Studio SQL Object Explorer against the `SportsFacilityDB` database.

### 2. Running the Backend API
```powershell
cd Backend
dotnet run --project SportsFacility.API/SportsFacility.API.csproj
```
The API will launch (usually on `http://localhost:5228` and `https://localhost:7147`). You can browse the Swagger UI at `http://localhost:5228/swagger`.

### 3. Running the Frontend
In a new terminal window:
```powershell
cd frontend
dotnet run --project SportsFacility.Frontend/SportsFacility.Frontend.csproj
```
The frontend will connect to the backend automatically. Note that the frontend defaults to using the local mock JSON files if `DataSourceMode` is set to `"Local"` in the `appsettings.json`. To use the live API, change `DataSourceMode` to `"API"` inside `SportsFacility.Frontend/wwwroot/appsettings.json` (or wherever configuration is set).

## Deployment (Hostinger VPS via GitHub Actions)
A GitHub Actions workflow is provided in `.github/workflows/deploy.yml`. To deploy to your Hostinger VPS automatically on `push` to `main`:

1. Go to your GitHub Repository -> **Settings** -> **Secrets and variables** -> **Actions**.
2. Add the following repository secrets:
   - `HOSTINGER_IP`: The IP address of your VPS.
   - `HOSTINGER_USERNAME`: The SSH user (e.g., `root`).
   - `HOSTINGER_PASSWORD`: The SSH password.
3. Upon push to `main`, GitHub Actions will build both projects, publish them, securely SCP them to `/var/www/sportsfacility`, and restart the `sportsfacility-api.service`.
*(Ensure you have systemd configured on your Hostinger VPS to run the API).*
