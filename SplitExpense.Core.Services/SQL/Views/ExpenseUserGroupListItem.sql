CREATE OR ALTER VIEW ExpenseUserGroupListItems
AS
SELECT 
EG.Id,
Name,
Description,
EGU.UserId,
IsAdmin
FROM ExpenseGroups EG 
INNER JOIN ExpenseGroupUsers EGU ON EGU.GroupId = EG.Id AND EG.IsDeleted = 0 AND EGU.IsDeleted = 0;