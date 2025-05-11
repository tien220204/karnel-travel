# karnel-travel

<!-- #run migrate command: "Add-Migration MigrateName -OutputDir Data/Migrations -Context ApplicationDbContext" -->


# KarnelTravel

<!-- ![KarnelTravel Icon]("ch co")

[![Build Status](https://ci.appveyor.com/api/projects/status/c9npduu2dp9ljlps?svg=true)](https://ci.appveyor.com/project/YourUsername/KarnelTravel)
[![License](https://img.shields.io/github/license/YourUsername/KarnelTravel.svg?maxAge=2592000)](https://github.com/YourUsername/KarnelTravel/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/KarnelTravel.svg?maxAge=2592000)](https://www.nuget.org/packages/KarnelTravel/)
![Downloads](https://img.shields.io/nuget/dt/KarnelTravel) -->

## Description

**KarnelTravel** is a comprehensive travel management application that helps users book flights, manage flight details, hotels, hotels management and integrate with third-party services for a seamless travel experience. 

The app includes features for managing flights, hotels, users, and roles, as well as integration with Keycloak for identity and access management.

## Features
* [X] User Authentication and Authorization (via Keycloak)
* [X] Flight Management (Create, Read, Update, Delete)(in progress)
* [X] Hotel Management (Create, Read, Update, Delete)
* [X] Role and Permission Management (in progress)
* [X] Integration with external APIs for search and booking (In progress)
* [X] Optimized performance with caching mechanisms and full text search engine
* [X] Secure API with JWT Authentication
* [X] Background tasks for cleaning data and sync data occasionally


<!-- * [X] Admin Dashboard for flight and user management -->

## Technologies Used

* **ASP.NET Core** – Backend framework
* **Keycloak** – Identity and Access Management
* **Serilog** – Logging
* **Hangfire** – Background task processing
* **Sql server** -Database for background job
* **PostgreSQL** – Main database
* **ElasticSearch** –  Full text search capabilities
<!-- * **Redis** – Distributed caching -->
* **FushionCache** –  Memory cache



## Getting Started

Follow the instructions below to set up and run the project locally:

### Prerequisites

- .NET 8 or higher
<!-- - Node.js and npm (for Angular frontend) -->
- PostgreSQL
- Redis (for caching, optional)
- Keycloak instance (for authentication and authorization)(can use existing data in prj)
<!-- - Cloudinary account (for media storage) -->
- Docker and Docker compose

### Install Dependencies

1. **Clone the repository**:

```bash
git https://github.com/tien220204/karnel-travel.git
cd karnel-travel
```

2. **Prepare the services for application**:
```bash
# Background Job (Hangfire SQL Server)

cd docker-compose-files/background-job-storage
docker compose up -d

# ElasticSearch
cd ../elastic-search
docker compose up -d

# Keycloak
cd ../keycloak
docker compose up -d
 ```
3. **Update Db**:

```bash
cd src/KarnelTravel.Infrastructure
dotnet ef database update
```
4. **Run application**:
```bash
cd src/KarnelTravel.Api
dotnet run
```
5. **Run API**:

Access the Swagger UI at:  
[http://localhost:5212/api/index.html?url=/api/specification.json](http://localhost:5212/api/index.html?url=/api/specification.json) for API list.

---

6. **Import initial location data**:

Before using other APIs, you must import the initial location data.

1. Download the Excel file ['karnel-travel/data/location/location.xls'](karnel-travel/data/location/location.xls).
2. Use the **Import Location API** to upload the data:

    - **Endpoint**: `POST /location/import-file`  
    - **Content-Type**: `multipart/form-data`  
    - **Form field name**: `file`

> ⚠️ **Note:** This step is required before using any feature that relies on location data (e.g., creating flights, promotions, etc.).

