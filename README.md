# Nowadays Issue Tracking System

## Overview
Nowadays is an issue tracking system designed to help organizations manage tasks and projects efficiently. Similar to Jira, it provides functionalities for managing companies, projects, employees, and issues.

- **Application Technical Infrastructure**
  - .NET Core
  - RestAPI (SwaggerUI)
  - Repository/UnitOfWork Pattern
  - SOLID Principles
  - EF Core (CodeFirst)

    

- **Additional Features**
  - JWT and Refresh Token: Implemented JWT Token and Refresh Token mechanisms.
  - .NET Identity Integration: Includes endpoints for Login, Register, Confirm Email, Forgot-Reset Password, and Login-2FA.
  - Exception Handling: Robust exception handling mechanism.
  - AutoMapper and FluentValidation: Used for mapping and validation.
  - Serilog Logging: Integrated Serilog for logging exceptions, with logs saved in JSON and TXT formats.
  - Database Seeding with Bogus: Utilized Bogus library to seed the database with fake test data for easier testing.


## Scenario
The Nowadays system will manage:

- Companies: Add, Delete, Update companies.
- Projects: Add, Delete, Update, Assign projects to companies.
- Employees: Add, Delete, Update, Assign employees to projects.
- Issues: Add, Delete, Update, Assign issues to employees.
- Reporting: Generate various reports.
- RESTful Services


- **The API will include the following services**
- Company Service: Manage companies.
- Project Service: Manage projects.
- Employee Service: Manage employees.
- Issue Service: Manage issues.
- Report Service: Generate reports.


- **Implementation Details**
- Interface/Abstract Class Usage: The existing infrastructure includes interfaces and abstract classes for better code organization and reusability.
- Identity Verification: Employee identity verification through TC (Turkish Citizenship) number validation using the provided web service.
- Getting Started


## Installation and Usage

To clone the project to your local environment:

```bash
git clone https://github.com/BatuhanKayaoglu/Nowadays.git

