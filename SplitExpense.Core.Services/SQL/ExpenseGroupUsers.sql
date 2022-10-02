CREATE TABLE ExpenseGroupUsers
(
	Id				INT				NOT NULL PRIMARY KEY IDENTITY(1,1),
	GroupId			INT				NOT NULL,
	UserId			INT				NOT NULL,
	IsAdmin			BIT				NOT NULL CONSTRAINT df_expensegroupsusersusers_isadmin DEFAULT(0),
	CreatedBy		NVARCHAR(128)	NOT NULL CONSTRAINT df_expensegroupsusers_createdby DEFAULT(CURRENT_USER) ,
	CreatedOn		DATETIME		NOT NULL CONSTRAINT df_expensegroupsusers_createdon DEFAULT(GETUTCDATE()),
	ModifiedBy		NVARCHAR(128)	NULL CONSTRAINT df_expensegroupsusers_modifiedby DEFAULT(CURRENT_USER),
	ModifiedOn		DATETIME		NULL,
	IsDeleted		BIT				NOT NULL CONSTRAINT df_expensegroupsusers_isdeleted DEFAULT(0),
	DateDeleted		DATETIME		NULL
)