-- 1. Crear la Base de Datos
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Project')
BEGIN
    CREATE DATABASE [Project];
END
GO

USE [Project];
GO

-- =============================================
-- PARTE 1: LOGIN Y USUARIOS (Del archivo original)
-- =============================================

-- 2. Roles del Sistema (Admin, User)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SystemRoles' AND xtype='U')
BEGIN
    CREATE TABLE [SystemRoles](
        [RolID] BIGINT IDENTITY(1,1) PRIMARY KEY,
        [Rol]   NVARCHAR(50) NOT NULL
    );
    -- Insertar roles por defecto
    INSERT INTO [SystemRoles] (Rol) VALUES ('Admin'),('User');
END
GO

-- 3. Usuarios
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE [Users](
        [UserID]       BIGINT IDENTITY(1,1) PRIMARY KEY,
        [UserFullName] NVARCHAR(100) NOT NULL,
        [UserName]     NVARCHAR(50)  NOT NULL,
        [PasswordHash] VARBINARY(256) NOT NULL,
        [UserRolID]    BIGINT NOT NULL,
        CONSTRAINT FK_Users_SystemRoles
            FOREIGN KEY (UserRolID) REFERENCES [SystemRoles](RolID)
    );
    -- Índice único para que no se repitan usuarios
    CREATE UNIQUE INDEX UX_Users_UserName ON [Users]([UserName]);
END
GO

-- =============================================
-- PARTE 2: INVENTARIO (Nuevo)
-- =============================================

-- 4. Categorías
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
BEGIN
    CREATE TABLE [Categories] (
        [CategoryID] INT IDENTITY(1,1) PRIMARY KEY,
        [Name]       NVARCHAR(100) NOT NULL,
        [Description] NVARCHAR(255) NULL
    );
END
GO

-- 5. Productos
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
BEGIN
    CREATE TABLE [Products] (
        [ProductID]   BIGINT IDENTITY(1,1) PRIMARY KEY,
        [Name]        NVARCHAR(150) NOT NULL,
        [SKU]         NVARCHAR(50)  NOT NULL UNIQUE,
        [Price]       DECIMAL(18, 2) NOT NULL DEFAULT 0,
        [CategoryID]  INT NULL,
        [IsActive]    BIT DEFAULT 1,
        CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryID) REFERENCES [Categories](CategoryID)
    );
END
GO

-- 6. Inventario (Stock Actual)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Inventory' AND xtype='U')
BEGIN
    CREATE TABLE [Inventory] (
        [InventoryID] BIGINT IDENTITY(1,1) PRIMARY KEY,
        [ProductID]   BIGINT NOT NULL UNIQUE,
        [Stock]       INT NOT NULL DEFAULT 0,
        [LastUpdated] DATETIME2 DEFAULT GETUTCDATE(),
        CONSTRAINT FK_Inventory_Products FOREIGN KEY (ProductID) REFERENCES [Products](ProductID)
    );
END
GO

-- 7. Historial de Movimientos (Vinculado a Usuarios)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='InventoryMovements' AND xtype='U')
BEGIN
    CREATE TABLE [InventoryMovements] (
        [MovementID]   BIGINT IDENTITY(1,1) PRIMARY KEY,
        [ProductID]    BIGINT NOT NULL,
        [UserID]       BIGINT NOT NULL, -- Relación con la tabla Users creada arriba
        [Quantity]     INT NOT NULL,    -- Positivo (entrada) o negativo (salida)
        [Reason]       NVARCHAR(200) NULL,
        [MovementDate] DATETIME2 DEFAULT GETUTCDATE(),
        
        CONSTRAINT FK_Movements_Products FOREIGN KEY (ProductID) REFERENCES [Products](ProductID),
        CONSTRAINT FK_Movements_Users    FOREIGN KEY (UserID)    REFERENCES [Users](UserID)
    );
END
GO