# SecurePrivacyTasks

** Table of Contents
- [Task 1: User Management Application with GDPR Considerations](#task-1-user-management-application-with-gdpr-considerations)
- [Task 2: Binary String Analysis](#secureprivacytasks---task-2-binary-string-analysis)


# Task 1 - User management application demonstrating key CRUD functionalities while incorporating critical GDPR considerations, such as cookie consent, data access rights, and secure authentication.
 
## Table of Contents

- [Overview](#overview)
- [Key Features](#key-features)
- [GDPR Considerations](#gdpr-considerations)
- [CRUD Operations](#crud-operations)
- [How to Run](#how-to-run)
- [Technologies Used](#technologies-used)

## Overview

SecurePrivacyTasks is built with **Angular** on the frontend and **.NET** on the backend, using **MongoDB** for data storage. The application provides the ability to create, update, and delete users, along with features focused on GDPR compliance, including cookie management, user consent, and the right to access personal data.

## Key Features

- **User Authentication:** Cookie-based authentication, session handling with support for login and registration.
- **CRUD Operations:** Allows for user creation, updating, listing, and deletion with permissions.
- **Role-based Permissions:** Users can be assigned permissions to create, edit, or delete other users.
- **GDPR Compliance:**
  - Cookie Consent Management.
  - Right to access personal data (via the "Access My Data" feature).
  - Profile management with the ability to download personal data in JSON format.

## GDPR Considerations

The application includes key features to ensure GDPR compliance:

### 1. **Cookie Consent:**
   - Users must accept or decline cookies when first interacting with the site.
   - Cookie management tools are available, allowing users to manage or clear non-essential cookies at any time.
   - Non-essential cookies such as analytics and ad-preferences are stored only after consent.

### 2. **Right to Access:**
   - Users can view and download their personal data through the "My Profile" page.
   - The `access-my-data` feature allows users to request a summary of all data stored about them, providing transparency over stored information.

### 3. **Profile Privacy:**
   - User details (including email, address, phone) are handled securely.
   - Hashed passwords are never returned in any API responses.
   
### 4. **Data Deletion & Consent:**
   - Consent for data storage is requested during user registration and stored in the backend.
   - Consent for cookies is managed via local storage and backend updates, ensuring GDPR compliance.

## CRUD Operations

The core of the application revolves around creating, updating, and deleting users, with the following key functionality:

### 1. **Create User:**
   Users with appropriate permissions can create other users. The user creation form includes fields for:
   - Username
   - Email
   - Address, Phone, City
   - Permissions (Create, Edit, Delete)
   - Consent for data storage

### 2. **Edit User:**
   Admins or users with "edit" permissions can modify the details of existing users via the edit page.

### 3. **Delete User:**
   Users with "delete" permissions can remove users from the system. 

### 4. **View Users:**
   The user list page shows all users created by the current logged-in user, based on their ID. Admin users can see more details.

## How to Run

### Prerequisites:
- **Node.js** and **npm** installed.
- **.NET SDK** installed.
- **MongoDB** running locally or accessible remotely.

### Steps:

0. Make sure you have a Mongo db instance running on mongodb://localhost:27017

1. Clone the repository:
   ```bash
   git clone https://github.com/hara94/SecurePrivacyTasks.git
   cd SecurePrivacyTasks

1.1 Simply run from Visual studio - SecurePrivacyTask1.sln or:

2. Install dependencies for the Angular frontend:
   ```bash
   cd .\SecurePrivacyTask1\ClientApp\
   npm install
  
3. Run the backend (.NET):
   ```bash
   cd ..\SecurePrivacyTask1\
   dotnet run
   
4. Run the Angular frontend:
   cd ClientApp
   npm start

5. Access the application at http://localhost:4200.

## Technologies Used

- **Angular**: Frontend framework for building the UI.
- **NgRx**: For state management.
- **.NET Core**: Backend API for handling user data and authentication.
- **MongoDB**: NoSQL database for storing user data.
- **BCrypt**: Password hashing to ensure security.
- **Cookie-based Authentication**: For managing sessions securely.
- **NGX-Toastr**: For displaying notifications.


# SecurePrivacyTasks - Task 2: Binary String Analysis

## Table of Contents
- [Objective](#objective)
- [Functionality](#functionality)
- [Steps to Run](#steps-to-run)
- [Example](#example)
- [Testing](#testing)
- [Input Validation](#input-validation)
- [License](#license)

This directory contains the solution for Task 2 of the SecurePrivacy coding challenge. 

## Task 2: Binary String Analysis Function

### Objective:
Write a C# function to evaluate binary strings based on specific criteria.

### Functionality:
The function evaluates whether a binary string is "good" based on these conditions:
1. The binary string must have an equal number of 0's and 1's.
2. For every prefix of the binary string, the number of 1's should never be less than the number of 0's.

### Steps to Run:

1. Clone the repository:

```bash
git clone https://github.com/hara94/SecurePrivacyTasks.git
```

2. Navigate to the `SecurePrivacyTask2/BinaryStringParser` directory.

3. Open the solution in Visual Studio or another C# development environment.

4. Build the project using `dotnet build`.

5. Run the program using `dotnet run`. You will be prompted to enter binary strings separated by commas for evaluation.

### Example:

```bash
Enter binary strings separated by commas:
1100, 1010, 11100011

Output:
Binary String: 1100 is Good
Binary String: 1010 is Good
Binary String: 11100011 is Not Good
```

### Testing:

The solution includes NUnit tests to validate the functionality. To run the tests:

1. Navigate to the `BinaryStringParserTests` project.
2. Run the tests using the following command:

```bash
dotnet test
```

### Input Validation:

The program includes input validation to ensure that only valid binary strings (containing only '0's and '1's) are processed.

### License:

This project is licensed under the MIT License.
