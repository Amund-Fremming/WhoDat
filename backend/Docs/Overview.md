# WhoDat backend Documentation

## Overview

The backend of the WhoDat application is structured as a REST-API, operating independently from the
frontend. It adheres to the Single Responsibility Principle, ensuring that logic related to different
models and their operations is clearly separated. Each model has its own repositories and services,
while controllers manage various application functionalities such as player management or game operations.
The core game logic is managed by a Hub class, which utilizes WebSockets with SignalR for real-time interaction.

### CloudFlare R2 For Image Storage And Handling

Our application uses a third-party library for image storage. When users upload images, they are passed to the
backend, which connects to a Cloudflare Worker for storage. The Cloudflare Worker uploads the image and returns
a URL. This URL is then provided to the frontend for access. Note that these URLs are publicly accessible, meaning
anyone with the URL can view the image. To enhance security and privacy, consider implementing access controls,
URL expiry times, and encryption to restrict access and protect user privacy.

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
