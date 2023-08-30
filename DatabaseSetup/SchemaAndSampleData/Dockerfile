FROM mcr.microsoft.com/mssql/server

USER root
RUN mkdir -p /usr/src/databaseSetup/
WORKDIR /usr/src/databaseSetup

EXPOSE 1433

COPY Shared/TableSchema.sql /usr/src/databaseSetup
COPY SchemaAndSampleData/SampleData.sql /usr/src/databaseSetup
COPY SchemaAndSampleData/setupServer.sh /usr/src/databaseSetup
COPY Shared/setupProcesses.sh /usr/src/databaseSetup
USER mssql

ENV ACCEPT_EULA Y
ARG password
RUN test -n "${password}" || (echo "password not set" && false)
ENV MSSQL_SA_PASSWORD ${password}

RUN /bin/bash /usr/src/databaseSetup/setupProcesses.sh "${password}"


HEALTHCHECK --interval=5s --timeout=1s --start-period=10s --retries=10 \
  CMD /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ${MSSQL_SA_PASSWORD} -Q "SELECT 1" -b