# TaskFlow Backend API

A robust, scalable RESTful API built with .NET 8 for the TaskFlow task management application.

## 🚀 Tech Stack

- **.NET 8** - Latest .NET framework with improved performance
- **ASP.NET Core Web API** - Modern web API framework
- **Entity Framework Core** - Object-relational mapping (ORM)
- **SQL Server** - Primary database (with SQLite support for development)
- **JWT Authentication** - Secure token-based authentication
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Input validation
- **Swagger/OpenAPI** - API documentation

## 📋 Features

- 🔐 JWT-based authentication and authorization
- 📝 CRUD operations for tasks, categories, and users
- 🔍 Advanced filtering and searching
- 🗄️ Database migrations and seeding
- 📚 Comprehensive API documentation
- 🛡️ Security middleware

## 🛠️ Prerequisites

Before running this project, make sure you have:

- **.NET 8 SDK** (version 8.0 or higher)
- **SQL Server** (Local DB, Express, or full version)
- **Visual Studio 2022** or **VS Code** with C# extension

## 🚀 Getting Started

### Installation

1. Clone the repository:
```bash
git clone [<repository-url>](https://github.com/nokfur/task-flow-be.git)
cd task-flow-be
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Set up the database connection string in `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);uid=sa;pwd=12345;database=TaskManagement;TrustServerCertificate=True"
  }
}
```

4. Apply database migrations:
```bash
cd TaskManagement
dotnet ef database update
```

### Development

Run the application:
```bash
dotnet run --project TaskManagement
```

Or with hot reload:
```bash
dotnet watch run --project TaskManagement
```

The API will be available at:
- HTTP: `http://localhost:5119`
- HTTPS: `https://localhost:7165`
- Swagger UI: `http://localhost:5119/swagger/index.html`

## 🗄️ Database Schema

### Core Entities

**User**
- Id (Guid, PK)
- Name (string)
- Email (string, unique)
- Password (string, hashed)
- Salt (string)
- CreatedAt (DateTime)
- Role (enum: User, Admin)

**Board**
- Id (Guid, PK)
- Title (string)
- Description (string)
- OwnerId (Guid, FK → User.Id)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
- IsTemplate (bool)

**BoardMember**
- BoardId (Guid, FK → Board.Id)
- MemberId (Guid, FK → User.Id)
- Role (enum: Owner, Admin, Member, Viewer)
- *Composite Primary Key (BoardId, MemberId)*

**Column**
- Id (Guid, PK)
- Title (string)
- Position (int)
- BoardId (Guid, FK → Board.Id)

**Task**
- Id (Guid, PK)
- Title (string)
- Position (int)
- Priority (enum: Low, Medium, High, Critical)
- DueDate (DateTime?)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
- ColumnId (Guid, FK → Column.Id)
- Description (string)

**Label**
- Id (Guid, PK)
- Name (string)
- Color (string, hex color)
- BoardId (Guid, FK → Board.Id)

**TaskLabel**
- TaskId (Guid, FK → Task.Id)
- LabelId (Guid, FK → Label.Id)
- *Composite Primary Key (TaskId, LabelId)*

### Protected Endpoints
Most endpoints require authentication. Include the JWT token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

### Roles
- **User**: Basic task management operations
- **Admin**: User, template management, and system administration

## 🔧 Configuration

### appsettings.Development.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(local);uid=sa;pwd=12345;database=TaskManagement;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Key": "b5aec850e7e188935e6832264e527945aef42149aa8567b64028f6b42decb66a",
    "Issuer": "TaskManagement",
    "Audience": "TaskManagement",
    "ExpiryInMinutes": 120
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Environment Variables
For production, use environment variables:
```bash
export ConnectionStrings__DefaultConnection="your-production-connection-string"
export JwtSettings__SecretKey="your-production-secret-key"
export EmailSettings__SenderPassword="your-email-password"
```

## 📦 Available Commands

### Database Operations
```bash
# Add new migration
cd BusinessObjects
dotnet ef migrations add <MigrationName> --startup-project ../TaskManagement/

# Update database
cd TaskManagement
dotnet ef database update

# Drop database
cd TaskManagement
dotnet ef database drop

# Generate SQL script
dotnet ef migrations script
```

### Development
```bash
# Run with hot reload
dotnet watch run

# Build solution
dotnet build

# Publish for production
dotnet publish -c Release -o ./publish
```

## 🛡️ Security

### Implemented Security Measures
- JWT token authentication
- Password hashing
- SQL injection prevention (EF Core)
- CORS policy enforcement
- Input validation
- HTTPS enforcement
- Security headers middleware

### Common Issues

**Database connection fails:**
- Check connection string format
- Ensure SQL Server is running
- Verify database exists (run migrations)

**JWT authentication not working:**
- Check secret key length (minimum 256 bits)
- Verify token expiry settings
- Ensure proper Authorization header format

**Migration errors:**
- Delete migrations folder and recreate
- Check for model conflicts
- Ensure database is accessible

**Performance issues:**
- Enable query logging to identify slow queries
- Check database indexes
- Monitor memory usage
- Review N+1 query problems
---

Built with ❤️ by the TaskFlow Team
