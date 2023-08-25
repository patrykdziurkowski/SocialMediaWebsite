sleep 15s
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P P@ssword1! -i TableSchema.sql
sleep 5s
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P P@ssword1! -i SampleData.sql
sleep infinity