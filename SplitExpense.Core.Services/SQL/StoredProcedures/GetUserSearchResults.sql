CREATE OR ALTER PROCEDURE GetUserSearchResult 
@UserId INT,
@Query	NVARCHAR(MAX),
@Limit	INT = 5
AS
SELECT 
TOP (@Limit)
U.Id,
CONCAT(U.FirstName, ' ', U.LastName) AS [Name],
U.DateOfBirth,
U.Email,
CASE WHEN UC.ConnectedUserId IS NULL THEN 0 ELSE 1 END IsConnection
FROM Users U
LEFT JOIN UserConnections UC ON UC.ConnectedUserId = U.Id AND UC.UserId = @UserId AND UC.IsDeleted = 0 AND U.Id <> @UserId
WHERE 
U.Id <> @UserId
AND U.Email LIKE('%'+@Query+'%')