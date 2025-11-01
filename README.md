# Camden Car Park – Employee Car Approval Application

## Table of Contents
- [Overview](#overview)
- [Application Architecture](#application-architecture)
- [Running the Application](#running-the-application)
  - [Method 1: Using the Batch File](#method-1-using-the-batch-file)
  - [Method 2: Using Visual Studio](#method-2-using-visual-studio)

## Overview

### Objective
Camden Group offers on-site car parking at its Steeple location to current employees.
Management would like to keep a record of all the cars that are currently using this facility
and have an approval system for new cars.

This application allows users to submit employee requests for approval to park
their car in the on-site car park. The application has a front-end user interface that
communicates with a C# backend API.

### Features
#### Front-End
- **Car List:** Display a list of Cars with the following details:
  - Employee Name
  - Car Registration Number
  - Car Make
  - Car Model
  - Colour
  - Year
  - Approved Status
  - Date Approved
- **Add Car Form:** A form to add a new car with all necessary fields
- **Edit Car Form:** Edit existing vehicle car park requests

#### Back-End
- Built using ASP.NET Core
- Uses Entity Framework Core with SQLite for data storage
- Includes car registration validation and other relevant business logic

## Application Architecture

This application uses a multi-tier architecture:

### Frontend
- Built using Blazor WebAssembly
- Provides the user interface for interacting with the car park system

### Backend
- .NET API providing the backend services
- Handles all business logic and data operations

### Database
- SQLite database included within the API project folder
- Pre-configured and included in the repository, no additional database setup required

## Running the Application

There are two ways to run this application:

### Method 1: Using the Batch File

1. Navigate to the root directory of the project
2. Run the `run-applications.bat` file by double-clicking it or executing it from the command line:
   ```
   .\run-applications.bat
   ```

### Method 2: Using Visual Studio

1. Open the `Camden-Car-Park.sln` solution in Visual Studio
2. In the toolbar, click on the dropdown next to the run button and select the 'Complete' custom startup profile
3. Click the Run button or press F5 to start the application

Note: Make sure you have all the necessary dependencies installed and the correct .NET version as specified in the project files

