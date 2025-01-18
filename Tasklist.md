# Tasklist

## TODO

- Handle error messages when GameHub does not work, many methods just call and forget, making the fe possible to stay frozen or dont reply
- Convert images to webP on upload
- Are you sure when trying to leave

## Refactor

Gameplay

- FE: only send boardcard ids to backend, backend handles updating, dont use includes and calculate, rather make a new state to jsut have the values
- BE: rewrite the update boardcards method, it can be a lot simpler

FlipCard

- Make flip card more readable
