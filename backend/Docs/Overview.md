# WhoDat backend Documentation

## Overview

The backend of the WhoDat application is structured as a REST-API, operating independently from the
frontend. It adheres to the Single Responsibility Principle, ensuring that logic related to different
models and their operations is clearly separated. Each model has its own repositories and services,
while controllers manage various application functionalities such as player management or game operations.
The core game logic is managed by a Hub class, which utilizes WebSockets with SignalR for real-time interaction.

### Repository

This layer is dedicated to direct data manipulation and does not include any business logic. A key aspect to
note here is the GetById functions, which are designed to throw a KeyNotFoundException if an entry does not
exist. This exception is caught in the controllers, allowing for the correct HTTP response (NotFound(...)) to
be returned to the user.

### Service

The service layer is responsible for most of the application's logic and permission checks. While most service
methods are documented in their respective interfaces, some important methods such as PlayerHasPermission,
PlayerHasGamePermission, and CanSendMessage are designed solely to verify user permissions. These methods ensure
that players can only interact with objects they own or are a part of, like a game. On fail, these methods will
throw so we can handle them in the controllers and return the right HTTP method.

### Controller

Controllers handle incoming requests from the frontend and generate responses. Although controllers do not
strictly adhere to the Single Responsibility Principle and may utilize multiple services, each controller
is tasked with specific responsibilities. For example, controllers handle player actions like password changes
or adding more cards to their galleries. Key methods include HandleException, which processes exceptions and
returns appropriate HTTP codes; ParsePlayerIdClaim, which extracts player IDs from JWTs; and EncodeForJsAndHtml,
which escapes HTML and JavaScript inputs for security reasons.

### Hub

Operating at the same level as controllers, the Hub uses SignalR to manage WebSockets. It is crucial for live
updates and real-time management of game states and related information. This component is central to the
application's functionality, especially for interactive game elements. Like the controllers, this class has a
function for handling errors and returning the right message, a parsing of player id and encoding HTML and JS input.

### Some special service functions

**CreateGame**: creates one board for making the board for the game, we later duplicate this board so each player
gets the same board.
**UpdateBoardCards**: updates the boardcards activity, and returns the number of active cards left on the board.
**GetBoardWithBoardCards**: creates the duplicate board that was made when the `CreateGame` was called if the
second board was not created, if it was, their board is returned.
**CreateBoardCards**: creates incomming boardcards from cards. Does also validate what king of game, so if only host
is supposed to choose cards the second player is not allowed. Does also update the state and return it. This is
important for the game state when the players are choosing their cards so we can track when both are ready or which
player is still choosing, or if both are finished so we can do the next action.
