#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CustomerServiceClean.API/CustomerServiceClean.API.csproj", "CustomerServiceClean.API/"]
COPY ["CustomerService.Application/CustomerService.Application.csproj", "CustomerService.Application/"]
COPY ["CustomerService.Domain/CustomerService.Domain.csproj", "CustomerService.Domain/"]
COPY ["GenericMongo/GenericMongo.csproj", "GenericMongo/"]
COPY ["CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "CustomerService.Infrastructure/"]
RUN dotnet restore "CustomerServiceClean.API/CustomerServiceClean.API.csproj"
COPY . .
WORKDIR "/src/CustomerServiceClean.API"
RUN dotnet build "CustomerServiceClean.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerServiceClean.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerServiceClean.API.dll"]