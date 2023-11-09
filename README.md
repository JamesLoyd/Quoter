# Quoter

## Features to add
- scrub the usernames in a quote, so it wont ping people
- add a method to remove quotes, based on mod roles
- add a "no quote" text phrase that will block the quoter from sniping a quote

## Technical TODO
- Wire up database and migrations :)
- find a good # of quotes to store, pop off the last ones
- restrict quotes to channels ?


User requests

* quote saving, quote recall, and quote deletion. deletion can only be done by a mod or the one who made the quote. quote saving and recall can be done by keyword. Quote recall and deletion can be done by Unique ID. If multiple quotes share a keyword then one is selected at random. A mod can delete all quotes by a user if needed. - Delta
* Gives mods etc permission to delete quotes... the old bot required admin permission to delete quotes so well with how inactive admins could be alot of them weren't deleted for a long time... something to keep in mind. - Tooski
