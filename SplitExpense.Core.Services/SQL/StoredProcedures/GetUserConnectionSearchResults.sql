CREATE OR ALTER PROCEDURE GetUserConnectionSearchResults @UserId INT, 
														 @Query NVARCHAR(MAX) = NULL
AS

CREATE TABLE #tmpUserConnections
(
	Id				INT			NOT NULL PRIMARY KEY,
	UserId			INT			NOT NULL,
	ConnectedUserId	INT			NOT NULL,
);

INSERT INTO #tmpUserConnections
SELECT 
Id,
UserId,
ConnectedUserId
FROM UserConnections WHERE UserId = @UserId AND IsDeleted = 0;

EXEC('
SELECT 
	U.Id,
	CONCAT(U.FirstName, '' '', U.LastName) AS [Name],
	U.DateOfBirth,
	U.Email
FROM 
#tmpUserConnections TUC 
INNER JOIN Users U ON U.Id = TUC.ConnectedUserId AND U.IsDeleted = 0 AND U.IsActive = 1
WHERE FirstName LIKE ''%'+@Query+'%'' OR LastName LIKE ''%'+ @Query+'%'' OR Email LIKE ''%'+@Query+'%''
');

DROP TABLE #tmpUserConnections;
