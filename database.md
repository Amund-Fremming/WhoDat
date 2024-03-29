# Database

**Player**

- PlayerID (PK)
- Username
- PasswordHash
- Salt

**Gallery**

- GalleryID (PK)
- PlayerID (FK)

**Game**

- GameID (PK)
- PlayerOneID (FK)
- PlayerTwoID (FK)

**Board**

- BoardID (PK)
- GameID (FK)
- ChoosenCard (FK)
- PlayersLeft

**Card**

- CardID (PK)
- GalleryID (FK)
- BoardID (FK)
- Name
- Url
- Active
