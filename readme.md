


# Build

## Development Mode
Swapping to Development environment will display more detailed information about the error that occurred.

Development environment should not be enabled in deployed applications, as it can result in sensitive information from exceptions being displayed to end users. For local debugging, development environment can be enabled by setting the ASPNETCORE_ENVIRONMENT environment variable to Development, and restarting the application.

Visual Studio manages it automatically. If you use another IDE run the following command:

`setx ASPNETCORE_ENVIRONMENT "Development"`

## Works in `Visual Studio Code 1.29.1` with plugins:
- `Debugger for Firefox` or `Debugger for Chrome` 
- `Omnisharp`
- `EditorConfig for VS Code`
- `.NET Core Test Explorer`
- `npm`
- `GitLens — Git supercharged`


 plugins.

## `Visual Studio 2017.8.9`?

## Jetbrains Raider/WebStorm?

# SDK and runtime



## Front
- [React Dev tools](https://fb.me/react-devtools)
  
## All On Windows 10 
- If new to git than [Github for Desktop](https://desktop.github.com/)
- Client build on server [Node 10.14.1](https://nodejs.org/dist/v10.14.1/node-v10.14.1-x64.msi)
- Server runtime [.NET Core SDK 2.1.500 ](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.500-windows-x64-installer) for ASP.NET Core
- Database [MongoDB Community Edition 4.0.4](https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/)
- Converter and extractor [Libre Office 6.1.3](https://www.libreoffice.org/download/download/)

## All on Ubuntu
- You already very cool and can resolve all on your own.

# Team

https://web.telegram.org/#/im?p=g283474501

# Domain

[Covenant](https://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D0%B2%D0%B5%D0%BD%D0%B0%D0%BD%D1%82_(%D1%8E%D1%80%D0%B8%D1%81%D0%BF%D1%80%D1%83%D0%B4%D0%B5%D0%BD%D1%86%D0%B8%D1%8F))

See [example document](src/ClientApp/public/document/55db3a1231a04e39983063027839bf36.txt)

TODO: we will be given with real covenantas

# Main scenario

- User uploads document
- Document is parsed
- Positions of covenants are found
- Document with highlighted covenants are  shown
- User clicks on covenant
- Covenant is added to task board
- Covenant may be acted upon (e.g. set notification)

## Example

```yaml

```


# Proof of Concept

- upload text only; next upload text only docx or rtf, next pdf
- find covenants by regex; next by nlp ml
- position is `{start char index, end char index, type of data index}`
- only found can be added; next allow manual highlight
- show in dashboard; next:allow attach action to dashboard item
- text only view; next: near native view;


# Solutions
- Will store native document to allow download and reparce original. So not client side (fat client) parsing
- Store document text in database or as file?

# Todo
- sketch dummy data and API and interfaces on MVC side to allow parallel start
- sketch dummy react view (read data stored in json file) to show document random highligh and to press on yellow to get dashboard item (all in raw text-json blobs, no real sructure)
- Update React to latest?
