CREATE DATABASE [Project];
GO

USE [Project];
GO

CREATE TABLE [SystemRoles](
    [RolID] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [Rol]   NVARCHAR(50) NOT NULL
);
GO

CREATE TABLE [Users](
    [UserID]       BIGINT IDENTITY(1,1) PRIMARY KEY,
    [UserFullName] NVARCHAR(100) NOT NULL,
    [UserName]     NVARCHAR(50)  NOT NULL,
    [PasswordHash] VARBINARY(256) NOT NULL,
    [UserRolID]    BIGINT NOT NULL,
    CONSTRAINT FK_Users_SystemRoles
        FOREIGN KEY (UserRolID) REFERENCES [SystemRoles](RolID)
);
GO

CREATE UNIQUE INDEX UX_Users_UserName ON [Users]([UserName]);

INSERT INTO [SystemRoles] (Rol) VALUES ('Admin'),('User')