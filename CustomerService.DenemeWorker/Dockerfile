#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CustomerService.DenemeWorker/CustomerService.DenemeWorker.csproj", "CustomerService.DenemeWorker/"]
COPY ["GenericMongo/GenericMongo.csproj", "GenericMongo/"]
RUN dotnet restore "CustomerService.DenemeWorker/CustomerService.DenemeWorker.csproj"
COPY . .
WORKDIR "/src/CustomerService.DenemeWorker"
RUN dotnet build "CustomerService.DenemeWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerService.DenemeWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.DenemeWorker.dll"]