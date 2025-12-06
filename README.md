# restapi-crud-practice
ASP.NET Core CRUD REST API practice project (with JWT authentication system)

# Required
- Windows OS for guaranteed support
- .NET SDK 9.0 for development and project code support
- Visual Studio 2022 for the IDE
- Postman or Insomnia for API Calls (if you dont want to use Swagger UI)
- PostgreSQL Server for database
- Git for version control and simple repository cloning

# Getting Started
1. Clone the repository:
`https://github.com/matisslaksevics/restapi-crud-practice.git`
2.Download and Install PostgreSQL Server and install it. Download link:
`https://www.postgresql.org/download`
3. Note down the admin/superuser credentials (username and password) during the installation process. If you already have PostgreSQL installed then write down the credentials you use to access the database.<br>
 For example:
   - username is `postgres`
   - password is `admin`
   - Default port is `5432`
   - Default database name is `CrudAPI`
4. Open Visual Studio 2022 and open the cloned repository folder (Select to open the solution file with the .sln extension).
   - If you are using Visual Studio for the first time, you may need to install the required workloads for ASP.NET Core development.
5. Open the `appsettings.json` file in the project and update the connection string with your PostgreSQL credentials.
	- If you wish to use an existing database then change the `Database` value to the name of your existing database.
	- If you want to create a new database then leave it as is.
6. Get the necessary packages by running the following commands in a PowerShell terminal within Visual Studio:
`dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`
`dotnet add package Microsoft.EntityFrameworkCore.Design`
7. Afterwards create the required database (or update an existing one) by running the following command in a PowerShell terminal within Visual Studio:
`dotnet ef database update`
8. Launch Insomnia or Postman (if you dont want to use Swagger UI to make API calls after the application is running.)
9. Launch the application in `http` mode or use the following command in a PowerShell terminal within Visual Studio:
`dotnet run`

# Creating API calls
The default URL for the API is: `http://localhost:5108/`

In the older version without the auth system there were shown instructions on how to make proper API calls. But in the current version Swagger is implemented.<br>
So to make API calls you can use Swagger UI by navigating to `http://localhost:5108/swagger/index.html` in your web browser.<br>
There you can see all the available endpoints and make API calls directly from the browser.

!!! It is required to register and login before making any API calls - since every single endpoint except the login and register ones require authentication !!!

localStorage.clear();
window.location.reload();