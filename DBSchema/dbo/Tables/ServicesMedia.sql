CREATE TABLE [dbo].[ServicesMedia] (
    [id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [FileName]   NVARCHAR (MAX) NULL,
    [FilePath]   NVARCHAR (MAX) NULL,
    [ServicesID] BIGINT         NULL,
    [IsActive]   BIT            NULL,
    [CreatedOn]  DATETIME       NULL,
    [UpdatedOn]  DATETIME       NULL,
    [UpdatedBy]  BIGINT         NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

