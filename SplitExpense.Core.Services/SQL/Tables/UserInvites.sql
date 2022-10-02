CREATE TABLE UserInvites
(
	Id				INT					NOT NULL PRIMARY KEY IDENTITY(1,1),
	ReferralId		UNIQUEIDENTIFIER	NOT NULL CONSTRAINT unq_userinvites_referralId UNIQUE,
	UserId			INT					NOT NULL,
	ReferralType	TINYINT				NOT NULL,
	GroupId			INT					NULL,
	ExpiresOn		DATETIME			NULL,
	CreatedOn		DATETIME			NOT NULL CONSTRAINT df_userinvites_createdon DEFAULT(GETUTCDATE()),
	CreatedBy		NVARCHAR(256)		NOT NULL CONSTRAINT df_userinvites_createdby DEFAULT(CURRENT_USER),
	IsDeleted		BIT					NOT NULL CONSTRAINT df_userinvites_isdeleted DEFAULT(0),
	DateDeleted		DATETIME			NULL,
);