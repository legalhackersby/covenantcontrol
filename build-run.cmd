:: build and runs whole site
dotnet restore
npm install src/ClientApp/
dotnet run --project src --launch-profile=src