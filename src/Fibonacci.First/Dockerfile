FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Fibonacci.First/Fibonacci.First.csproj", "src/Fibonacci.First/"]
COPY ["src/Fibonacci.Shared/Fibonacci.Shared.csproj", "src/Fibonacci.Shared/"]
RUN dotnet restore "src/Fibonacci.First/Fibonacci.First.csproj"
COPY . .
WORKDIR "/src/src/Fibonacci.First"
RUN dotnet build "Fibonacci.First.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Fibonacci.First.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fibonacci.First.dll"]
