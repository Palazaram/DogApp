# DogApp

# Project architecture
## Models

Inside the 'Models' folder, there are two models: 'Dog' and 'DogDTO.'
- The `Dog` model represents the core data structure for managing information about dogs.
- The `DogDTO` model, located in the 'DTO' folder, serves as a Data Transfer Object for data exchange between the client and the server, ensuring a clear separation of data and presentation layers.

## Repositories

**DogRepository**: Located in the 'Repositories' folder. It utilizes the principles of Dependency Injection for robust and flexible data management.
- In the `IDog` interface which contains methods for working with elemets (dogs), such as retrieving a list, adding, and editing.
- In the `DogManager` service has been implemented, which implements the `IDog` interface and provides the business logic for managing notes. 

## Data

**ApplicationDbContext**: In the `Data` folder you'll find the `ApplicationDbContext` class, which acts as the database context for the application. It handles interactions with the underlying database and is essential for data persistence.
`Migrations`: Within the `Migrations` subfolder, you can find database migration files. These files are utilized to update the database schema when structural changes occur.

## Controllers

**DogController**: The `Controllers` folder contains the `DogController`, which serves as the entry point for handling HTTP requests and managing interactions with the API. This controller exposes endpoints for managing dog-related data.

## ConfigureClasses

**DogConfiguration**: The `ConfigureClasses` folder includes the `DogConfiguration` class. This class is used for configuring the database table and can be crucial for tasks such as database schema migrations and custom schema management.

## Request Limits
Request rate limiting is implemented in the `Program.cs` file and is also configured in the `appsettings.json` file. This feature ensures that the API operates within defined rate limits to prevent abuse and maintain stability.

## Testing

The project includes a comprehensive testing suite that covers various aspects of the application, including `MS` `NUnit` and `xUnit` tests. These tests help verify the reliability and correctness of the implemented functionality.

## Dependencies

The project may rely on various libraries, frameworks, and NuGet packages to provide its functionality. Ensure that all dependencies are installed before building and running the project.
