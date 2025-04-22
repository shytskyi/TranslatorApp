USE [TranslatorApp]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--v0.1 | 21.04.2025 | Ivan Shytskyi |
CREATE OR ALTER   PROCEDURE [dbo].[GetLogsFiltered]
    @PathFilter  NVARCHAR(256)   = NULL,  
    @Status   INT             = NULL   
AS
BEGIN

    WITH OrderedLogs AS
    (
        SELECT
            Id,
            Timestamp,
            Path,
            Method,
            StatusCode,
            ROW_NUMBER() OVER (ORDER BY Timestamp DESC) AS RowNum
        FROM dbo.ApplicationLogs
        WHERE (@PathFilter IS NULL OR Path LIKE @PathFilter + '%')
         AND (@Status  IS NULL OR StatusCode = @Status)
    )
    SELECT
        Id,
        Timestamp,
        Path,
        Method,
        StatusCode
    FROM OrderedLogs

END
GO