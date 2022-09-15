#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_ENVIRONMENT="Development"
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Students.Services/Students.Services.csproj", "Students.Services/"]
RUN dotnet restore "Students.Services/Students.Services.csproj"
COPY . .
WORKDIR "/src/Students.Services"
RUN dotnet build "Students.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Students.Services.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Students.Services.dll"]