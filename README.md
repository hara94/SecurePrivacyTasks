# SecurePrivacyTasks

**SecurePrivacyTasks** is a user management application demonstrating key CRUD functionalities while incorporating critical GDPR considerations, such as cookie consent, data access rights, and secure authentication.

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

1. Clone the repository:
   ```bash
   git clone https://github.com/hara94/SecurePrivacyTasks.git
   cd SecurePrivacyTasks

2. Install dependencies for the Angular frontend:
   ```bash
   cd .\SecurePrivacyTask1\ClientApp\
   npm install
  
3. Run the backend (.NET):
   ```bash
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