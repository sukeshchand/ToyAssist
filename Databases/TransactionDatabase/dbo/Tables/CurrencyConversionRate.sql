CREATE TABLE [dbo].[CurrencyConversionRate] (
    [BaseCurrency]   VARCHAR (50)    NOT NULL,
    [ToCurrency]     VARCHAR (50)    NOT NULL,
    [ConversionRate] DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_CurrencyRate] PRIMARY KEY CLUSTERED ([BaseCurrency] ASC, [ToCurrency] ASC)
);

