CREATE OR ALTER PROCEDURE GetGroupExpenseListItems  @GroupId INT,
                                                    @UserId  INT
AS

CREATE TABLE #tmpGroupExpenses
(
    [Id]            INT               NOT NULL,
    [GroupId]       INT               NOT NULL,
    [Name]          NVARCHAR(256)     NOT NULL,
    [Description]   NVARCHAR(512)     NULL,
    [UserId]        INT               NOT NULL,
    [PaidByUser]    INT               NOT NULL,
    [Amount]        FLOAT             NOT NULL,
    [SplitType]     TINYINT           NOT NULL,
    [CreatedOn]     DATETIME          NOT NULL
);

CREATE TABLE #tmpUserExpenses
(
    [Id]            INT               NOT NULL,
    [ExpenseId]     INT               NOT NULL,
    [UserId]        INT               NOT NULL,
    [Amount]        FLOAT             NOT NULL
);

INSERT INTO #tmpGroupExpenses
SELECT 
[Id],         
[GroupId],    
[Name],       
[Description],
[UserId],     
[PaidByUser], 
[Amount],     
[SplitType],
[CreatedOn]
FROM Expenses
WHERE GroupId = @GroupId AND IsDeleted = 0;

INSERT INTO #tmpUserExpenses
SELECT 
[Id],
[ExpenseId],
[UserId],
[Amount]
FROM ExpenseUsers
WHERE ExpenseId IN (SELECT Id FROM #tmpGroupExpenses)
AND UserId = @UserId
AND IsDeleted = 0


SELECT 
TGE.[Id] AS ExpenseId,
TGE.GroupId,
TGE.[Name],
TGE.[Description],
TGE.[PaidByUser] AS PaidBy,
CONCAT(U.FirstName, ' ', U.LastName) AS PaidByName,
TGE.[Amount] AS PaidAmount,
(TGE.Amount - TUE.Amount) AS ToBePaidAmount,
TGE.CreatedOn,
(CASE WHEN TGE.PaidByUser = @UserId THEN 1 ELSE 0 END) AS IsLent
FROM 
#tmpGroupExpenses TGE
INNER JOIN #tmpUserExpenses TUE ON TUE.ExpenseId = TGE.Id
INNER JOIN Users U ON U.Id = TGE.PaidByUser AND U.IsActive = 1 AND U.IsDeleted = 0

DROP TABLE #tmpGroupExpenses;
DROP TABLE #tmpUserExpenses;