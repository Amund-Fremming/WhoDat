# TODO`S

- WARNING CreatePlayerTwoBoard may need to store each boardcard in the db with a repo method tracking the boardcards.
- BUG UploadImageToCloudflare does not return the correct url
- BUG CreateBoardCards has bug, if only host choosing, return a unauthorized or so, p2 cannot chose cards if this is the state

- TODO BoardCardServiceTest: Missing test classes
- TODO BoardCardServiceTest: Missing complete CreateBoardCards method, make it shorter

- REFACTOR backend repos, updating functions can be changed to just one

<hr />

## Backend

- [x] Implement interfaces
- [x] Configure relations in AppContext
- [x] Create basic Repo methods
- [x] Create basic Service methods
- [x] Generate database
- [x] Add JWT for security
- [x] Add Transactions
- [x] Create Controller methods
- [x] Admin Controller
- [x] Add change passord api
- [x] Add signalR endpoints for game logic
- [x] Image upload func to CloudFlare
- [x] Connect backend with CloudFlare
- [x] Add input validation
- [x] Go over design, try to find more api´s to implement
- [x] Implement JWT
- [x] Test JWT implementation
- [x] Create Docs for services and general (especially GetById funcs and PlayerHasPermission, they throw)
- [x] Create Docs for Service Interfaces
- [x] Create Docs for Repository Interfaces
- [ ] Create tests for services
- [?] Create tests for Repositories
- [?] Handling for reconnect when disconnected (Maybe just broadcasting a message?)
- [ ] Create premade cards and their endpoints
- [x] Add error handling in Services
- [ ] ?? Remove logging when finished, remove try/catch from non transactions in service

- [ ] Change authentication to use OAuth
- [ ] Setup Azure env
- [ ] Setup github actions with test pipeline to dev / prod branch to azure

<hr />

## Frontend

- [x] User test with Figma prototype
- [x] Tweak Design
- [ ] Add wrapper / provider for keyboard up (might also add scrollview)
- [x] Add util classes for styles (colors) and styling functions for converting pixel values
- [x] Implement generic components
- [x] Implement Navigation bar
- [x] Implement Routers / View-render for different screens
- [ ] Implement the screens with their components
- [ ] Implement the LoginWall
- [ ] Add Token handling for security
- [ ] Add input validation
- [ ] Add user feedback when api fails
- [ ] Create docs

<hr />

## Deploy

- [ ] Deploy to Eas and App Store Connect
- [ ] Deploy to Azure
- [ ] User Test with Test Flight
- [ ] Fix user feedback
- [ ] Repeat deploy again until finished
- [ ] Deploy to the AppStore
- [ ] Update the docs, and add pipeline docs

<hr />

## Monetization

- Remove update card functinallity, players get x number of cards, can buy more
- Bundle packs (Premade galleries)

<hr />
