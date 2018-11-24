# Azure Function REST Api Framework

WebApi application with Entity Framework with the following endpoints.
- GET /books - get all books
- GET /book/{id} - get book by id
- DELETE /book/{id} - delete book (should not delete item in DB, only mark it is deleted)
- POST /book/{id} - create book
- PATCH /book/{id} - update book (partial, update only fields which were sent)
- Authentication /auth/login - pass basic authentication credentials username: admin and password: admin

``` csharp
Book object
{
  id: string, // guid
  name: string,
  numberOfPages: number,
  dateOfPublication: number, // utc timestamp
  createDate: number, // utc timestamp, internal only (not returned by api)
  updateDate: number, // utc timestamp, internal only (not returned by api)
  authors: string[]
}
``` 

Also it uses following technology.
* AutoMapper
* EF
* JsonPatch
* Repository Pattern
* Unit of Work

Note: There is a bug in Azure Function local development, it do not generate proper extensions.json file while running, hence local machine testing require running following code inside console app side by side all the time.

``` csharp
internal class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {
            System.IO.File.Copy(@"c:\users\admin\source\repos\Rest.Api\Rest\extensions.json",
                @"C:\Users\admin\source\repos\Rest.Api\Rest\bin\Debug\netcoreapp2.1\bin\extensions.json",
                true);
            Console.WriteLine("Copy done, " + DateTime.Now.ToString());
            Task.Delay(500).Wait();
        }
    }
}
```

How to run
* Create MS SQL database and run script "CreateTableScript.sql".
* Create a console application and place above code in it with proper paths configured. Run it after last step and keep it running.
* Open sln file in VS2017 (Azure Function tools for Visual Studio should be installed).
* Run project "Rest" which will start all functions.

## Security
Application utilizes simple JWT based bearer token authentication.
Call /auth/login function with Basic authentication. Currently there is only one set of credential allowed to login which is username: admin and password: admin
This call will give you bearer token. For all other calls, pass this token as Bearer token in call.