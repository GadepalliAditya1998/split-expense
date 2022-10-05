CREATE OR ALTER PROCEDURE GetGroupUserWiseBalances @GroupId INT,
												   @UserId	INT
AS

CREATE TABLE #tmpGroupUsers
(
	GroupId			INT,
	UserId			INT
);

INSERT INTO #tmpGroupUsers
SELECT 
GroupId, 
UserId 
FROM ExpenseGroupUsers WHERE GroupId = @GroupId AND IsDeleted = 0;

CREATE TABLE #tmpGroupExpenses
(
	ExpenseId			INT,
	PaidByUser			INT,
	UserId				INT,
);

INSERT INTO #tmpGroupExpenses
SELECT 
Id,
PaidByUser,
UserId
FROM Expenses WHERE GroupId = @GroupId AND IsDeleted = 0;


CREATE TABLE #tmpExpenseShareUser
(
	ExpenseId			INT,
	UserId				INT,
	Balance				FLOAT,
	PaidByUser			INT
);

INSERT INTO #tmpExpenseShareUser
SELECT 
TGE.ExpenseId,
EU.UserId,
EU.Balance,
TGE.PaidByUser
FROM
#tmpGroupExpenses TGE
INNER JOIN ExpenseUsers EU 
ON EU.ExpenseId = TGE.ExpenseId AND EU.Balance > 0 AND EU.UserId <> TGE.PaidByUser AND EU.IsDeleted = 0


SELECT 
TGU.GroupId,
TGU.UserId,
CONCAT(U.FirstName, ' ', U.LastName) AS [Name],
ISNULL((SELECT SUM(Balance) FROM #tmpExpenseShareUser TUS WHERE TUS.PaidByUser = @UserId AND TUS.UserId = TGU.UserId),0) AS DebtAmount,
ISNULL((SELECT SUM(Balance) FROM #tmpExpenseShareUser TUS WHERE TUS.PaidByUser = TGU.UserId AND TUS.UserId = @UserId),0) AS LentAmount
FROM 
#tmpGroupUsers TGU
INNER JOIN Users U ON TGU.UserId = U.Id AND U.IsDeleted = 0 AND TGU.UserId <> @UserId;


DROP TABLE #tmpGroupUsers;
DROP TABLE #tmpGroupExpenses;
DROP TABLE #tmpExpenseShareUser;