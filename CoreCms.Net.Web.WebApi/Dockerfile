FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app

EXPOSE 80

COPY ./ /app

ENV TZ=Asia/Shanghai

RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "CoreCms.Net.Web.WebApi.dll"]