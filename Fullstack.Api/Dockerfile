#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Fullstack.Api/Fullstack.Api.csproj", "Fullstack.Api/"]
RUN dotnet restore "Fullstack.Api/Fullstack.Api.csproj"
COPY . .
WORKDIR "/src/Fullstack.Api"
RUN dotnet build "Fullstack.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fullstack.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fullstack.Api.dll"]