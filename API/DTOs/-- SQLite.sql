-- SQLite
INSERT INTO Users (Id, UserName)
VALUES (16, "roman");

DELETE Users WHERE Id NOT IN (SELECT AppUserId from Photos);