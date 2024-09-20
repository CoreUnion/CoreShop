#!/bin/bash

# docker 一键启动脚本
# 注意事项：
#  1. 在docker-compose 中为mysql镜像设置 container_name, 然后修改admin和api中 appsettings.json 中的数据库连接
#  2. 在docker-compose 中为redis镜像设置 container_name, 然后修改admin和api中 appsettings.json 中的redis连接
#  3. admin 默认端口为Properties中launchSettings.json 中的端口1987,api的默认端口为Properties中launchSettings.json 中的端口2015。 容器内admin和api的端口为8080
#     docker-compose中admin端口映射设置为:8088:8080 (8088 为访问端口， 8080 默认)
#     docker-compose中api端口映射设置为:8089:8080 (8089 为访问端口， 8080 默认)
#      部署成功后 访问地址为：http://localhost:8088 (admin)    http://localhost:8089  (api)


# 获取当前脚本的目录
SCRIPT_DIR=$(dirname "$0")

# 0x01 先清理Debug 和 Release目录
clean_build_directories() {
    local dir=$1
    if [ -d "$dir" ]; then
        echo "Cleaning $dir"
        rm -rf "$dir"
    fi
}

find "$SCRIPT_DIR" -type d \( -name "Debug" -o -name "Release" \) | while read -r dir; do
    clean_build_directories "$dir"
done

clear
# 0x02 生成项目
dotnet build

# relase
dotnet publish "./CoreCms.Net.Web.Admin/CoreCms.Net.Web.Admin.csproj" -c release -o ./docker/admin --no-restore
clear

dotnet publish "./CoreCms.Net.Web.WebApi/CoreCms.Net.Web.WebApi.csproj" -c release -o ./docker/api --no-restore
clear

cp docker-compose.yaml ./docker
cd docker

# 启动docker-compose ; 
docker-compose up -d

echo "build completed."
