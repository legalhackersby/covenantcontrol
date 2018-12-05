


# Build

- Works in `Visual Studio Code 1.29.1` with Fifefox, Omnisharp plugins.
- `Visual Studio 2017.8.9`?
- WebStorm?

# SDK and runtime



# Front
- [React Dev tools](https://fb.me/react-devtools)
  
# All On Windows 10
- [node-v10.14.1](https://nodejs.org/dist/v10.14.1/node-v10.14.1-x64.msi)
- [.NET Core SDK 2.1.500 ](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.500-windows-x64-installer)



# Team

https://web.telegram.org/#/im?p=g283474501

# Domain

[Conventant](https://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D0%B2%D0%B5%D0%BD%D0%B0%D0%BD%D1%82_(%D1%8E%D1%80%D0%B8%D1%81%D0%BF%D1%80%D1%83%D0%B4%D0%B5%D0%BD%D1%86%D0%B8%D1%8F))

```
Договор между Васей Пупкиним и ИООО Рога и Копыта.

Вася Пупкин, далее именуемы Рогоносец.
ИООО Рога и Копыта, далее именуемая как Организация.

Рогоносец обязуется внести плату в размере `100 р. (сто рублей)` за рога не познее `31 декабря 2021 года` на `счёт № 100300500`
Рогоносец обязуется носить рога не снимая.

```

TODO: we will be given with real covenantas

# Main scenario

- User uploads document
- Document is parsed
- Positions of found
- Document with highlightet coventants is shown
- User clicks on covenat
- Covenant is added to board
- Coventatnt may be acted upon

## Example

```yaml
PaymantCovenant:
  DueDate: 31 декабря 2021 года
  Action: Внести плату
  Amount: 100 р. (сто рублей)
  TargetAccount: № 100300500
```


# Proof of Concept

- upload text only; next upload text only docx or rtf, next pdf
- find covenants by regex; next by nlp ml
- position is `{start char index, end char index, type of data index}`
- only found can be added; next allow manual highlight
- show in dashboard; next:allow attach action to dashboard item



# Tech

- Front is React
- Front back ASP.NET Core
- Database?
- docx, doc, pdf parsing libs?

# Solutions
- Will store native document to allow download and reparce original. So not client side (fat client) parsing
- Store document text in database or as file?

# Todo
- sketch dummy data and API and interfaces on MVC side to allow parallel start
- sketch dummy react view (read data stored in json file) to show document random highligh and to press on yellow to get dashboard item (all in raw text-json blobs, no real sructure)
- Update React to latest?