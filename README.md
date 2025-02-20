# LibraryManagementSystem

## Setup

## Database

We use SQLite. The *database.db* file should have some example data.

~~~~SQL
CREATE TABLE Authors (
    Id          INTEGER       PRIMARY KEY AUTOINCREMENT
                              NOT NULL
                              UNIQUE,
    Name        TEXT (0, 255) NOT NULL
                              DEFAULT [missing name],
    DateOfBirth TEXT
);


CREATE TABLE Books (
    Id              INTEGER       PRIMARY KEY AUTOINCREMENT
                                  NOT NULL
                                  UNIQUE,
    Title           TEXT (0, 255) NOT NULL
                                  DEFAULT [missing title],
    PublicationYear INTEGER,
    AuthorId        INTEGER       REFERENCES Authors (Id) ON DELETE SET NULL
                                                          ON UPDATE CASCADE
                                                          MATCH SIMPLE
                                  NOT NULL
                                  DEFAULT ( -99999) 
);

~~~~

#### notes 

SQLite was chosen because it was familiar and presumably easier to set up than postgresql. There are certain aspects of SQLite that make it a sub-optimal choice: Worse concurrency, no DateTime datatype (we use ISO 8601) etc. If scaling was necessary we would reconsider the choice of database.


## API

Requests can be tested at https://localhost:7078/swagger/index.html. They should work as described in the requirements. 

- /books is paginated, default page size is 50, link to next page is returned in header
- when POSTing, IDs for books and authors are ignored by the controller (autoincrement is used)


## similarity search, performance

- Levenstein distance (https://en.wikipedia.org/wiki/Levenshtein_distance#Definition) is used to find similar strings. We return strings that have a distance of 1 or less.

- Things that could be improved/added:
	- a common error when typing is to swap two adjacent letters: camera/camrea. We can could catch this by using a levenstein distance of 2, but we might get too many bad results. Damerau–Levenshtein distance could be used instead (https://en.wikipedia.org/wiki/Damerau%E2%80%93Levenshtein_distance)

	- The naive algorithm we use for the levenstein distance isn't too efficient. We assume that the effect is not too big, since most titles are short.

	- We have to load the entire set of books into memory when we search. We could instead generate tags containing similar strings in the database, and use those when querying. 

	- We should consider adding indexes to the database to speed up queries