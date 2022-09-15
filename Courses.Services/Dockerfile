#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_ENVIRONMENT="Development"
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Courses.Services/Courses.Services.csproj", "Courses.Services/"]
RUN dotnet restore "Courses.Services/Courses.Services.csproj"
COPY . .
WORKDIR "/src/Courses.Services"
RUN dotnet build "Courses.Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Courses.Services.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Courses.Services.dll"]