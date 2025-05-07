- Will need the EF Core package corresponding to the DB you use. For example for SQLite it is `Microsoft.EntityFrameworkCore.Sqlite`
- For migrations we need the following:
  - `Microsoft.EntityFrameworkCore.Tools` as a global tool for migrations: `dotnet tool install --global dotnet-ef`
  - `Microsoft.EntityFrameworkCore.Design` as a package reference
- **!!! Versions of all of the above need to match with our .NET version (8.0 in our case)**
- In this repo we use a code-first approach, i.e. creating the models for our DB and then creating a DB from them. If there's an exisiting DB, the reverse DB-first approach may be more suitable. See: https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli

- Steps for creating the DB:

  1.  Go to `Entities` for setting up entities (our models)
  1.  Go to `DbContext` to see how to setup a DB context
  1.  Go to `Program.cs` to register our DB context
  1.  `cd` to project folder, then run the CLI:
      ```bash
      dotnet ef migrations add CityInfoDBInitialMigration # This creates 1. migration files and 2. a snapshot of the current context model
      dotnet ef database update # This creates the DB from the migration. Since we didn't have a DB yet, it simply creates new tables from our context models
      ```
  1.  Now we have a DB file, we can inspect by using an SQLite extension, e.g. `SQLite Viewer` in VS Code
      - You can also view the history of migrations in the DB file
  1.  If more migrations are needed later on, we simply run the same migration command with a different name, e.g. `dotnet ef migrations add MoreMigrations`. This will compare with the last snapshot and create the relevant new migrations
  1.  We can then seed the DB with initial data, see `DBContext`

- Steps to link the DB to the controllers:
  1. Defined a repository, see `Services/CityInfoRepository` and its associated interface
  1. Register the repository to the builder in `Program.cs`
  1. Use the repository in the controllers, see `Controllers`
