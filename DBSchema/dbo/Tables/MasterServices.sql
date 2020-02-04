CREATE TABLE [dbo].[MasterServices] (
    [id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [IsActive]    BIT            NULL,
    [ImagePath]   NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

