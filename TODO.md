# TODO`S

<hr />

## Fix first!

- [ ] WARNING May need to add Includes for PlayerHasPermission to work!
- [ ] WARNING UnnauthorizedException might not be thrown but Exception instead, i only catch this in the service
- [ ] TODO-LATER handle GameFullException in frontend, display game full

- [ ] TODO throw exception not return bool in DoesUsernameExsist and handle in controller
- [ ] TODO PlayerService UpdateUsername does not chech that the username is available
- [ ] TODO Add unique contraints to player username

- [ ] BUG UpdatePassword does not work properly

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
- [ ] Add logger service and controller
- [x] Admin Controller
- [x] Add change passord api
- [ ] Add signalR endpoints for game logic
- [ ] Go over design, try to find more api´s to implement
- [ ] Generate Mock data
- [ ] Test Backend
- [x] Implement JWT
- [ ] Test JWT implementation
- [ ] Add input validation
- [ ] Look for Edge-cases and fix them
- [ ] Add error handling in Services
- [ ] Add error handling in Repositories
- [ ] Create Docs for services and general
- [ ] Create Docs for Interfaces
- [ ] Add logic for adding payment solution for buying more gallery cards??

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
