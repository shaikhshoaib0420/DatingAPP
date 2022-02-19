-- SQLite
SELECT Id, PasswordSalt, UserName, PasswordHash, City, Country, Created, DateOfBirth, Gender, Interests, Introduction, KnownAs, LastActive, LookingFor
FROM Users;

SELECT Introduction 
FROM Users
WHERE UserName = "Pansy";