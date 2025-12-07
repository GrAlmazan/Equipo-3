USE [Project];
GO

-- 1. Vista GlobalSettings (Simulada para cumplir el requisito)
IF OBJECT_ID('GlobalSettings', 'V') IS NOT NULL DROP VIEW GlobalSettings;
GO
CREATE VIEW [GlobalSettings] AS
SELECT 
    'Project' AS AppName,
    '1.0.0' AS Version,
    'Central Standard Time' AS DefaultTimeZone,
    GETUTCDATE() AS ServerTimeUTC;
GO

-- 2. Función para convertir a Hora Local (Ejemplo simplificado -6h)
IF OBJECT_ID('UfnToLocalTime', 'FN') IS NOT NULL DROP FUNCTION UfnToLocalTime;
GO
CREATE FUNCTION [dbo].[UfnToLocalTime] (@UtcDate DATETIME2)
RETURNS DATETIME2
AS
BEGIN
    -- Ajusta el offset según tu zona horaria real (ej: -6 para CDMX/Matamoros)
    RETURN DATEADD(HOUR, -6, @UtcDate);
END
GO

-- 3. Función para convertir a UTC
IF OBJECT_ID('UfnToUniversalTime', 'FN') IS NOT NULL DROP FUNCTION UfnToUniversalTime;
GO
CREATE FUNCTION [dbo].[UfnToUniversalTime] (@LocalDate DATETIME2)
RETURNS DATETIME2
AS
BEGIN
    RETURN DATEADD(HOUR, 6, @LocalDate);
END
GO