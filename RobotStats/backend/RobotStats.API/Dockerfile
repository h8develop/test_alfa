FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RobotStats.API.csproj", "."]
RUN dotnet restore "RobotStats.API.csproj"
COPY . .
RUN dotnet publish "RobotStats.API.csproj" -c Release -o /app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
RUN mkdir -p /app/Data

EXPOSE 80
ENTRYPOINT ["dotnet", "RobotStats.API.dll"]