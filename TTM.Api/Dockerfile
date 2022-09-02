#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TTM.Api/TTM.Api.csproj", "TTM.Api/"]
RUN dotnet restore "TTM.Api/TTM.Api.csproj"
COPY . .
WORKDIR "/src/TTM.Api"
RUN dotnet build "TTM.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TTM.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TTM.Api.dll"]