sleep 15s
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $1 -i TableSchema.sql
