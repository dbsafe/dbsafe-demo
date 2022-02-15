## dbsafe-demo

Demonstrates how to use NuGet packages [SqlDbSafe]( https://www.nuget.org/packages/SqlDbSafe/) and [PgDbSafe]( https://www.nuget.org/packages/PgDbSafe/) to test DAL components that connect to SQL Server and PostgreSQL.

### Projects
--------

### ProductBL
Project where the domain objects are defined.

### ProductDAL
A Data Access Layer that uses Entity Framework to communicate with the Product database in SQL Server.

### ProductDAL.Tests
The unit test project with the integration test for testing the DAL.

### ProductDAL.PG
A Data Access Layer that uses Entity Framework to communicate with the Product database in PostgreSQL.

### ProductDAL.PG.Tests
Unit test project with the integration test for testing the DAL connecting to PostgreSQL.

### Databases
------

Demo databases used by the test:

https://github.com/dbsafe/dbsafe-pg-db

https://github.com/dbsafe/dbsafe-sql-db