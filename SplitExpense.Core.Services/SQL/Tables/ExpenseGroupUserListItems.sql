CREATE VIEW ExpenseGroupUserListItems
AS
SELECT 
U.Id,
CONCAT(U.FirstName, ' ', U.LastName) AS [Name],
EGU.[GroupId],
EGU.[IsAdmin],
U.[IsActive],
U.[Email]
FROM
ExpenseGroupUsers EGU
INNER JOIN Users U ON U.Id = EGU.UserId AND U.IsDeleted = 0