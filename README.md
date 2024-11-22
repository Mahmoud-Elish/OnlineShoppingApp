# OnlineShoppingApp

## Overview
This project implements a simple online shopping application with the following features:

- **Authentication and Authorization:**
  - Basic login and registration system with user roles (Admin and Customer).
  - Only registered users can place orders.

- **Admin Features:**
  - Set and manage currency exchange rates (stored in Redis cache with expiration).
  - Full control over items (CRUD operations).
  - Ability to close orders.

- **Customer Features:**
  - View items and create oredrs.
  - Place orders and apply discount promo codes.

- **Order Management:**
  - Calculate total order value based on currency exchange rates and discounts.
  - Create and retrieve orders with detailed information.

## Technology Stack

- **Framework:** ASP.NET Core 3.1 or above
- **Authentication:** Identity Core
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Caching:** Redis Cache
- **Tech & Design:** C#, OOP, DI, N-tiers, ASP.Net web API, Entity Framework (EF) and LINQ.

## Packages
StackExchange.Redis
Microsoft.AspNetCore.Authentication.JwtBearer
Microsoft.Extensions.Configuration.Binder
Microsoft.EntityFrameworkCore.Proxies
Microsoft.Aspnetcore.Identity.EntityFrameworkCore
Microsoft.Aspnetcore.Identity
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design

# To Install and Run
Check packages installing.
Change ConnectionStrings of your database in appsettings.json.
In package manager console Run two commands.
=> add-migration [name of migration].
=> update-database.
---

## Features and API Endpoints

### Authentication

#### 1. Login API
- **Endpoint:** `POST /api/auth/login`
- **Description:** Accepts email and password, returns a JWT token.
- **Request Body:**
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- **Response:**
  ```json
  {
    "token": "JWT Token"
  }
  ```

---

### Item Management (Admin Only)

#### 2. CRUD Operations for Items
- **Endpoints:**
  - `GET /api/items`: Retrieve all items.
  - `GET /api/items/{id}`: Retrieve item by ID.
  - `POST /api/items`: Create a new item.
  - `PUT /api/items/{id}`: Update an existing item.
  - `DELETE /api/items/{id}`: Delete an item.

- **Item Entity:**
  ```json
  {
    "id": 1,
    "itemName": "A1",
    "description": "Description of the item",
    "uomId": 1,
    "quantity": 100,
    "price": 1500
  }
  ```

---

### Currency Exchange Management

#### 3. Save Currency Exchange Rate
- **Endpoint:** `POST /api/currency`
- **Description:** Saves the currency exchange rate in Redis cache.
- **Request Body:**
  ```json
  {
    "currencyCode": "USD",
    "exchangeRate": 30
  }
  ```

#### 4. Get Currency Exchange Rate
- **Endpoint:** `GET /api/currency/exchange-rate`
- **Description:** Retrieves the exchange rate for a given currency code.
- **Query Parameter:**
  - `currencyCode`: The code of the currency to retrieve.

---

### Order Management

#### 5. Create Order (Customer Only)
- **Endpoint:** `POST /api/orders`
- **Description:** Creates an order with detailed information, applying any discounts if applicable.
- **Request Body:**
  ```json
  {
    "items": [
      { "itemId": 1, "quantity": 2 },
      { "itemId": 3, "quantity": 5 }
    ],
    "discountPromoCode": "PROMO2024"
  }
  ```
- **Response:**
  ```json
  {
    "orderId": 123,
    "totalPrice": 3940.00,
    "currencyCode": "EGP",
    "discountApplied": true
  }
  ```

#### 6. Get Orders (Customer Only)
- **Endpoint:** `GET /api/orders`
- **Description:** Retrieves all orders placed by the logged-in customer.

#### 7. Close Order (Admin Only)
- **Endpoint:** `PUT /api/orders/{id}/close`
- **Description:** Closes an order by its ID.

---

## Configuration

### Application Settings

#### Redis Cache Configuration
- **Key:** `RedisConnection`
- **Example:** `localhost:6379`

#### Currency Configuration
- **Key:** `BaseCurrency`
- **Example:** `"USD"`

#### Discount Configuration
- **Keys:**
  - `DiscountPromoCode`
  - `DiscountValue`
- **Example:**
  ```json
  {
    "DiscountPromoCode": "PROMO2024",
    "DiscountValue": 10.0
  }
  ```

#### Redis Cache Expiration
- **Key:** `RedisExpirationTime`
- **Description:** Time in seconds for Redis cache expiration.
- **Example:** `3600`

---

## Database Design

### Tables

1. **Orders:** Stores order information.
2. **OrderDetails:** Stores details of items in an order.
3. **Customers:** Stores customer details.
4. **Items:** Stores item details.
5. **UnitOfMeasure (UOM):** Stores unit of measure.

### Sample Data

#### Orders Table
| Id  | RequestDate | CloseDate | Status | CustomerId | DiscountPromoCode | DiscountValue | TotalPrice | CurrencyCode | ExchangeRate | ForeignPrice |
|------|-------------|-----------|--------|------------|-------------------|---------------|------------|--------------|--------------|--------------|
| 1    | 2024-01-26  |           | Open   | 1000       | VS02              | 60.00         | 3940.00    | EGP          | 1            | 3940.00      |

#### Items Table
| Id | ItemName | Description | UomId | Quantity | Price |
|----|----------|-------------|-------|----------|-------|
| 1  | A1       | Desc 1      | 1     | 10000    | 1500  |

---

## Development Guidelines

1. Use clean, maintainable, and readable code.
2. Follow SOLID principles.
3. Ensure all endpoints are secured using JWT authentication.
4. Use dependency injection wherever applicable.
5. Implement unit tests for critical features.
6. Use `async` and `await` for asynchronous operations.

---

## Testing

1. Test all endpoints using Postman or a similar tool.
2. Verify JWT authentication works correctly for secured endpoints.
3. Test Redis caching with different expiration settings.
4. Confirm calculations for orders, including discounts and exchange rates.

---

## Deployment

1. Ensure the Redis server is running and accessible.
2. Configure the connection strings and app settings.
3. Run migrations to set up the database:
   ```bash
   dotnet ef database update
   ```
4. Start the application:
   ```bash
   dotnet run
   ```

---

## Contact
For any issues or questions, please contact with me.
 
