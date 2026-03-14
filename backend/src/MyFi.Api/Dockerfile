FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/MyFi.sln backend/
COPY backend/src/MyFi.Api/MyFi.Api.csproj backend/src/MyFi.Api/

RUN dotnet restore backend/src/MyFi.Api/MyFi.Api.csproj

COPY backend/ backend/

WORKDIR /src/backend/src/MyFi.Api
RUN dotnet publish MyFi.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MyFi.Api.dll"]
