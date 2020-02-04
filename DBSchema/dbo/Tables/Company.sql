CREATE TABLE [dbo].[Company] (
    [id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompanyName] NVARCHAR (MAX) NULL,
    [IsActive]    BIT            NULL,
    [Description] NVARCHAR (MAX) NULL,
    [CreatedOn]   DATETIME       NULL,
    [UpdatedOn]   DATETIME       NULL,
    [UpdatedBy]   INT            NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

