USE [TranslatorApp]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--v0.1 | 21.04.2025 | Ivan Shytskyi |
CREATE OR ALTER PROCEDURE [dbo].[GetLogsByDateRange]
    @StartDate DATETIME,
    @EndDate   DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Timestamp,
        Path,
        Method,
        QueryString,
        RequestBody,
        ResponseBody,
        StatusCode
    FROM [dbo].[ApplicationLogs]
    WHERE Timestamp BETWEEN @StartDate AND @EndDate
    ORDER BY Timestamp ASC;
END
GO


