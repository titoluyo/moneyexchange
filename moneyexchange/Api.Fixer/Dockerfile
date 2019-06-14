FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src

COPY ["Api.Fixer.Entities/Api.Fixer.Entities.csproj", "Api.Fixer.Entities/"]
RUN dotnet restore "Api.Fixer.Entities/Api.Fixer.Entities.csproj"
COPY ["Api.Fixer.Domain/Api.Fixer.Domain.csproj", "Api.Fixer.Domain/"]
RUN dotnet restore "Api.Fixer.Domain/Api.Fixer.Domain.csproj"
COPY ["Api.Fixer.Data/Api.Fixer.Data.csproj", "Api.Fixer.Data/"]
RUN dotnet restore "Api.Fixer.Data/Api.Fixer.Data.csproj"
COPY ["Api.Fixer/Api.Fixer.csproj", "Api.Fixer/"]
RUN dotnet restore "Api.Fixer/Api.Fixer.csproj"
COPY . .
WORKDIR "/src/Api.Fixer"
RUN dotnet build "Api.Fixer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Api.Fixer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Api.Fixer.dll"]