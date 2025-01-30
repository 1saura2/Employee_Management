# Employee Management System

Objective:

The Employee Management System is a basic web application built with 
.NET Core MVC, SQL Server, and Dapper for data access.
It enables users to manage employees and departments effectively.


## Technologies Used:

Backend:.NET Core 6/8 MVC.

Frontend includes Razor Views, Bootstrap, jQuery, and JavaScript.

Database: SQL Server Management studio 20.

ORM: Used Dapper.

Validation: Integrated model validation


## Features

Employee Management:

Create: Add a new employee.

Read: Show a paginated personnel list with search and filtering options.

Update: Change employee information.

Delete: Delete an employee (soft delete)

## Department Management

List: View all departments.

Add: Create a new department.

## Additional Functionalities

View Details: Show employee details, including the department.

Search & Filter: Search employees by name and filter by department.

Dapper ORM: Used for data access via parameterized queries for prevention of SQL injection.

## Setup Instructions

Prerequisites:

Install .NET SDK (Core 6/8)

Install SQL Server and SQL Server Management Studio 20 (SSMS)

Install Visual Studio (with ASP.NET and web development workload)

## Database Setup

You can find the SQL setup script for the database in the [database_setup.sql](CREATE_TABLES_SQL.txt) file.
