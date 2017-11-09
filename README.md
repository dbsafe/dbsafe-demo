dbsafe-demo
===========
Demonstrates how to use [NuGet package SqlDbSafe]( https://www.nuget.org/packages/SqlDbSafe/) to test a DAL component that connects to a SQL Server database.

Projects
--------

**ProductBL:**<br>
Project where the domain objects are defined.

**ProductDatabase:**<br>
SQL Server Database project with a demo database.

**ProductDatabase.Tests:**<br>
SQL Server Database Test project with a dummy test. This project deploys the database when the test executes.

**ProductDAL:**<br>
A Data Access Layer that uses Entity Framework to communicate with the Product database.

**ProductDAL.Tests:**<br>
The unit test project with the integration test for testing the DAL.
