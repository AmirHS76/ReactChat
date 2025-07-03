# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the entire solution into the container
COPY . ./

# Restore using the solution file
RUN dotnet restore ReactChat.sln

# Publish the Presentation project
WORKDIR /src/ReactChat
RUN dotnet publish -c Release -o /app/out ReactChat.Presentation.csproj

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 7240
ENTRYPOINT ["dotnet", "ReactChat.Presentation.dll"]
