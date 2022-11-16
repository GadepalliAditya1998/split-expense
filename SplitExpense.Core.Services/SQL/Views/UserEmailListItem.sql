CREATE OR ALTER VIEW UserEmailListItems
AS
SELECT 
Id,
CONCAT(FirstName, ' ', LastName) AS Name,
Email
FROM Users WHERE IsDeleted = 0;
