#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/SourceBaseBE.MainService/SourceBaseBE.MainService.csproj", "src/SourceBaseBE.MainService/"]
COPY ["subs/iSoft.Core/iSoft.Common/iSoft.Common.csproj", "subs/iSoft.Core/iSoft.Common/"]
COPY ["subs/iSoft.Core/iSoft.ExportLibrary/iSoft.ExportLibrary.csproj", "subs/iSoft.Core/iSoft.ExportLibrary/"]
COPY ["subs/iSoft.Core/iSoft.InfluxDB/iSoft.InfluxDB.csproj", "subs/iSoft.Core/iSoft.InfluxDB/"]
COPY ["subs/iSoft.Core/iSoft.SocketIOClient/iSoft.SocketIOClient.csproj", "subs/iSoft.Core/iSoft.SocketIOClient/"]
COPY ["src/SourceBaseBE.CommonFunc/SourceBaseBE.CommonFunc.csproj", "src/SourceBaseBE.CommonFunc/"]
COPY ["subs/iSoft.Core/iSoft.ElasticSearch/iSoft.ElasticSearch.csproj", "subs/iSoft.Core/iSoft.ElasticSearch/"]
COPY ["subs/iSoft.Core/iSoft.ExcelHelper/iSoft.ExcelHepler/iSoft.ExcelHepler.csproj", "subs/iSoft.Core/iSoft.ExcelHelper/iSoft.ExcelHepler/"]
COPY ["subs/iSoft.Core/iSoft.RabbitMQ/iSoft.RabbitMQ.csproj", "subs/iSoft.Core/iSoft.RabbitMQ/"]
COPY ["subs/iSoft.Core/iSoft.Redis/iSoft.Redis.csproj", "subs/iSoft.Core/iSoft.Redis/"]
COPY ["src/SourceBaseBE.Database/SourceBaseBE.Database.csproj", "src/SourceBaseBE.Database/"]
COPY ["subs/iSoft.Core/iSoft.ConnectionCommon/iSoft.ConnectionCommon.csproj", "subs/iSoft.Core/iSoft.ConnectionCommon/"]
COPY ["subs/iSoft.Core/iSoft.DBLibrary/iSoft.DBLibrary.csproj", "subs/iSoft.Core/iSoft.DBLibrary/"]
COPY ["subs/iSoft.Core/iSoft.Database/iSoft.Database.csproj", "subs/iSoft.Core/iSoft.Database/"]
RUN dotnet restore "src/SourceBaseBE.MainService/SourceBaseBE.MainService.csproj"
COPY . .
WORKDIR "/src/src/SourceBaseBE.MainService"
RUN dotnet build "SourceBaseBE.MainService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SourceBaseBE.MainService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SourceBaseBE.MainService.dll"]