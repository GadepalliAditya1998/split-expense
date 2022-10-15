CREATE TABLE UserConnections
(
	Id				INT			NOT NULL PRIMARY KEY IDENTITY(1,1),
	UserId			INT			NOT NULL,
	ConnectedUserId	INT			NOT NULL,
	CreatedOn		DATETIME	NOT NULL CONSTRAINT df_userconnections_createdon DEFAULT GETUTCDATE(),
	IsDeleted		BIT			NOT NULL CONSTRAINT df_userconnections_isdeleted DEFAULT 0,
	DateDeleted		DATETIME	NULL
);