# ZStore.net

ZStore.net is an e-commerce platform designed to manage product categories, user accounts, orders, and more. Built using **ASP.NET Core**, it provides a scalable and maintainable architecture with a focus on performance and modularity.

## Table of Contents
- [ZStore.net](#zstorenet)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Technology Stack](#technology-stack)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Clone the Repository](#clone-the-repository)
    - [Setup Configuration](#setup-configuration)
    - [Database Setup](#database-setup)
    - [Running the Application](#running-the-application)
  - [Docker Deployment](#docker-deployment)
  - [Contributing](#contributing)
  - [License](#license)

## Features
- Admin Panel for managing products, categories, and users
- User authentication and authorization
- Shopping cart functionality
- Order management
- Customizable storage options

## Technology Stack
- **Backend:** ASP.NET Core
- **Database:** Entity Framework Core with SQL Server
- **ORM:** Entity Framework Core
- **Frontend:** Razor Pages (for now, expandable to Angular/React)
- **Hosting:** Dockerized for easy deployment

## Getting Started

### Prerequisites
- .NET 6 or later installed
- SQL Server or any other database of choice configured
- Visual Studio or VS Code
- Docker (optional, for containerized deployment)

### Clone the Repository
  ```bash
    git clone https://github.com/ismailza/zstore.net.git
  ```
### Setup Configuration
1. Open the project in Visual Studio or VS Code
2. Create a file named appsettings.json in the root directory:
  ```bash
    cp appsettings.Example.json appsettings.json
  ```
3. Create a connection string named ZStoreConnection in the appsettings.json file and replace the value with your database connection string:
  ```json
    "ConnectionStrings": {
      "ZStoreConnection": "Server=localhost,1433;Database=dotnetcore;User Id=sa;Password=;Trusted_Connection=True;TrustServerCertificate=True;Integrated Security=False;MultipleActiveResultSets=true"
    }
  ```
4. You can also configure other settings in the appsettings.json file, such as storage options, etc.
   - For the storage options, you can choose between LocalStorage and other cloud storage providers like Azure Blob Storage, AWS S3, etc.
   - For Cart storage, you can choose between Session storage and Cookie storage.

### Database Setup
The project use SQL Server as the default database provider. You can change the database provider by modifying the connection string in the appsettings.json file.

You can use docker compose to run the SQL Server container:
  ```bash
    docker-compose up -d
  ```
If you are using SQL Server, you can create the database by running the following commands in the Package Manager Console:
  ```bash
    dotnet ef database update
  ```

### Running the Application
1. Build and run the application:
  ```bash
    dotnet run
  ```
2. Open your browser and navigate to https://localhost:5079

## Docker Deployment
1. Build the Docker image:
  ```bash
    docker build -t zstore.net .
  ```
2. Run the Docker container:
  ```bash
    docker run -d -p 5079:80 zstore.net
  ```
3. Open your browser and navigate to https://localhost:5079

## Contributing
Feel free to fork this repository and submit pull requests. All contributions are welcome.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
