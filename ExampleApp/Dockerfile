FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExampleApp/ExampleApp.csproj", "ExampleApp/"]
RUN dotnet restore "ExampleApp/ExampleApp.csproj"
COPY . .
WORKDIR "/src/ExampleApp"
RUN dotnet build "ExampleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExampleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExampleApp.dll"]
