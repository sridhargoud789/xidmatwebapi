CREATE TABLE [dbo].[ServiceViewCounts] (
    [id]               BIGINT   IDENTITY (1, 1) NOT NULL,
    [CompanyServiceID] BIGINT   NULL,
    [ViewedOn]         DATETIME NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

