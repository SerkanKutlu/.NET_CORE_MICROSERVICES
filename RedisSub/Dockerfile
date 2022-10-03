#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RedisSub/RedisSub.csproj", "RedisSub/"]
COPY ["GenericMongo/GenericMongo.csproj", "GenericMongo/"]
COPY ["CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "CustomerService.Infrastructure/"]
COPY ["CustomerService.Application/CustomerService.Application.csproj", "CustomerService.Application/"]
COPY ["CustomerService.Domain/CustomerService.Domain.csproj", "CustomerService.Domain/"]
RUN dotnet restore "RedisSub/RedisSub.csproj"
COPY . .
WORKDIR "/src/RedisSub"
RUN dotnet build "RedisSub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RedisSub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisSub.dll"]