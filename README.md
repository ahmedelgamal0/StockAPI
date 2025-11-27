# StockAPI

## Project Description
This is a stock market API project that allows users to manage stocks, comments, and user accounts. It provides functionalities for authentication, data retrieval, and data manipulation related to stock market information.

## Technologies Used
- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Authentication

## Setup Instructions

### Prerequisites
- .NET SDK (version 9.0 or later)
- SQL Server
- Docker (optional, for docker-compose)

### Database Setup
1. Update the connection string in `api/appsettings.json` and `api/appsettings.Development.json` to point to your SQL Server instance.
2. Navigate to the `api` directory:
   ```bash
   cd api
   ```
3. Apply Entity Framework Core migrations to create the database schema:
   ```bash
   dotnet ef database update
   ```

### Running the Application

#### Using .NET CLI
1. Navigate to the `api` directory:
   ```bash
   cd api
   ```
2. Run the application:
   ```bash
   dotnet run
   ```
   The API will typically run on `https://localhost:7041` and `http://localhost:5289`.

#### Using Docker Compose (if docker-compose.yml is configured)
1. Navigate to the `api` directory where `docker-compose.yml` is located:
   ```bash
   cd api
   ```
2. Build and run the Docker containers:
   ```bash
   docker-compose up --build
   ```

## API Endpoints
The API documentation (Swagger UI) will be available at `/swagger` when the application is running (e.g., `https://localhost:7041/swagger`).

### Authentication
- `POST /api/account/register`: Register a new user.
- `POST /api/account/login`: Log in an existing user and get a JWT token.

### Stocks
- `GET /api/stock`: Get a list of stocks.
- `GET /api/stock/{id}`: Get a single stock by ID.
- `POST /api/stock`: Create a new stock. (Requires authentication)
- `PUT /api/stock/{id}`: Update an existing stock. (Requires authentication)
- `DELETE /api/stock/{id}`: Delete a stock. (Requires authentication)

### Comments
- `GET /api/comment`: Get a list of comments.
- `GET /api/comment/{id}`: Get a single comment by ID.
- `POST /api/comment/{stockId}`: Create a new comment for a stock. (Requires authentication)
- `PUT /api/comment/{id}`: Update an existing comment. (Requires authentication)
- `DELETE /api/comment/{id}`: Delete a comment. (Requires authentication)

## Project Structure
- `Controllers/`: API controllers.
- `Data/`: Database context.
- `Dtos/`: Data Transfer Objects for requests and responses.
- `Extensions/`: Extension methods.
- `Helpers/`: Helper classes.
- `Interfaces/`: Repository and service interfaces.
- `Mappers/`: Mapping logic between models and DTOs.
- `Migrations/`: Entity Framework Core database migrations.
- `Models/`: Entity models.
- `Repository/`: Concrete implementations of repository interfaces.
- `Services/`: Business logic services.
