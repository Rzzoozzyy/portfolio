# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution file and project files
COPY *.sln .
COPY PortfolioMvc/*.csproj ./PortfolioMvc/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Set working directory to the project folder and publish
WORKDIR /app/PortfolioMvc
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/PortfolioMvc/out .

# Expose port 80 (optional)
EXPOSE 80

# Entry point
ENTRYPOINT ["dotnet", "PortfolioMvc.dll"]
