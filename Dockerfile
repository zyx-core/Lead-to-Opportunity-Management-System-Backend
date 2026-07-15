# Stage 1: Build the .NET application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["LeadToOpportunityManagement.slnx", "./"]
COPY ["LeadToOpportunity.API/LeadToOpportunity.API.csproj", "LeadToOpportunity.API/"]
COPY ["LeadToOpportunity.BLL/LeadToOpportunity.BLL.csproj", "LeadToOpportunity.BLL/"]
COPY ["LeadToOpportunity.DAL/LeadToOpportunity.DAL.csproj", "LeadToOpportunity.DAL/"]
COPY ["LeadToOpportunity.Models/LeadToOpportunity.Models.csproj", "LeadToOpportunity.Models/"]
COPY ["LeadToOpportunity.Shared/LeadToOpportunity.Shared.csproj", "LeadToOpportunity.Shared/"]

# Restore dependencies
RUN dotnet restore "LeadToOpportunity.API/LeadToOpportunity.API.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the API
WORKDIR "/src/LeadToOpportunity.API"
RUN dotnet publish "LeadToOpportunity.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Serve with ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 5295
EXPOSE 80

# Configure Kestrel to listen on 5295
ENV ASPNETCORE_URLS=http://+:5295

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LeadToOpportunity.API.dll"]
