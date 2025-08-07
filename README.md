# restapi-crud-practice
ASP.NET Core CRUD REST API practice project

# Required
- Windows OS for guaranteed support
- .NET SDK 9.0 for development and project code support
- Visual Studio 2022 for the IDE
- Postman or Insomnia for API Calls
- PostgreSQL Server for database
- Git for version control and repository cloning

# Getting Started
1. Clone the repository:
`https://github.com/matisslaksevics/restapi-crud-practice.git`
2. Make sure to have PostgreSQL Server installed and running. Download link:
`https://www.postgresql.org/download`
3. Get the necessary packages by running the following commands in a PowerShell terminal within Visual Studio:
`dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`
`dotnet add package Microsoft.EntityFrameworkCore.Design`
4. Make sure PostgreSQL Server is set up and is running, then create the required database running the following command in a PowerShell terminal within Visual Studio:
`dotnet ef database update`
5. Launch Insomnia or Postman
6. Launch the application in `http` mode or use the following command in a PowerShell terminal within Visual Studio:
`dotnet run`

# Creating API calls
The default URL for the API is: `http://localhost:5108/`
## Clients
For client related API calls use the follow URL: `http://localhost:5108/clients/`
For creating and updating client rows in the database table you will require the following JSON structure:
```
{
	"FirstName" : "",
	"LastName" : "",
	"Email" : ""
}
```

In order to create and run a POST or PUT request you will need to:
- Add the specific structure in the body
- Add a header Content-Type : application/json
- For PUT/update calls you need to provide a specific ID

`GET http://localhost:5108/clients/` - returns all clients in the database.
`GET http://localhost:5108/clients/1` - returns a specific client (in this case client with ID of 1).
`POST http://localhost:5108/clients` - creates a new client (with using the provided header and JSON structure).
`PUT http://localhost:5108/clients/1` - updates a specific client (in this case client with ID of 1).
`DELETE http://localhost:5108/clients/1` - deletes a specific client (in this case client with ID of 1).

## Books
For book related API calls use the follow URL: `http://localhost:5108/books/`
For creating and updating book rows in the database table you will require the following JSON structure:
```
{
	"BookName": "",
	"ReleaseDate": ""
}
```

In order to create and run a POST or PUT request you will need to:
- Add the specific structure in the body
- Add a header Content-Type : application/json
- For PUT/update calls you need to provide a specific ID

`GET http://localhost:5108/books/` - returns all books in the database.
`GET http://localhost:5108/books/1` - returns a specific book (in this case book with ID of 1).
`POST http://localhost:5108/books` - creates a new book (with using the provided header and JSON structure).
`PUT http://localhost:5108/books/1` - updates a specific book (in this case book with ID of 1).
`DELETE http://localhost:5108/books/1` - deletes a specific book (in this case book with ID of 1).

## Borrows
For borrowing related API calls use the follow URL: `http://localhost:5108/borrows/`
For creating and updating borrow rows in the database table you will require the following JSON structure:
```
{
	"ClientId" : ,
	"BookId" : ,
	"BorrowDate" : "",
	"ReturnDate" : ""
}
```

In order to create and run a POST or PUT request you will need to:
- Add the specific structure in the body
- Add a header Content-Type : application/json
- For PUT/update calls you need to provide a specific ID
- Provide valid client and book IDs that exist within the database

  `GET http://localhost:5108/borrows/` - returns all borrows that have overdue returns in the database.
  `GET http://localhost:5108/borrows/1` - returns a specific borrow (in this case borrow with ID of 1).
  `POST http://localhost:5108/borrows` - creates a new borrow (with using the provided header and JSON structure).
  `PUT http://localhost:5108/borrows/1` - updates a specific borrow (in this case borrow with ID of 1). Also depending on the given dates overdue status will be updated automatically.
  `DELETE http://localhost:5108/borrows/1` - deletes a specific borrow (in this case borrow with ID of 1).


