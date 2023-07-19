#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LearnSmartCoding.CosmosDb.Linq.API.csproj", "."]
RUN dotnet restore "./LearnSmartCoding.CosmosDb.Linq.API.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "LearnSmartCoding.CosmosDb.Linq.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LearnSmartCoding.CosmosDb.Linq.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create the uploads directory within the container
# RUN mkdir /app/uploads

# This volume is virtual mount for application
VOLUME ["/app/uploads"]

ENTRYPOINT ["dotnet", "LearnSmartCoding.CosmosDb.Linq.API.dll"]