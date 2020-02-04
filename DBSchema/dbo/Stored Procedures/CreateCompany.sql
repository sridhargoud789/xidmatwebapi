-- =============================================
-- Author:		Sridhar Goud
-- Create date: 2Feb2020
-- Description:	
-- =============================================
CREATE PROCEDURE CreateCompany 
@CompanyName nvarchar(max),
@Description nvarchar(max),
@CompanyID int =null output
AS
BEGIN
	INSERT INTO Company(CompanyName,IsActive,Description,CreatedOn)values(@CompanyName,1,@Description,dbo.LocalDateTime())

	SET @CompanyID = SCOPE_IDENTITY()
END
