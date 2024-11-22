# TODO REFACTOR

## Refactoring
- [ ] Make State object that game can hold for all state logic
- [ ] RUN NEW MIGRATIONS, NAMES HAVE CHANGED
- [ ] Services direct return await on repo funcitons (I changed away from this, but maybe it is better)
- [ ] Når repo skal oppdatere no etrenger jeg ikke 4 funksjoner, bare en. Kanskje flytte til generisk repo, med base klasse implementert?

### Refactor later
- [ ] Identity provider
- [ ] Options pattern
- [ ] State in game to a own object ??
- [ ] Refactor bloaded classes (Board- and BoardCardService)

### TODO
- [ ] Run new migrations, test login and so on.
- [ ] Understand how to add IRepo, maybe implement this in repo classes, then extend RepoBase in the repo classes
- [ ] Implement The base repos
- [ ] Maybe possible to make generic services or controllers
