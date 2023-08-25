FROM mcr.microsoft.com/mssql/server

USER root
RUN mkdir -p /usr/src/databaseSetup/
WORKDIR /usr/src/databaseSetup

EXPOSE 1433

COPY TableSchema.sql /usr/src/databaseSetup
COPY SampleData.sql /usr/src/databaseSetup
COPY createSchemaAndSampleData.sh /usr/src/databaseSetup
COPY initializeServerSample.sh /usr/src/databaseSetup
USER mssql

ENV ACCEPT_EULA Y
ENV MSSQL_SA_PASSWORD P@ssword1!

CMD /bin/bash /usr/src/databaseSetup/initializeServerSample.sh