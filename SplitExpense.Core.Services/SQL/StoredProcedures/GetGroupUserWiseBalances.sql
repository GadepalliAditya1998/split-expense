CREATE OR ALTER PROCEDURE GetGroupUserWiseBalances @GroupId INT,
												   @UserId	INT
AS

DECLARE @TotalAmountPaidByUser INT = 0;

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
	Amount				FLOAT,
	PaidByUser			INT
);

INSERT INTO #tmpExpenseShareUser
SELECT 
TGE.ExpenseId,
EU.UserId,
EU.Amount,
TGE.PaidByUser
FROM
#tmpGroupExpenses TGE
INNER JOIN ExpenseUsers EU 
ON EU.ExpenseId = TGE.ExpenseId AND EU.Balance > 0 AND EU.UserId <> TGE.PaidByUser AND EU.IsDeleted = 0

CREATE TABLE #tmpGroupUserPayments
(
PaidByUserId		INT,
PaidToUserId		INT,
TotalAmountPaid		FLOAT
);

INSERT INTO #tmpGroupUserPayments
(PaidByUserId, PaidToUserId, TotalAmountPaid)
SELECT 
PaidByUserId, 
PaidToUserId, 
SUM(Amount) AS TotalAmountPaid
FROM PaymentTransactions 
WHERE GroupId = @GroupId AND IsDeleted = 0
GROUP BY PaidByUserId, PaidToUserId

SELECT 
TGU.GroupId,
TGU.UserId,
CONCAT(U.FirstName, ' ', U.LastName) AS [Name],
ISNULL((SELECT SUM(Amount) FROM #tmpExpenseShareUser TUS WHERE TUS.PaidByUser = @UserId AND TUS.UserId = TGU.UserId),0) AS TotalLentAmount,
ISNULL((SELECT SUM(Amount) FROM #tmpExpenseShareUser TUS WHERE TUS.PaidByUser = TGU.UserId AND TUS.UserId = @UserId),0) AS TotalOwingAmount,
ISNULL((SELECT SUM(TotalAmountPaid) FROM #tmpGroupUserPayments TGUP WHERE TGUP.PaidByUserId = @UserId AND TGUP.PaidToUserId = TGU.UserId), 0) AS PaidAmount,
ISNULL((SELECT SUM(TotalAmountPaid) FROM #tmpGroupUserPayments TGUP WHERE TGUP.PaidByUserId = TGU.UserId AND TGUP.PaidToUserId =  @UserId), 0) AS AmountReturned
FROM 
#tmpGroupUsers TGU
INNER JOIN Users U ON TGU.UserId = U.Id AND U.IsDeleted = 0 AND TGU.UserId <> @UserId;


DROP TABLE #tmpGroupUsers;
DROP TABLE #tmpGroupExpenses;
DROP TABLE #tmpExpenseShareUser;