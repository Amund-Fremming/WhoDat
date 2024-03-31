# Database

**Player**

- PlayerID (PK)
- Username
- PasswordHash
- Salt

**Gallerie**

- GalleryID (PK)
- PlayerID (FK)

**Game**

- GameID (PK)
- PlayerOneID (FK)
- PlayerTwoID (FK)
- State
- Turn

**Message**

- GameMessageID (PK)
- GameID (FK)
- Message

**Board**

- BoardID (PK)
- GameID (FK)
- ChosenCard (FK)
- PlayersLeft

**Card**

- CardID (PK)
- GalleryID (FK)
- Name
- Url
- Active

**BoardCards**

- BoardCardID (PK)
- BoardID (FK)
- CardID (FK)
