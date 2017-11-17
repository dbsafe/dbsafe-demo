dbsafe-demo
===========
Demonstrates how to use [NuGet package SqlDbSafe]( https://www.nuget.org/packages/SqlDbSafe/) to test a DAL component that connects to a SQL Server database.

Projects
--------

**ProductBL:**<br>
Project where the domain objects are defined.

**For SQL Server**

**ProductDatabase:**<br>
SQL Server Database project with a demo database.

**ProductDatabase.Tests:**<br>
SQL Server Database Test project with a dummy test. This project deploys the Product_Build database to SQL Server when the test executes.

**ProductDAL:**<br>
A Data Access Layer that uses Entity Framework to communicate with the Product database in SQL Server.

**ProductDAL.Tests:**<br>
The unit test project with the integration test for testing the DAL.

**For PostgreSQL**

**ProductDAL.PG:**<br>
A Data Access Layer that uses Entity Framework to communicate with the Product database in PostgreSQL. Follow the instructions in restoring-product-database.txt to create the product_build database in PostgreSQL.

**ProductDAL.PG.Tests:**<br>
The unit test project with the integration test for testing the DAL.
