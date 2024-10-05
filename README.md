# AI-Powered Flash Card

An English learning flash card system.

Deployed application:
- Url: https://tdevflashcard.azurewebsites.net
- Username: alice
- Password: Pass123$


## Features
- [x] User authentication: signup, login, and logout. Support login with Google
- [x] Flash card management: create, read, update, and delete
- [x] Study mode: a basic carousel for users to review their flashcards
- [x] AI-Powered enhancement: use AI to generate example sentences for vocabulary words
- [ ] AI image suggestion


## High level architecture

<img src="./images/flash-card-architecture.png" alt="High level architecture">


## Technologies

- Auth
  - Duende (IdentityServer) a famous OpenID Connect and OAuth 2.0 framework. Allows us to manage users ourselve
  - Asp Net Core Identity: provided by Microsoft, to ease the user management, supported by Duende
  - Sqlite: to have a quick development. Must be replaced by MSSQL Server or Postgres when deploying to Staging or higher environments
- Client app
  - React
  - React bootstrap: I want to have friendly UI and easy to develope the app. It turns out this is not a good choice. The Tooltip makes my UI shake. No built-in hooks for the Toast
  - React router dom V6: just want to try the taste of "loader" and "action"
- BFF
  - .Net 8: open source, cross-platform
  - YARP: to forward requests from the SPA to the API without writing code
- API
  - EF Core: to manipulate data
  - Azure OpenAI Chat Completion service: to leverage the power of GPT models to generate example sentences. Another reason I choose Azure is because I have a Visual Studio subscription ($150)
  - .Net 8
  - Sqlite


## Local setup instructions

### Prerequisites

- .Net 8 SDK
- Node 16 or higher
- Visual Studio 2022 (optional)

### Start projects

3 projects to start
- FlashCard.Auth
- FlashCardClient.Server
- FlashCard.Api

#### With Visual Studio

Configure startup projects

<img src="./images/multi-startup-projects.png" alt="Set multiple startup projects" style="max-width: 600px" />

#### Or, with cmd

Open a new cmd
```
cd src\FlashCard.Auth
dotnet run
```

Open another cmd
```
cd src\FlashCardClient\FlashCardClient.Server
dotnet run
```

Open another cmd
```
cd src\FlashCard.Api
dotnet run
```

### Open the app in browser

https://localhost:5173/


## Known issues and limitations
- UI responsiveness. Currently, it is not mobile friendly
- No paging
- Handle session timeout more properly. Currently, it auto redirects user to the login page
- The current prompt is not optimized. If the user has a typo, it may fail to generate examples

## Ideas for future improvements
- A better UI design
- Search decks
- Paging + sorting
- Allow user to revert word and meaning for all cards in a deck
- Mark learned cards, remove from learning