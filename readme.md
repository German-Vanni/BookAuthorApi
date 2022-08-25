# Book/Author Rest Api

#### Implementation:

- .Net 6.

- Authentication through JWT tokens.

- Applied Repository-Service pattern with generic repository and dependency injection.

- Repositories tied together with the UnitOfWork pattern

- Used Entity Framework Core as the ORM

- Logging done with [NLog](https://nlog-project.org/)

- Created a [custom middleware](https://github.com/German-Vanni/BookAuthorApi/blob/main/BookAuthor.Api/Middleware/ErrorHandlingMiddleware.cs) to handle exceptions (and to make the Controllers slimmer)

- Pagination done with the help of [X.PagedList](https://github.com/dncuug/X.PagedList)

- Implemented a custom [ActionFilter](https://github.com/German-Vanni/BookAuthorApi/blob/main/BookAuthor.Api/ActionFilters/ClaimedUserValidationFilter.cs) for authenticated routes

- Created some [custom attributes](https://github.com/German-Vanni/BookAuthorApi/tree/main/BookAuthor.Api/Util/Attributes) to make validation easier

- Access to Swagger on development mode

- Applied SoftDelete functionality through Global QueryFilters
  
  

To run the project you need access to a SQL Server Database



Then add the connection string to appSettings.json:

    "ConnectionStrings": {
        "SqlServerCS": "Your_Connection_String"
      },

Also inside that same json you can modify some other configurations as well: 

    "Jwt": {
        "Issuer": "BookAuthorApi",
        "Lifetime": "1440",
        "Key": "SomeKeyToUseIfItsUndefinedInEnvironmentVariables"
      },
      "Roles": {
        "User": {
          "Id": "",
          "Name": "User"
        },
        "Admin": {
          "Id": "",
          "Name": "Admin"
        }
      },
      "AdminAccount": {
        "Id": "",
        "UserName": "",
        "NormalizedUserName": "",
        "NormalizedEmail": ",
        "Email": "",
        "Password": ""
      }

But you can use the defaults from appSettings.Development.json
For the Jwt singing key, you can define it as an environment variable with the name "ASPNET_API_SECRET".



You can use the inserts.sql file in the root of the repository to add some rows to your database. 

#### Implementation:

```
GET
/api/book/
/api/book/{id}
/api/book/approve
/api/author
/api/author/{id}
/api/author/approve

POST
/api/book
/api/book/rate/{id}
/api/book/approve/{id}
/api/author
/api/author/approve/{id}
/api/account/register
/api/account/login

PUT
/api/book/{id}
/api/author/{id}

DELETE
/api/book/{id}
/api/author/{id}

```
