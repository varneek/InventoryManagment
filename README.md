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

To search the data by ID use:- /api/Product/GetProductById?id=

To Create new data use:- /api/Product/CreateProduct

To update the data use:- /api/Product/UpdateProduct?id= 

To Delete the data use:- /api/Product/DeleteProduct?id=

To toggle activate and deactivate the data use:- /{id}/toggle


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
