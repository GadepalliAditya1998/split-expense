CREATE OR ALTER PROCEDURE GetUserConnectionListItems @UserId INT
AS
DECLARE @UserReferrals TABLE
(
	ReferralId		INT
);
INSERT INTO @UserReferrals
SELECT Id FROM UserInvites WHERE UserId = @UserId;


SELECT 
UC.Id,
U.Id AS UserId,
CONCAT(U.FirstName, ' ', U.LastName) AS UserName,
U.Email AS UserEmail,
CASE WHEN EXISTS(SELECT 1 FROM @UserReferrals UR WHERE UR.ReferralId = U.ReferralId) THEN 1 ELSE 0 END AS IsReferred,
CASE WHEN EXISTS(SELECT 1 FROM @UserReferrals UR WHERE UR.ReferralId = U.ReferralId) THEN U.CreatedOn ELSE NULL END AS JoinByReferralOn
FROM 
UserConnections UC
INNER JOIN Users U ON U.Id = UC.ConnectedUserId AND UC.UserId = @UserId AND UC.IsDeleted = 0;

GO