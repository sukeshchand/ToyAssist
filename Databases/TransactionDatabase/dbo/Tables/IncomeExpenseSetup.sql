CREATE TABLE [dbo].[IncomeExpenseSetup] (
    [IncomeExpenseSetupId] INT            IDENTITY (1, 1) NOT NULL,
    [StartDate]            DATETIME       NULL,
    [EndDate]              DATETIME       NULL,
    [IncomeExpenseType]    INT            NULL,
    [Amount]               FLOAT (53)     NULL,
    [Currency]             VARCHAR (50)   NULL,
    [Descr]                NVARCHAR (200) NULL,
    [NextPaymentDate]      DATETIME       NULL,
    [NextBillingDate]      DATETIME       NULL,
    [PaymentUrl]           NVARCHAR (500) NULL,
    [AccountLogInUrl]      NVARCHAR (500) NULL,
    CONSTRAINT [PK_IncomeExpenseSetup] PRIMARY KEY CLUSTERED ([IncomeExpenseSetupId] ASC)
);

