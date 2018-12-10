


# DISCLAIMER

Alpha version. Code was typed fast, streams and async-awaits not used, layers not layered and no optimizations done, long running stuff not in background. Does reflect our ability for work fast, not our ability to code well.

See [presentation in Russian here](https://youtu.be/I50DY7bAkvE?t=193)

# Run

`dotnet run --launch-profile=src`

# Build

## Works in `Visual Studio Code 1.29.1` with plugins:
- `Debugger for Firefox` or `Debugger for Chrome` 
- `Omnisharp`
- `EditorConfig for VS Code`
- `.NET Core Test Explorer`
- `npm`
- `GitLens — Git supercharged`
- 

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

See real covenants in `data/` folder.

# Main scenario

- User uploads document
- Document is parsed
- Positions of covenants are found
- Document with highlighted covenants are  shown
- User clicks on covenant
- Covenant is added to task board
- Covenant may be acted upon (e.g. set notification)

## Similar solutions

- https://ebrevia.com/
- https://kirasystems.com/

# Features 
- Suppored uploads: txt, rtf, doc, docx, odt
- Text only view

# Proof of Concept
- upload pdf
- by nlp ml;
- allow manual highlight for data collection and tuning
- show in dashboard; next:allow attach action to dashboard item
- near native view;


# Solution
- Will store native document to allow download and reparse original. So not client side (fat client) parsing
- Store document file system to allow command line tools to run upon


# Tech

- Решение построенно на технологиях c полностью открытым исходным кодом(React, .NET Core, LibreOffice, MongoDb)
- Эвристическая модель распознания основана на:
  1. Поиске ключевых слов и их вариаций
  2. Взаимном расположении ключевых слов в структуре документа
  3. Задание веса ключевого слова для типа ковенанты
- Модель готова к более сложным вероятностым иерархическим эвристикам и к интеграции машинного обучения

