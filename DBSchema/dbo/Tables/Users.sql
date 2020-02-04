CREATE TABLE [dbo].[Users] (
    [UserId]               INT            IDENTITY (1, 1) NOT NULL,
    [EmailId]              NVARCHAR (50)  NULL,
    [Password]             NVARCHAR (MAX) NULL,
    [PasswordSalt]         INT            NULL,
    [FirstName]            NVARCHAR (256) NULL,
    [LastName]             NVARCHAR (256) NULL,
    [Gender]               VARCHAR (10)   NULL,
    [DOB]                  DATE           NULL,
    [MobileNoCoountryCode] NVARCHAR (50)  NULL,
    [MobileNo]             NVARCHAR (50)  NULL,
    [PhoneNoCountryCode]   NVARCHAR (50)  NULL,
    [PhoneNo]              NVARCHAR (50)  NULL,
    [CreatedOn]            DATETIME       NULL,
    [CreatedBy]            NVARCHAR (50)  NULL,
    [IsActive]             BIT            NULL,
    [CompanyID]            BIGINT         NULL,
    [UpdatedOn]            DATETIME       NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

