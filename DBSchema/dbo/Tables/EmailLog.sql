CREATE TABLE [dbo].[EmailLog] (
    [id]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [From]    NVARCHAR (MAX) NULL,
    [To]      NVARCHAR (MAX) NULL,
    [CC]      NVARCHAR (MAX) NULL,
    [BCC]     NVARCHAR (MAX) NULL,
    [Body]    NVARCHAR (MAX) NULL,
    [IsSent]  BIT            NULL,
    [SentOn]  DATETIME       NULL,
    [Subject] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

