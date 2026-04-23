# Smart Library Management System with IoT Book Locator

## Overview
The Smart Library Management System is designed to streamline the management of library resources, providing an efficient way to locate books using IoT technology. This system allows users to search for books, view their availability, and manage library operations effectively.

## Features
- **Book Management**: Add, update, and delete book records.
- **IoT Book Locator**: Utilize IoT technology to locate books within the library.
- **User-Friendly Interface**: A clean and intuitive web interface for easy navigation.
- **Search Functionality**: Quickly find books by title, author, or category.
- **Responsive Design**: Optimized for both desktop and mobile devices.

## Project Structure
- **SmartLibraryManagementSystem.Web**: Contains the web application, including controllers, models, views, and static files.
- **SmartLibraryManagementSystem.Business**: Contains business logic and service interfaces for managing books.
- **SmartLibraryManagementSystem.Data**: Contains data access logic, including entity definitions and repositories.
- **SmartLibraryManagementSystem.Tests**: Contains unit tests for business logic, data access, and web controllers.

## Setup Instructions
1. **Clone the Repository**: 
   ```
   git clone <repository-url>
   cd SmartLibraryManagementSystem
   ```

2. **Install Dependencies**: 
   Use the .NET CLI to restore the required packages.
   ```
   dotnet restore
   ```

3. **Configure Database**: 
   Update the `appsettings.json` file with your SQL Server connection string.

4. **Run Migrations**: 
   Apply database migrations to set up the initial database schema.
   ```
   dotnet ef database update
   ```

5. **Run the Application**: 
   Start the application using the following command:
   ```
   dotnet run --project SmartLibraryManagementSystem.Web
   ```

6. **Access the Application**: 
   Open your web browser and navigate to `http://localhost:5000` to access the Smart Library Management System.

## Contributing
Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.