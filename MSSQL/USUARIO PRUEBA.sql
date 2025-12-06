USE [Project];
GO

INSERT INTO [Users] (UserFullName, UserName, PasswordHash, UserRolID)
VALUES (
    'Admin Sistema', 
    'admin', 
    HASHBYTES('SHA2_256', 'Trolelote.Con.Todo_2025!'), 
    1 -- Rol Admin
);
GO