#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DocumentService.API/DocumentService.API.csproj", "DocumentService.API/"]
COPY ["DocumentService.Infrastructure/DocumentService.Infrastructure.csproj", "DocumentService.Infrastructure/"]
COPY ["DocumentService.Core/DocumentService.Core.csproj", "DocumentService.Core/"]
COPY ["GenericMongo/GenericMongo.csproj", "GenericMongo/"]
RUN dotnet restore "DocumentService.API/DocumentService.API.csproj"
COPY . .
WORKDIR "/src/DocumentService.API"
RUN dotnet build "DocumentService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DocumentService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DocumentService.API.dll"]