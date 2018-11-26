FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY WebAPI45/WebAPI45.csproj WebAPI45/
RUN dotnet restore WebAPI45/WebAPI45.csproj
COPY . .
WORKDIR /src/WebAPI45
RUN dotnet build WebAPI45.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish WebAPI45.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebAPI45.dll"]
