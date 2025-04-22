
-- 1) All logs of the time interval
EXEC dbo.GetLogsByDateRange
    @StartDate = '2025-04-20',
    @EndDate   = '2025-04-21';

-- 2) All GET requests with code 200
EXEC dbo.GetLogsByMethodAndStatus
    @Method     = 'GET',
    @StatusCode = 200;

-- 3) `/Users`, only codes = 302
EXEC dbo.GetLogsFiltered 
    @PathFilter='/User', 
    @Status=302

