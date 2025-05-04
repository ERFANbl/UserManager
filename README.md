
# User Manager Web Application

A full-stack application for managing users with location-based filtering, featuring a .NET backend and React-based frontend.

## Features

- ✅ User creation with location data
- 🔍 Filter users by province/city
- 🗑️ Bulk delete functionality
- 📊 Paginated user list
- 📚 Swagger API documentation
- 🔒 CORS security configuration
- 📱 Responsive Tailwind CSS UI

## Technology Stack

**Backend**  
| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core 8|
| ORM | Entity Framework Core |
| Database | SQL Server |
| API Docs | Swagger UI |

**Frontend**  
| Component | Technology |
|-----------|------------|
| UI Library | React (CDN) |
| HTTP Client | Axios |
| Styling | Tailwind CSS |
| JSX Runtime | Babel Standalone |

## Prerequisites

- [ASP.NET Core 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js v16+](https://nodejs.org/)
- [SQL Server 2019+](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Modern web browser

## 🛠️ Installation Guide

### 1. Clone Repository
```bash
git clone https://github.com/your-username/user-manager.git
cd user-manager
```

### 2. Database Configuration
1. Create SQL Server database:
   ```sql
   CREATE DATABASE UserManagerDB;
   ```
2. Update connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=UserManagerDB;Trusted_Connection=True;"
   }
   ```

### 3. Backend Setup
```bash
# Restore packages
dotnet restore

# Run application
dotnet run
```
**Access API at:** `http://localhost:5002`  
**Swagger UI:** `http://localhost:5000/swagger`

### 4. Frontend Setup
```bash
# Install HTTP server (if needed)
npm install -g serve

# Start frontend (from project root)
serve -l 5001
```
**Access UI at:** `http://localhost:5001`

## 🌐 CORS Configuration
Backend allows requests from:
- `http://localhost:5001` (Frontend)
- `http://localhost:5002` (Backend itself)

## 📂 Project Structure
```
user-manager/
├── Program.cs          # Backend entry point
├── appsettings.json    # Configuration file
├── Controllers/        # API controllers
│   ├── UsersController.cs
│   └── LocationController.cs
├── Models/             # Data models
├── Services/           # Business logic
├── Migrations/         # Database migrations
└── index.html          # Frontend application
```

## 🔄 API Endpoints
| Endpoint | Method | Description |
|----------|--------|-------------|
| `/Users` | GET | Get paginated users |
| `/Users/create` | POST | Create new user |
| `/Users/delete` | DELETE | Bulk delete users |
| `/Location/provinces` | GET | List all provinces |
| `/Location/cities` | GET | Get cities by province ID |

## 🚨 Troubleshooting

**Common Issues**  
**CORS Errors:**
- Verify frontend is running on port 5001
- Check CORS policy in `Program.cs`
- Ensure no typos in origin URLs

**Database Issues:**
```bash
# Run EF Core migrations (if applicable)
dotnet ef database update
```
- Confirm SQL Server instance is running
- Verify database permissions

**Frontend Issues:**
- Check browser console for errors
- Ensure backend is running first
- Clear browser cache if UI doesn't update

## 🤝 Contributing
1. Fork the repository
2. Create feature branch: `git checkout -b feature/new-feature`
3. Commit changes: `git commit -m 'Add awesome feature'`
4. Push to branch: `git push origin feature/new-feature`
5. Open a Pull Request
---

**Note:** Replace placeholder values (your-username, database credentials) with actual values before deployment.
```
