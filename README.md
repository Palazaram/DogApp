# DogApp

# Project architecture
## Models

In the `Models` folder, you'll find a `Dog` model and DogDTO model
- Dog Model: The Models folder contains the Dog model, which represents the core data structure for managing information about dogs.
- DTO Model: Inside the DTO folder, you will find the DogDTO model.

## Repositories

`DogRepository`: Located within the `Repositories` folder, the `DogRepository` class is responsible for data access. It utilizes the principles of Dependency Injection for robust and flexible data management. This class depends on the `IDog` interface and the `DogManager` service for effective data operations.

## Data

`ApplicationDbContext`: The `Data` folder houses the `ApplicationDbContext` class, which acts as the database context for the application. It handles interactions with the underlying database and is essential for data persistence.
`Migrations`: Within the `Migrations` subfolder, you can find database migration files. These files are utilized to update the database schema when structural changes occur.

## Controllers

`DogController`: The `Controllers` folder contains the `DogController`, which serves as the entry point for handling HTTP requests and managing interactions with the API. This controller exposes endpoints for managing dog-related data.

## ConfigureClasses

DogConfiguration: The `ConfigureClasses` folder includes the `DogConfiguration` class. This class is used for configuring the database table and can be crucial for tasks such as database schema migrations and custom schema management.

## Request Limits
Request rate limiting is implemented in the `Program.cs` file and is also configured in the `appsettings.json` file. This feature ensures that the API operates within defined rate limits to prevent abuse and maintain stability.

## Testing

The project includes a comprehensive testing suite that covers various aspects of the application, including `MS` `NUnit` and `xUnit` tests. These tests help verify the reliability and correctness of the implemented functionality.

## Dependencies

The project may rely on various libraries, frameworks, and NuGet packages to provide its functionality. Ensure that all dependencies are installed before building and running the project.
