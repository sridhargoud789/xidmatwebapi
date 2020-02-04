CREATE TABLE [dbo].[CompanyServices] (
    [id]                 BIGINT         IDENTITY (1, 1) NOT NULL,
    [CompanyID]          BIGINT         NULL,
    [MasterServiceID]    INT            NULL,
    [IsApproved]         BIT            NULL,
    [ApprovedOn]         DATETIME       NULL,
    [IsActive]           BIT            NULL,
    [CreatedOn]          DATETIME       NULL,
    [CreatedBy]          BIGINT         NULL,
    [ServiceTitle]       NVARCHAR (MAX) NULL,
    [ServiceDescription] NVARCHAR (MAX) NULL,
    [Timings]            NVARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

