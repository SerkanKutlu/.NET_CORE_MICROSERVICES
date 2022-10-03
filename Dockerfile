FROM mcr.microsoft.com/dotnet/core/sdk:latest
WORKDIR /app
COPY . .
COPY ["CustomerService.KafkaConsumer/CustomerService.KafkaConsumer.csproj", "CustomerService.KafkaConsumer/"]
COPY ["CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "CustomerService.Infrastructure/"]
COPY ["CustomerService.Application/CustomerService.Application.csproj", "CustomerService.Application/"]
COPY ["CustomerService.Domain/CustomerService.Domain.csproj", "CustomerService.Domain/"]
COPY ["GenericMongo/GenericMongo.csproj", "GenericMongo/"]
RUN dotnet restore "CustomerService.KafkaConsumer/CustomerService.KafkaConsumer.csproj"
RUN dotnet publish CustomerService.KafkaConsumer.csproj -c Release -o out
WORKDIR out
ENTRYPOINT ["dotnet", "DockerizeWebExample.dll"]