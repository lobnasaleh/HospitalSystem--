#sdk
FROM mcr.microsoft.com/dotnet/sdk:8.0  as build
WORKDIR /source

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c release -o /app

#runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app .
# define the command that will be run when a Docker container start => starting .NET application within the containe
ENTRYPOINT [ "dotnet","HospitalManagementSystem2.dll" ]
