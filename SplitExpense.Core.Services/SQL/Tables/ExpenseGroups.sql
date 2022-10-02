CREATE TABLE Users
(
	Id				INT				NOT NULL PRIMARY KEY IDENTITY(1,1),
	Name			NVARCHAR(256)	NOT NULL,
	Description		NVARCHAR(512)	NULL,
	UserId			INT				NOT NULL,
	CreatedBy		NVARCHAR(128)	NOT NULL CONSTRAINT df_expensegroups_createdby DEFAULT(CURRENT_USER) ,
	CreatedOn		DATETIME		NOT NULL CONSTRAINT df_expensegroups_createdon DEFAULT(GETUTCDATE()),
	ModifiedBy		NVARCHAR(128)	NULL CONSTRAINT df_expensegroups_modifiedby DEFAULT(CURRENT_USER),
	ModifiedOn		DATETIME		NULL,
	IsDeleted		BIT				NOT NULL CONSTRAINT df_expensegroups_isdeleted DEFAULT(0),
	DateDeleted		DATETIME		NULL
)