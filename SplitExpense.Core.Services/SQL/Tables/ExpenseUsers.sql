CREATE TABLE ExpenseUsers
(
	Id				INT				NOT NULL PRIMARY KEY IDENTITY(1,1),
	ExpenseId		INT				NOT NULL,
	UserId			INT				NOT NULL,
	Amount			FLOAT			NOT NULL,
	Balance			FLOAT			NOT NULL,
	CreatedBy		NVARCHAR(128)	NOT NULL CONSTRAINT df_expenseusers_createdby DEFAULT(CURRENT_USER) ,
	CreatedOn		DATETIME		NOT NULL CONSTRAINT df_expenseusers_createdon DEFAULT(GETUTCDATE()),
	ModifiedBy		NVARCHAR(128)	NULL CONSTRAINT df_expenseusers_modifiedby DEFAULT(CURRENT_USER),
	ModifiedOn		DATETIME		NULL,
	IsDeleted		BIT				NOT NULL CONSTRAINT df_expenseusers_isdeleted DEFAULT(0),
	DateDeleted		DATETIME		NULL
)