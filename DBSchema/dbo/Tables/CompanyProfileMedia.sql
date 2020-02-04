CREATE TABLE [dbo].[CompanyProfileMedia] (
    [id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [FileName]  NVARCHAR (MAX) NULL,
    [FilePath]  NVARCHAR (MAX) NULL,
    [CompanyID] BIGINT         NULL,
    [IsActive]  BIT            NULL,
    [CreatedOn] DATETIME       NULL,
    [UpdatedOn] DATETIME       NULL,
    [UpdatedBy] INT            NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

