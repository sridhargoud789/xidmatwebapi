-- =============================================
-- Author:		Sridhar Goud
-- Create date: 2Feb2020
-- Description:	Get Local datetime
-- =============================================
CREATE FUNCTION LocalDateTime 
(
	
)
RETURNS datetime
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result datetime

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = dateadd(hh,4,getutcdate())

	-- Return the result of the function
	RETURN @Result

END
