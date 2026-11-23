# Lab 10 – Jewellery Management API

ASP.NET Core Web API (.NET 10) with Entity Framework Core and SQL Server LocalDB.
Full CRUD for jewellery items, including image upload.

## Endpoints

| Method | URL                   | What it does                              |
|--------|-----------------------|-------------------------------------------|
| GET    | /api/jewellery        | List all items (optional `?search=`)      |
| GET    | /api/jewellery/{id}   | Get one item                              |
| POST   | /api/jewellery        | Add an item (form-data, image required)  |
| PUT    | /api/jewellery/{id}   | Update an item (form-data, image optional)|
| DELETE | /api/jewellery/{id}   | Delete an item and its image              |

## Run it

1. Open `lab10_Jewellery_API.slnx` in Visual Studio.
2. Tools → NuGet Package Manager → Package Manager Console:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
3. Press F5 and test with Postman.
