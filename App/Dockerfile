FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App
EXPOSE 8080

COPY . ./
RUN dotnet restore "./Web/Web.csproj"
RUN dotnet publish "./Web/Web.csproj" -c Development -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .

RUN apt-get update && apt-get install -y curl
HEALTHCHECK CMD curl --fail http://localhost:8080/health || exit
ENTRYPOINT ["dotnet", "Web.dll"]