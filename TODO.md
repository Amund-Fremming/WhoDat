# TODO`S

## Fix first!

- [ ] WARNING May need to add Includes for PlayerHasPermission to work!

- [ ] GetBoardWithBoardCards
  - TODO: if playerTwoBoard is not created, create a duplicate from player one
  - INFO: her the other person needs to get their board with gameid
- [ ] CreateBoardCards:

  - responsibility for sending gamestate P1/P2 starting
  - have to check game for gametype and see the state
  - if both choosing, let player create 20 incomming
  - when a create comes in, update the state and broadcast
  - if the other player has created, broadcast game started
  - Method in service needs to return state!

  game flow

  - oppretter kort, da vi er begge klare og far game started
  - hent board fra frontend!

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
- [ ] Handling for reconnect when disconnected
- [ ] Image upload func to azure
- [x] Add input validation
- [x] Go over design, try to find more api´s to implement
- [ ] Generate Mock data
- [ ] Create premade cards
- [ ] Create endpoints for getting premade boards or boardcards.
- [ ] Test Backend
- [x] Implement JWT
- [x] Test JWT implementation
- [ ] Add error handling in Services
- [ ] Add error handling in Repositories
- [ ] Create Docs for services and general (especially GetById funcs and PlayerHasPermission, they throw)
- [ ] Create Docs for Interfaces

- [ ] FEATURE: Add logic for adding payment solution for buying more gallery cards??
- [ ] FEATURE: Friends functonality
- [ ] FEATURE: Persistent logs with a controller to get them

<hr />

## Frontend

- [x] User test with Figma prototype
- [x] Tweak Design
- [ ] Implement generic components
- [ ] Implement Navigation bar
- [ ] Implement Routers / View-render for different screens
- [ ] Implement the screens with their components
- [ ] Implement the LoginWall
- [ ] Add Token handling for security
- [ ] Add input validation
- [ ] Test the app

<hr />

## Deploy

- [ ] Deploy to Eas and App Store Connect
- [ ] Deploy to Azure
- [ ] User Test with Test Flight
- [ ] Fix user feedback
- [ ] Repeat deploy again until finished
- [ ] Deploy to the AppStore

<hr />

## Monetization

- Remove update card functinallity, players get x number of cards, can buy more
- Bundle packs (Premade galleries)

<hr />

## To docs

- CreateGame creates one board for making the boards the same we use this one
- StartGame creates a duplicate board of the board that was created then the game was made
- UpdateBoardCards returns a number indicating the number of cards left on board
- GetBoardWithBoardCards creates a duplicate board of playerones board if player two board does not excist
