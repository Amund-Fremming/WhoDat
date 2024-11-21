# TODO REFACTOR

## Result Pattern + Logging
- [x] Admin
- [x] Auth
- [ ] Board
- [ ] BoardCard
- [ ] Card
- [ ] Game 
- [ ] Hub
- [ ] Message
- [x] Player


MIGHT ME SOME PLACES WHERE I WROTE GAMESTATE AND NOT GAMEENTITY

HUB KAN F� RESULTS MED FEIL, DISSE M� SENDES P� SAMME KANAL SOM VED SUKSESS OG M� VISES TIL SLUTTBRUKER, OG DISCONNECTE

## Refactoring
- [x] Enum to its use case or Shared
- [x] Exception to its use case or Shared
- [x] ImageHandler to its use case or Shared
- [x] Rename prohect files to backend not RaptorProject
- [x] Change namespaces to match the projectname and features
- [ ] RUN NEW MIGRATIONS, NAMES HAVE CHANGED
- [x] Change namespace to match folder structure
- [ ] Make State object that game can hold for all state logic
- [ ] Solid
- [ ] Dto from class to record
- [ ] Implement caching
- [ ] use [FromServices]
- [ ] N�r repo skal oppdatere no etrenger jeg ikke 4 funksjoner, bare en. Kanskje flytte til generisk repo, med base klasse implementert?

### Refactor later
- [ ] Identity provider
- [ ] Options pattern
- [ ] N�r fail skjer i repo, m� service ofte retunrere annen type Data.
      Ender ofte med � m�tte retunere ny tuple eller ny Result, skulle gjerne
      kunnet retunrert resultatet direkte fra repo i service og videre opp.

