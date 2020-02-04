-- =============================================
-- Author:		Sridhar Goud
-- Create date: 2Feb2020
-- Description:	
-- =============================================
CREATE PROCEDURE CreateService 
	-- Add the parameters for the stored procedure here
@CompanyID bigint,
@MasterServiceID int,
@IsApproved bit,
@ApprovedOn datetime,
@IsActive bit,
@CreatedBy bigint,
@ServiceTitle nvarchar,
@ServiceDescription nvarchar(max),
@Timings nvarchar(100),
@CompanyServiceID int = null output
AS
BEGIN
	INSERT INTO [dbo].[CompanyServices] (CompanyID,MasterServiceID,IsApproved,ApprovedOn,IsActive,CreatedOn,CreatedBy,ServiceTitle,
ServiceDescription,Timings)
VALUES(@CompanyID,@MasterServiceID,@IsApproved,@ApprovedOn,@IsActive,dbo.LocalDateTime(),@CreatedBy,@ServiceTitle,
@ServiceDescription,@Timings)
set @CompanyServiceID = scope_identity()

END
