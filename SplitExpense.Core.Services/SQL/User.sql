﻿CREATE TABLE Users
(
	Id				INT				NOT NULL PRIMARY KEY IDENTITY(1,1),
	FirstName		NVARCHAR(256)	NOT NULL,
	MiddleName		NVARCHAR(256)	NULL,
	LastName		NVARCHAR(256)	NULL,
	ReferralId		INT				NULL,
	IsActive		BIT				NOT NULL CONSTRAINT  df_users_isactive DEFAULT(1),
	CreatedOn		DATETIME		NOT NULL CONSTRAINT df_users_createdon DEFAULT(GETUTCDATE()),
	ModifiedBy		NVARCHAR(128)	NULL CONSTRAINT df_users_modifiedby DEFAULT(CURRENT_USER),
	ModifiedOn		DATETIME		NULL,
	IsDeleted		BIT				NOT NULL CONSTRAINT df_users_isdeleted DEFAULT(0),
	DateDeleted		DATETIME		NULL
)