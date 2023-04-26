#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FrontedReact/FrontedReact.csproj", "FrontedReact/"]
RUN dotnet restore "FrontedReact/FrontedReact.csproj"
COPY . .
WORKDIR "/src/FrontedReact"
RUN dotnet build "FrontedReact.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FrontedReact.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FrontedReact.dll"]