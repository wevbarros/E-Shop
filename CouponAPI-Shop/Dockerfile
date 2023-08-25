# Imagem base
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Diretório de trabalho
WORKDIR /app

# Copiar arquivos e restaurar dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar tudo e compilar
COPY . ./
RUN dotnet publish -c Release -o out


# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CouponAPI-Shop.dll"]
