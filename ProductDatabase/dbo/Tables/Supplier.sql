CREATE TABLE [dbo].[Supplier] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [ContactName]  NVARCHAR (255) NULL,
    [ContactPhone] VARCHAR (50)   NULL,
    [ContactEmail] VARCHAR (255)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

