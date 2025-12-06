# Battleship Game

A .NET 10 implementation of the Battleship game featuring a REST API and a Blazor WebAssembly frontend.
The solution follows **Clean Architecture** principles.

## 🚀 How to Run

You need the **.NET 10 SDK** installed.
Open two terminal instances in the root folder:

### 1. Start the API (Backend)
```bash
dotnet run --project BattleshipGame.Api
```

The API will start on **http://localhost:5207**. API Documentation and testing available at: **http://localhost:5207/scalar/v1**

### 2. Start the Client (Frontend)

```bash
dotnet run --project BattleshipGame.Web
```

Open the URL shown in the terminal (e.g., **http://localhost:xxxx**) to play.

### ⚙️ Configuration
Game rules (Board size and Ships) can be configured in BattleshipGame.Api/appsettings.json:

```json
"GameSettings": {
  "BoardSize": 10,
  "Ships": [
    { "Name": "Aircraft Carrier", "Size": 5 },
    { "Name": "Battleship", "Size": 4 },
    ...
  ]
}
```