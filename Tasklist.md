# Tasklist

## TODO

- BUG? : SendMessage needs a bool, is reply for updating to the right state. now a reply wil change the state to the p2 asked

- Display oponent username/profile picture
- Convert images to webP on upload
- Are you sure when trying to leave

## Refactor

Gameplay

- FE: only send boardcard ids to backend, backend handles updating, dont use includes and calculate, rather make a new state to jsut have the values
- BE: rewrite the update boardcards method, it can be a lot simpler

FlipCard

- Make flip card more readable
