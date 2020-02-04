CREATE TABLE [dbo].[ServiceRequests] (
    [id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompanyServiceID] BIGINT         NULL,
    [FullName]         NVARCHAR (MAX) NULL,
    [EmailID]          NVARCHAR (MAX) NULL,
    [MobileNoCC]       NVARCHAR (10)  NULL,
    [MobileNo]         NVARCHAR (50)  NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [CreatedOn]        DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

