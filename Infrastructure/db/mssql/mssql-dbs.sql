USE [master]
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ClientsDB')
  BEGIN
    CREATE DATABASE [ClientsDB]
END
GO

USE [ClientsDB]
GO

IF OBJECT_ID(N'[Person]') IS NULL
BEGIN
    BEGIN TRANSACTION;
        CREATE TABLE [Person] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(50) NULL,
        [Genre] nvarchar(1) NOT NULL,
        [Age] int NOT NULL,
        [Identification] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NULL,
        [Phone] nvarchar(max) NULL,
        [Discriminator] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NULL,
        [Status] bit NULL,
        CONSTRAINT [PK_Person] PRIMARY KEY ([Id])
        );

        COMMIT;
END;

USE [master]
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'AccountsDB')
  BEGIN
    CREATE DATABASE [AccountsDB]
END
GO

USE [AccountsDB]
GO

IF OBJECT_ID(N'[Account]') IS NULL
CREATE TABLE [Account] (
    [Id] uniqueidentifier NOT NULL,
    [Number] nvarchar(max) NOT NULL,
    [Type] nvarchar(max) NULL,
    [InitialBalance] decimal(14,2) NOT NULL,
    [Status] bit NOT NULL,
    [ClientIdentification] nvarchar(max) NOT NULL,
    [ClientName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY ([Id]),
);
GO
CREATE TABLE [Client] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NULL,
    [Identification] nvarchar(max) NULL,
    [Status] bit NOT NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY ([Id])
);
GO

USE [master]
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'TransactionsDB')
  BEGIN
    CREATE DATABASE [TransactionsDB]
END
GO

USE [TransactionsDB]
GO

IF OBJECT_ID(N'[Transaction]') IS NULL
BEGIN TRANSACTION;
GO

CREATE TABLE [Transaction] (
    [Id] uniqueidentifier NOT NULL,
    [Type] nvarchar(max) NULL,
    [Date] datetime2 NOT NULL,
    [Amount] decimal(18,4) NOT NULL,
    [Balance] decimal(18,4) NOT NULL,
    [AccountNumber] nvarchar(max) NOT NULL DEFAULT N'',
    CONSTRAINT [PK_Transaction] PRIMARY KEY ([Id]),
);
GO

COMMIT;
GO
