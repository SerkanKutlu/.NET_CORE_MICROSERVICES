#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CustomerService.KafkaConsumer/CustomerService.KafkaConsumer.csproj", "CustomerService.KafkaConsumer/"]
COPY ["CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "CustomerService.Infrastructure/"]
COPY ["CustomerService.Application/CustomerService.Application.csproj", "CustomerService.Application/"]
COPY ["CustomerService.Domain/CustomerService.Domain.csproj", "CustomerService.Domain/"]
COPY ["GenericMongo/GenericMongo.csproj", "GenericMongo/"]
RUN dotnet restore "CustomerService.KafkaConsumer/CustomerService.KafkaConsumer.csproj"
COPY . .
WORKDIR "/src/CustomerService.KafkaConsumer"
RUN dotnet build "CustomerService.KafkaConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerService.KafkaConsumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.KafkaConsumer.dll"]