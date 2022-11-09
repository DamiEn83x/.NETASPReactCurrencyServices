FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["CurrencyService/CurrencyService.csproj", "CurrencyService/"]
RUN dotnet restore "CurrencyService/CurrencyService.csproj"
COPY . .
WORKDIR "/src/CurrencyService"
RUN dotnet build "CurrencyService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CurrencyService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CurrencyService.dll"]