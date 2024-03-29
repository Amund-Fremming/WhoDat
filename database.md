# Database

**Players**

- PlayerID (PK)
- Username
- PasswordHash
- Salt

**Galleries**

- GalleryID (PK)
- PlayerID (FK)

**Games**

- GameID (PK)
- PlayerOneID (FK)
- PlayerTwoID (FK)
- State
- Turn

**GameMessage**

- GameMessageID (PK)
- GameID (FK)
- Message

**Boards**

- BoardID (PK)
- GameID (FK)
- ChosenCard (FK)
- PlayersLeft

**Cards**

- CardID (PK)
- GalleryID (FK)
- Name
- Url
- Active

**BoardCards**

- BoardCardID (PK)
- BoardID (FK)
- CardID (FK)
