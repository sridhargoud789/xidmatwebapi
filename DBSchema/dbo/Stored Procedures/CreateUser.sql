-- =============================================
-- Author:		Sridhar Goud
-- Create date: 2Feb2020
-- Description:	
-- =============================================
CREATE PROCEDURE CreateUser 
	-- Add the parameters for the stored procedure here
@EmailId nvarchar(50),
@Password nvarchar(max),
@PasswordSalt int,
@FirstName nvarchar(256),
@LastName nvarchar(256),
@Gender varchar(10),
@DOB date,
@MobileNoCoountryCode nvarchar(50),
@MobileNo nvarchar(50),
@PhoneNoCountryCode nvarchar(50),
@PhoneNo nvarchar(50),
@CompanyID bigint,
@UserID bigint =null output
AS
BEGIN
	INSERT INTO Users(EmailId,Password,PasswordSalt,FirstName,LastName,Gender,DOB,MobileNoCoountryCode,MobileNo,PhoneNoCountryCode,
						PhoneNo,CreatedOn,CompanyID)
		VALUES(@EmailId,@Password,@PasswordSalt,@FirstName,@LastName,@Gender,@DOB,@MobileNoCoountryCode,@MobileNo,@PhoneNoCountryCode,
						@PhoneNo,dbo.LocalDateTime(),@CompanyID)

		set @UserID = SCOPE_IDENTITY()
END
