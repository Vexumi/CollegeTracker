FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CollegeTracker.WEB/CollegeTracker.WEB.csproj", "CollegeTracker.WEB/"]
RUN dotnet restore "CollegeTracker.WEB/CollegeTracker.WEB.csproj"
COPY . .
WORKDIR "/src/CollegeTracker.WEB"
RUN dotnet build "CollegeTracker.WEB.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CollegeTracker.WEB.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CollegeTracker.WEB.dll"]
