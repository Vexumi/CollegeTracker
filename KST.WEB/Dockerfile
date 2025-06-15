FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY KST.WEB/KST.WEB.csproj KST.WEB/
COPY KST.Business/KST.Business.csproj KST.Business/
COPY KST.DataAccess/KST.DataAccess.csproj KST.DataAccess/

RUN dotnet restore "KST.WEB/KST.WEB.csproj"

COPY . .

WORKDIR /src/KST.WEB
RUN dotnet build "KST.WEB.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "KST.WEB.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "KST.WEB.dll"]
