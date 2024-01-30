USE corbli;

SELECT User.ID, User.Username, RoleInfo.Role
FROM USERINFO AS User
JOIN USERROLES AS UserRole ON User.ID = UserRole.UserID
JOIN ROLES AS RoleInfo ON UserRole.RoleID = RoleInfo.ID; 