# shortestpath-webapi

Steps to execute

1. Install dotnet sdk (https://dotnet.microsoft.com/download)
2. Clone repo
    ``` bash
      git clone git@github.com:satheeshds/shortestpath-webapi.git
  
      cd shortestpath-webapi/src/
    ```
3. Testing
    ``` bash
    
    dotnet test shortestpath-webapi-test/
    ```
4. Run
    ``` bash
    
    dotnet run -p shortestpath-webapi --launch-profile shortestpath_webapi
    
    info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
    info: Microsoft.Hosting.Lifetime[0]
      Now listening on: https://localhost:5001
    info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
    info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
    ```
    
5. API Testing
    ``` 
    Open the browser with http://localhost:5000/swagger/index.html
    ```
    
    
Using docker

``` bash

cd shortestpath-webapi/src/

docker build -t shortestpath-webapi -f shortestpath-webapi/Dockerfile .

docker run -p 8085:80 shortestpath-webapi
```
```
Open browser on http://localhost:8085/swagger/index.html

```

> By using docker, we may not test data persistence, since the docker containers are transient every time new container spins up there will be new database file. 
