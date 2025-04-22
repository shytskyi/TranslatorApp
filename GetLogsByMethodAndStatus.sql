USE [TranslatorApp]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--v0.1 | 21.04.2025 | Ivan Shytskyi |
CREATE OR ALTER PROCEDURE [dbo].[GetLogsByMethodAndStatus]
    @Method     NVARCHAR(10)    = NULL,  -- 'GET', 'POST' 
    @StatusCode INT             = NULL   -- 200, 404, 500 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Timestamp,
        Path,
        Method,
        QueryString,
        StatusCode
    FROM dbo.ApplicationLogs
    WHERE (@Method     IS NULL OR Method     = @Method)
      AND (@StatusCode IS NULL OR StatusCode = @StatusCode)
    ORDER BY Timestamp DESC;
END
GO