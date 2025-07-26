# bsStoreApp - Book Store Web API

This project is a Book Store application developed using ASP.NET Core Web API, following the principles of layered architecture. The project incorporates modern web API development techniques and best practices.

---

## ✨ Features

- **Layered Architecture:** Designed in accordance with the principle of separation of responsibilities (Entities, Repositories, Services, Presentation, WebApi).
- **Repository Pattern:** Abstracts the data access logic, centralizing database operations.
- **JWT Authentication:** Secure user authentication and authorization processes using JSON Web Tokens (JWT).
- **API Versioning:** Supports different API versions (v1, v2).
- **Content Negotiation:** Ability to return responses in XML and custom CSV formats based on the `Accept` header.
- **Swagger/OpenAPI:** Automatic documentation of API endpoints and a testable interface.
- **HATEOAS Support:** Makes the API more discoverable by adding relevant links to the API responses.
- **Data Shaping:** Allows the client to dynamically select the fields they want.
- **Advanced Filtering and Pagination:** Filtering, sorting, and paging of data with request parameters.
- **Global Error Handling:** A central and consistent error handling mechanism throughout the application.
- **Advanced Logging with NLog:** Detailed logging of requests and errors.
- **Rate Limiting and Caching:** Limiting incoming requests to the API and caching frequently used data to improve performance.
- **Entity Framework Core:** Database management with the Code-First approach.

---

## 🛠️ Technologies Used

- **Backend:** ASP.NET Core 8
- **Database:** Entity Framework Core 8, Microsoft SQL Server
- **Logging:** NLog
- **API Documentation:** Swashbuckle (Swagger)
- **Other Libraries:** AutoMapper, AspNetCoreRateLimit

---

## 🚀 Getting Started

Follow the steps below to run the project on your local machine.

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Installation

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/your-username/bsStoreApp.git
    ```

2.  **Navigate to the project directory:**
    ```sh
    cd bsStoreApp/WebApi
    ```

3.  **Create the Configuration File:**
    The project reads sensitive information (database connection string, JWT key, etc.) from the `appsettings.json` file. This file is not included in the repository for security reasons. Create a file named `appsettings.json` in the `WebApi` folder and fill it with your own information using the template below:

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "ConnectionStrings": {
        "sqlConnection": "Server=(localdb)\\mssqllocaldb; Database=bsStoreAppDB; Trusted_Connection=True;"
      },
      "JwtSettings": {
        "ValidIssuer": "bsStoreApp",
        "ValidAudience": "https://localhost:7102",
        "Secret": "ThisShouldBeASecretAndLongKeyOtherwiseTheApplicationWillNotBeSecure"
      }
    }
    ```

4.  **Create the Database:**
    Create the database by applying the Entity Framework Core migrations.
    ```sh
    dotnet ef database update
    ```

5.  **Run the Application:**
    ```sh
    dotnet run
    ```

---

## 📖 API Usage

After running the application, you can go to the Swagger interface to view all endpoints, test them, and get information about the models:

- **Swagger UI:** `https://localhost:7102/swagger`

---

## 📂 Project Architecture

The project consists of the following layers:

-   **Entities:** Contains the basic entities (models), DTOs, and error models of the application.
-   **Repositories:** The data access layer. Manages database queries and operations.
-   **Services:** The business logic layer. Performs operations on the data using the repositories.
-   **Presentation:** Contains the API endpoints (Controllers) and action filters.
-   **WebApi:** The starting point of the project. This is where services and middleware are configured.

---

## 🤝 Contributing

Your contributions will make the project better! Please feel free to open a pull request or create an issue.

1.  Fork the Project.
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`).
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`).
4.  Push to the Branch (`git push origin feature/AmazingFeature`).
5.  Open a Pull Request.

---

## 📄 License

This project is licensed under the MIT License. See the `LICENSE` file for more information.
