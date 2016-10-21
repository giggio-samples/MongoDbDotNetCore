FROM microsoft/dotnet:1.0.0-preview2-sdk
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
COPY . /app
RUN dotnet restore
CMD dotnet run