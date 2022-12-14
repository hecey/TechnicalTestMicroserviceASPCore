#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["./Nuget.Config", "./"]
COPY ["TransactionService.csproj", "./"]
RUN dotnet restore "TransactionService.csproj" --configfile ./Nuget.Config
COPY . .
WORKDIR "/src"
RUN dotnet build "TransactionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TransactionService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 7009

ENTRYPOINT ["dotnet", "TransactionService.dll"]