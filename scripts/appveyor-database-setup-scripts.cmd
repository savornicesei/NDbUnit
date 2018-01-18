@ECHO ON

REM SQL Server setup *****
sqlcmd -S ".\SQL2014" -U sa -P Password12! -i "%APPVEYOR_BUILD_FOLDER%\test\NDbUnit.Test\Scripts\sqlserver-testdb-create.sql"

REM MySQL setup *****
mysql --user=root --password=Password12! < "%APPVEYOR_BUILD_FOLDER%\test\NDbUnit.Test\Scripts\mysql-testdb-create.sql"

REM PostgreSQL setup *****
REM NOTE: postgres doesn't support password as args to psql so ensure that PGPASSWORD env var is set...
SET PGPASSWORD=Password12!

REM NOTE: postgres won't support a CREATE DATABASE call inside a larger script, so CREATE has to be its own invocation...
psql --username=postgres --no-password --command="CREATE DATABASE testdb;"

psql --username=postgres --no-password --dbname=testdb --file="%APPVEYOR_BUILD_FOLDER%\test\NDbUnit.Test\Scripts\postgres-testdb-create.sql"

REM ORACLE XE setup *****
REM *** DISABLED UNTIL APPVEYOR SUPPORTS ORA XE ON CI SERVERS ***
REM %ORACLE_HOME%\bin\sqlplus xdba/xdba @"%APPVEYOR_BUILD_FOLDER%\test\NDbUnit.Test\Scripts\oracle-testdb-create.sql"

