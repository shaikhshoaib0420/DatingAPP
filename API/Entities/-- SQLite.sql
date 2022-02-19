-- SQLite
SELECT Id, PasswordSalt, UserName, PasswordHash, City, Country, Created, DateOfBirth, Gender, Interests, Introduction, KnownAs, LastActive, LookingFor
FROM Users
WHERE UserName = 'lisa'