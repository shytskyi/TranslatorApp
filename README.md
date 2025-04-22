# TranslatorApp

A sample ASP.NET Core MVC application with a three‑layer architecture (DAL, BLL, Presentation) and Entity Framework Core.

## Prerequisites

- [.NET 8 SDK]
- SQL Server (local or remote)
- Git

## Setup Instructions

1. **Create the database**  
   Open SQL Server Management Studio (or `sqlcmd`) and create a new empty database named `TranslatorApp`:

   CREATE DATABASE [TranslatorApp];
   GO

2. **Clone the repository**
   git clone https://github.com/shytskyi/test_1.git
   cd TranslatorApp
   
4. **Configure your connection string**
   In PresentationLayer/appsettings.json, find the DefaultConnection entry and replace the Server= value with your SQL Server instance name:
   "Server=YOUR_SQL_SERVER;Database=TranslatorApp;TrustServerCertificate=True;Trusted_Connection=True"
   
6. **Create a new EF Core migration**
   This will scaffold the code‑first migration to create all tables defined in your DbContext:
   Add-Migration InitialCreate
   Update-Database
   
8. **Load the SQL scripts**
   Execute the provided SQL scripts against your TranslatorApp database, in this order:
   - BaseData.sql
   - GetLogsByDateRange.sql
   - GetLogsByMethodAndStatus.sql
   - GetLogsFiltered.sql
    
9. **Run the application**
   
