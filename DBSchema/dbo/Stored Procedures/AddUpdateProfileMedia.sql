-- =============================================
-- Author:		Sridhar Goud
-- Create date: 
-- Description:	Add Update Company Profile Media
-- =============================================
CREATE PROCEDURE AddUpdateProfileMedia 
@CompanyID bigint,
@Filenames nvarchar(max),
@Filepaths nvarchar(max)
AS
BEGIN
	insert into CompanyProfileMedia(FileName,FilePath,CompanyID,IsActive,CreatedOn)
	select a.data,b.data,@CompanyID,1,dbo.LocalDateTime()
	from dbo.splitById(@Filenames,',') a
	join dbo.splitById(@Filepaths,',') b on b.id=a.id

END
