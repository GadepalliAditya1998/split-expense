CREATE TABLE PaymentTransactions
(
	Id								INT							NOT NULL PRIMARY KEY IDENTITY(1,1),
	TransactionIdentifier			UNIQUEIDENTIFIER			NOT NULL,
	GroupId							INT							NOT NULL,
	PaidByUserId					INT							NOT NULL,
	PaidToUserId					INT			NOT NULL,
	Amount				FLOAT		NOT NULL,
	PaymentMode			TINYINT		NOT NULL,
	PaymentDate			DATETIME	NOT NULL CONSTRAINT df_paymenttransactions_paymentdate DEFAULT GETUTCDATE(),
	CreatedOn			DATETIME	NOT NULL CONSTRAINT df_paymenttransactions_createdon DEFAULT GETUTCDATE(),
	CreatedBy			NVARCHAR(128)	NOT NULL CONSTRAINT df_paymenttransactions_createdby DEFAULT SYSTEM_USER,
	IsDeleted			BIT			NOT NULL Constraint df_paymenttransactions_isdeleted	DEFAULT(0),
	DateDeleted			DATETIME	NULL
);