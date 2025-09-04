# InventoryManagment.web

A simple ASP.NET Core Web API for managing products in an inventory.
Supports CRUD operations, search, filtering, sorting, and status toggling.

## 1. Clone the Repository

   git clone https://github.com/varneek/InventoryManagment.git

## 2. update appsettings.json with your local database connection string.

Example for LocalDB:

  "ConnectionStrings": {
  
    "DefaultConnection": "Server=DESKTOP-717QPUS\\SQLEXPRESS;Database=InventoryDB;Trusted_Connection=True;MultipleActiveResultSets=true"
    
  }

### Run migrations
  use:
  
  Add-Migration InitialCreate
  
  Update-Database

## 3. Run the API

# API Endpoints

To Get all the data use:- /api/Product/GetAllProducts

To search the data by ID use:- /api/Product/GetProductById?id=xyz

To Create new data use:- /api/Product/CreateProduct

To update the data use:- /api/Product/UpdateProduct?id=xyz

To Delete the data use:- /api/Product/DeleteProduct?id=xyz

To toggle activate and deactivate the data use:- /{id}/toggle

To search the data by name use:- /search?name=xyz


## Example Requests

### Add a Product:-

POST /api/Product/CreateProduct

Content-Type: Body/json

{
  "name": "Coffee Machine",
  
  "description": "Automatic coffee maker",
  
  "price": 15000,
  
  "stockQuantity": 10,
  
  "category": "Appliances",
  
  "isActive": true
  
}

### Toggle Product Active Status:-

PATCH /5/toggle

### Search Products by Id:-

GET /api/Product/search?id=5

To Check if the api is working or not you can use Postman application!!

# Postman Collection

A Postman collection is included:

https://varneek-nagar2-3144184.postman.co/workspace/Varneek-Nagar's-Workspace~03ae9b68-cddf-4969-8d54-2d8af8fa81c2/collection/48025099-ba876977-4e74-4908-ba10-8d1668cceb1c?action=share&creator=48025099

