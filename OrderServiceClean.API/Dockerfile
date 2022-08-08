#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrderServiceClean.API/OrderServiceClean.API.csproj", "OrderServiceClean.API/"]
COPY ["OrderService.Infrastructure/OrderService.Infrastructure.csproj", "OrderService.Infrastructure/"]
COPY ["OrderService.Application/OrderService.Application.csproj", "OrderService.Application/"]
COPY ["OrderService.Domain/OrderService.Domain.csproj", "OrderService.Domain/"]
RUN dotnet restore "OrderServiceClean.API/OrderServiceClean.API.csproj"
COPY . .
WORKDIR "/src/OrderServiceClean.API"
RUN dotnet build "OrderServiceClean.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderServiceClean.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderServiceClean.API.dll"]