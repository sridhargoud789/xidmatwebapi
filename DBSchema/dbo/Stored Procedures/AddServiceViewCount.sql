-- =============================================
-- Author:		Sridhar Goud
-- Create date: 2Feb2020
-- Description:	Customer Service Views Count
-- =============================================
CREATE PROCEDURE AddServiceViewCount 
	-- Add the parameters for the stored procedure here
	@ServiceID int = 0, 
	@p2 int = 0
AS
BEGIN
	INSERT INTO [dbo].[ServiceViewCounts](CompanyServiceID,ViewedOn)
	VALUES(@ServiceID,dbo.LocalDateTime())
END
