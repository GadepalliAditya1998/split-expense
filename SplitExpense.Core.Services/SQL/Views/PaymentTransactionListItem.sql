CREATE OR ALTER VIEW PaymentTransactionListItem
AS
SELECT 
PT.Id,
PT.TransactionIdentifier,
PT.GroupId,
PT.PaidByUserId, 
(SELECT CONCAT([FirstName],' ', [LastName]) FROM Users WHERE Id = PT.PaidByUserId) AS PaidByName,
PT.PaidToUserId, 
(SELECT CONCAT([FirstName],' ', [LastName])  FROM Users WHERE Id = PT.PaidByUserId) AS PaidToName,
PT.Amount,
PT.PaymentMode,
PT.PaymentDate
FROM 
PaymentTransactions PT