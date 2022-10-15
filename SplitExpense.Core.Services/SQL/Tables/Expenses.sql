CREATE TABLE Expenses
(
	Id				INT				NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name			NVARCHAR(256)	NOT NULL,
	Description		NVARCHAR(512)	NULL,
	UserId			INT				NOT NULL,
	GroupId			INT				NULL,
	PaidByUser		INT				NOT NULL,
	ExpenseDate		DateTime		NOT NULL,
	Amount			FLOAT			NOT NULL,
	SplitType		TINYINT			NOT NULL,
	CreatedBy		NVARCHAR(128)	NOT NULL CONSTRAINT df_expenses_createdby DEFAULT(CURRENT_USER) ,
	CreatedOn		DATETIME		NOT NULL CONSTRAINT df_expenses_createdon DEFAULT(GETUTCDATE()),
	ModifiedBy		NVARCHAR(128)	NULL CONSTRAINT df_expenses_modifiedby DEFAULT(CURRENT_USER),
	ModifiedOn		DATETIME		NULL,
	IsDeleted		BIT				NOT NULL CONSTRAINT df_expenses_isdeleted DEFAULT(0),
	DateDeleted		DATETIME		NULL
)