FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CurrencyService/CurrencyServiceAPI.csproj", "CurrencyService/"]
RUN dotnet restore "CurrencyService/CurrencyServiceAPI.csproj"
COPY . .
WORKDIR "/src/CurrencyService"
RUN dotnet build "CurrencyServiceAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CurrencyServiceAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CurrencyServiceAPI.dll"]