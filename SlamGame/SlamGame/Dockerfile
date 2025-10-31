# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy and restore
COPY ["SlamGame.csproj", "."]
RUN dotnet restore "SlamGame.csproj"

# Copy the rest and build
COPY . .
RUN dotnet build "SlamGame.csproj" -c $BUILD_CONFIGURATION -o /app/build



# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SlamGame.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
# Final runtime stage
FROM base AS final
WORKDIR /app
ENV Slam="NotSoSecretKeyThatMightJustBeAWasteOfTimeDain!!!11132HopesGoneYEHE"
ENV ASPNETCORE_URLS=http://+:8080
ENV EXTERNAL_PORT=8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SlamGame.dll"]