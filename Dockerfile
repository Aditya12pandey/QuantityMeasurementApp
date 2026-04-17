# Use the official .NET 8 SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["QuantityMeasurementApp.API/QuantityMeasurementApp.API.csproj", "QuantityMeasurementApp.API/"]
COPY ["QuantityMeasurementAppBusiness/QuantityMeasurementAppBusiness.csproj", "QuantityMeasurementAppBusiness/"]
COPY ["QuantityMeasurementAppEntity/QuantityMeasurementAppEntity.csproj", "QuantityMeasurementAppEntity/"]
COPY ["QuantityMeasurementAppRepository/QuantityMeasurementAppRepository.csproj", "QuantityMeasurementAppRepository/"]
RUN dotnet restore "QuantityMeasurementApp.API/QuantityMeasurementApp.API.csproj"

# Copy the rest of the code and build the app
COPY . .
WORKDIR "/src/QuantityMeasurementApp.API"
RUN dotnet build "QuantityMeasurementApp.API.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "QuantityMeasurementApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8 runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuantityMeasurementApp.API.dll"]
