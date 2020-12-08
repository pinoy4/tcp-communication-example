# TCP COMMUNICATION EXAMPLE

### Server app (listener)

This will start the listener on `localhost:12345`.
To change the host and port you can edit the [ServerApp/tcpOptions.json](ServerApp/tcpOptions.json) file.

```
dotnet restore ServerApp
dotnet build -c Release ServerApp
dotnet ServerApp/build/Release/netcoreapp3.1/ServerApp.dll
```

### Client app (sender)

This will send a message to the listener on `localhost:12345`.
To change the host and port you can edit the [ClientApp/tcpOptions.json](ClientApp/tcpOptions.json) file.

```
dotnet restore ClientApp
dotnet build -c Release ClientApp
dotnet ClientApp/build/Release/netcoreapp3.1/ClientApp.dll start-scan -n test01
```
