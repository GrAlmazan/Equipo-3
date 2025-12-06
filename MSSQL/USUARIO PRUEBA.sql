USE [Project];
GO

INSERT INTO [Users] (UserFullName, UserName, PasswordHash, UserRolID)
VALUES (
    'Admin Sistema', 
    'admin', 
    HASHBYTES('SHA2_256', '123456'), 
    1 -- Rol Admin
);
GO