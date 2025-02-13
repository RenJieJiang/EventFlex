# 第一阶段：基础镜像用于运行时环境
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 第二阶段：构建环境，用于还原 NuGet 包、编译代码等
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./UserManagement.API/UserManagement.API/UserManagement.API.csproj", "UserManagement.API/"]
RUN dotnet restore "./UserManagement.API/UserManagement.API.csproj"
COPY "./UserManagement.API/UserManagement.API" "UserManagement.API/"

# 安装 EF Core CLI 工具
RUN dotnet tool install --global dotnet-ef --version "9.0.*"

WORKDIR "/src/UserManagement.API"
RUN dotnet build "./UserManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 第三阶段：发布阶段，用于准备发布的文件
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 第四阶段：最终生产环境或非调试模式下使用的阶段
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# 设置 PATH 环境变量，使得全局工具可被找到
ENV PATH="${PATH}:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "UserManagement.API.dll"]