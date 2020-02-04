-- =============================================
-- Author:		Sridhar Goud
-- Create date: 
-- Description:	Add Update ServicesMedia Media
-- =============================================
CREATE PROCEDURE [dbo].[AddUpdateServicesMedia] 
@ServicesID bigint,
@Filenames nvarchar(max),
@Filepaths nvarchar(max)
AS
BEGIN
	insert into ServicesMedia(FileName,FilePath,ServicesID,IsActive,CreatedOn)
	select a.data,b.data,@ServicesID,1,dbo.LocalDateTime()
	from dbo.splitById(@Filenames,',') a
	join dbo.splitById(@Filepaths,',') b on b.id=a.id

END

