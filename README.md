# SqlGen

Generate the code from SQL Server tables. Api, Data Managers, entities etc.




### Server menu

This lists the SQL Server connections configured for the application.

You can add your own connection strings to the application by editing the `SqlGenUi.exe.config` file

### Database menu

Once connected to a server you can change the database via the `Database` menu

### Schema menu

You can filter the table list to only show tables in a specific schema via the `Schema` menu

### Options menu

These options control code generation:
* `Add Grant` to generate `GRANT` statements to a role
* `ALTER stored procs` to generate `ALTER PROC` statements instead of `CREATE PROC`
* `Include Audit columns` to include or exclude any columns starting with `AUDIT_` from the code generators

### Generated code

This window shows the generated code.  You can drag & drop the code to a text editor, e.g. Visual Studio or Sql Server Management Studio.
