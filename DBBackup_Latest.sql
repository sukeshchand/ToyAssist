/****** Object:  Table [dbo].[Account]    Script Date: 13-02-2024 Feb 06:49:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AcountId] [int] NOT NULL,
	[AccountName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[AcountId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 13-02-2024 Feb 06:49:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[CurrencyId] [int] NOT NULL,
	[CurrencyCode] [varchar](20) NOT NULL,
	[CurrencyName] [varchar](50) NOT NULL,
	[CurrencySymbol] [nvarchar](50) NULL,
 CONSTRAINT [PK_CurrencyMaster] PRIMARY KEY CLUSTERED 
(
	[CurrencyId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CurrencyConversionRate]    Script Date: 13-02-2024 Feb 06:49:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CurrencyConversionRate](
	[BaseCurrencyId] [int] NOT NULL,
	[ToCurrencyId] [int] NOT NULL,
	[ConversionRate] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_CurrencyConversionRate] PRIMARY KEY CLUSTERED 
(
	[BaseCurrencyId] ASC,
	[ToCurrencyId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpensePayment]    Script Date: 13-02-2024 Feb 06:49:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpensePayment](
	[ExpensePaymentId] [bigint] IDENTITY(1,1) NOT NULL,
	[ExpenseSetupId] [int] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Year] [int] NULL,
	[Month] [int] NULL,
	[Day] [int] NULL,
	[PaymentDoneDate] [datetime] NULL,
	[ExpensePaymentStatus] [int] NOT NULL,
	[Descr] [nvarchar](max) NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[Amount] [decimal](18, 4) NULL,
	[PaymentStatus] [int] NULL,
 CONSTRAINT [PK_ExpensePayment] PRIMARY KEY CLUSTERED 
(
	[ExpensePaymentId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpenseSetup]    Script Date: 13-02-2024 Feb 06:49:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseSetup](
	[ExpenseSetupId] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[ExpenseName] [varchar](50) NOT NULL,
	[ExpenseDescr] [nvarchar](500) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Amount] [decimal](10, 4) NULL,
	[TaxAmount] [decimal](10, 4) NULL,
	[CurrencyId] [int] NULL,
	[BillGeneratedDay] [int] NULL,
	[BillPaymentDay] [int] NULL,
	[ExpirationDay] [int] NULL,
	[PaymentUrl] [nvarchar](500) NULL,
	[AccountProfileUrl] [nvarchar](500) NULL,
	[PrepaidOrPostpaid] [bit] NULL,
 CONSTRAINT [PK_ExpenseSetup] PRIMARY KEY CLUSTERED 
(
	[ExpenseSetupId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Account] ([AcountId], [AccountName]) VALUES (1, N'sukesh')
GO
INSERT [dbo].[Currency] ([CurrencyId], [CurrencyCode], [CurrencyName], [CurrencySymbol]) VALUES (1, N'INR', N'Indian rupee', N'₹')
GO
INSERT [dbo].[Currency] ([CurrencyId], [CurrencyCode], [CurrencyName], [CurrencySymbol]) VALUES (2, N'SEK', N'Swedish Krona', N'kr')
GO
INSERT [dbo].[Currency] ([CurrencyId], [CurrencyCode], [CurrencyName], [CurrencySymbol]) VALUES (3, N'USD', N'US Dollar', N'$')
GO
INSERT [dbo].[CurrencyConversionRate] ([BaseCurrencyId], [ToCurrencyId], [ConversionRate]) VALUES (2, 1, CAST(7.65 AS Decimal(18, 2)))
GO
INSERT [dbo].[CurrencyConversionRate] ([BaseCurrencyId], [ToCurrencyId], [ConversionRate]) VALUES (2, 3, CAST(0.09 AS Decimal(18, 2)))
GO
INSERT [dbo].[CurrencyConversionRate] ([BaseCurrencyId], [ToCurrencyId], [ConversionRate]) VALUES (3, 1, CAST(83.31 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[ExpensePayment] ON 
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (1, 1, 1, 2023, 12, NULL, CAST(N'2023-12-27T00:00:00.000' AS DateTime), 2, NULL, CAST(N'2023-12-27T08:16:53.793' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (3, 2, 1, 2023, 12, NULL, CAST(N'2023-12-28T00:27:34.683' AS DateTime), 2, NULL, CAST(N'2023-12-28T00:27:34.480' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (4, 3, 1, 2023, 12, NULL, CAST(N'2023-12-28T00:29:01.343' AS DateTime), 2, NULL, CAST(N'2023-12-28T00:29:01.343' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (5, 4, 1, 2023, 12, NULL, CAST(N'2023-12-28T00:29:52.290' AS DateTime), 2, NULL, CAST(N'2023-12-28T00:29:52.290' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (6, 15, 1, 2023, 12, NULL, CAST(N'2023-12-28T00:31:44.610' AS DateTime), 2, NULL, CAST(N'2023-12-28T00:31:44.610' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (7, 14, 1, 2023, 12, NULL, CAST(N'2023-12-28T00:34:16.237' AS DateTime), 2, NULL, CAST(N'2023-12-28T00:34:16.237' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (8, 1, 1, 2024, 1, NULL, CAST(N'2024-01-10T18:20:27.627' AS DateTime), 2, NULL, CAST(N'2024-01-02T16:14:11.187' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (9, 2, 1, 2024, 1, NULL, CAST(N'2024-01-11T09:24:39.450' AS DateTime), 2, NULL, CAST(N'2024-01-02T16:14:28.477' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (10, 3, 1, 2024, 1, NULL, CAST(N'2024-01-11T08:40:43.810' AS DateTime), 2, NULL, CAST(N'2024-01-03T22:57:36.007' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (11, 4, 1, 2024, 1, NULL, NULL, 1, NULL, CAST(N'2024-01-03T22:59:07.580' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (12, 8, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:01:59.640' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:01:59.640' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (13, 9, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:03:40.510' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:03:40.510' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (14, 16, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:03:52.683' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:03:52.683' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (15, 15, 1, 2024, 1, NULL, CAST(N'2024-01-08T13:42:48.397' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:05:25.887' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (16, 14, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:05:54.853' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:05:54.853' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (17, 17, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:09:20.590' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:09:20.590' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (18, 6, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:10:29.550' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:10:29.550' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (19, 7, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:10:35.517' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:10:35.517' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (20, 10, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:10:39.787' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:10:39.787' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (21, 13, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:10:48.613' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:10:48.613' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (22, 12, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:10:57.253' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:10:57.253' AS DateTime), NULL, NULL)
GO
INSERT [dbo].[ExpensePayment] ([ExpensePaymentId], [ExpenseSetupId], [AccountId], [Year], [Month], [Day], [PaymentDoneDate], [ExpensePaymentStatus], [Descr], [CreatedDateTime], [Amount], [PaymentStatus]) VALUES (23, 11, 1, 2024, 1, NULL, CAST(N'2024-01-03T23:18:25.133' AS DateTime), 2, NULL, CAST(N'2024-01-03T23:18:25.133' AS DateTime), NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ExpensePayment] OFF
GO
SET IDENTITY_INSERT [dbo].[ExpenseSetup] ON 
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (1, 1, N'LIC Life insurance', NULL, CAST(N'2017-03-27T00:00:00.000' AS DateTime), CAST(N'2029-03-27T00:00:00.000' AS DateTime), CAST(5151.0000 AS Decimal(10, 4)), CAST(115.8800 AS Decimal(10, 4)), 1, NULL, 27, 27, N'https://ebiz.licindia.in/D2CPM/#Login', NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (2, 1, N'Post life insurance', NULL, CAST(N'2022-03-23T00:00:00.000' AS DateTime), CAST(N'2028-03-23T00:00:00.000' AS DateTime), CAST(10500.0000 AS Decimal(10, 4)), NULL, 1, NULL, 1, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (3, 1, N'SBI Personal loan', NULL, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2030-02-02T00:00:00.000' AS DateTime), CAST(28000.0000 AS Decimal(10, 4)), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (4, 1, N'SC Personal loan', NULL, CAST(N'2023-06-01T00:00:00.000' AS DateTime), NULL, CAST(18305.0000 AS Decimal(10, 4)), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (6, 1, N'Room rent - VictoriaHem', NULL, CAST(N'2023-06-01T00:00:00.000' AS DateTime), NULL, CAST(9000.0000 AS Decimal(10, 4)), NULL, 2, NULL, 30, 30, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (7, 1, N'Internet Bitcomm Sweden', NULL, CAST(N'2023-06-01T00:00:00.000' AS DateTime), NULL, CAST(600.0000 AS Decimal(10, 4)), NULL, 2, NULL, 30, 30, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (8, 1, N'Bridge Chitty', NULL, CAST(N'2023-06-01T00:00:00.000' AS DateTime), NULL, CAST(18000.0000 AS Decimal(10, 4)), NULL, 1, NULL, 1, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (9, 1, N'Asianet Internet HP8498', NULL, CAST(N'2022-01-02T00:00:00.000' AS DateTime), NULL, CAST(1100.0000 AS Decimal(10, 4)), NULL, 1, 1, 15, 15, N'https://payments.asianet.co.in/Default.aspx', N'https://myabb.in/', NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (10, 1, N'Ellivio electricity Netowk bill', NULL, CAST(N'2022-01-02T00:00:00.000' AS DateTime), NULL, CAST(330.0000 AS Decimal(10, 4)), NULL, 2, NULL, 30, 30, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (11, 1, N'Bixia electricity bill', NULL, CAST(N'2022-01-02T00:00:00.000' AS DateTime), NULL, CAST(330.0000 AS Decimal(10, 4)), NULL, 2, NULL, 30, 30, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (12, 1, N'Swedish Union Insurance', NULL, CAST(N'2022-01-02T00:00:00.000' AS DateTime), NULL, CAST(270.0000 AS Decimal(10, 4)), NULL, 2, NULL, 30, 30, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (13, 1, N'Swedish Acasa Isurance', NULL, CAST(N'2022-01-02T00:00:00.000' AS DateTime), NULL, CAST(120.0000 AS Decimal(10, 4)), NULL, 2, NULL, 30, 30, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (14, 1, N'Postal lottery', NULL, CAST(N'2022-01-02T00:00:00.000' AS DateTime), NULL, CAST(180.0000 AS Decimal(10, 4)), NULL, 2, NULL, 30, 30, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (15, 1, N'Rishan Pre-School bill', NULL, CAST(N'2022-01-02T00:00:00.000' AS DateTime), NULL, CAST(1278.0000 AS Decimal(10, 4)), NULL, 2, NULL, 30, 30, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (16, 1, N'Google one subscription', NULL, CAST(N'2022-01-02T00:00:00.000' AS DateTime), NULL, CAST(9.3000 AS Decimal(10, 4)), NULL, 3, NULL, 1, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[ExpenseSetup] ([ExpenseSetupId], [AccountId], [ExpenseName], [ExpenseDescr], [StartDate], [EndDate], [Amount], [TaxAmount], [CurrencyId], [BillGeneratedDay], [BillPaymentDay], [ExpirationDay], [PaymentUrl], [AccountProfileUrl], [PrepaidOrPostpaid]) VALUES (17, 1, N'Syamala Expense', NULL, NULL, NULL, CAST(30000.0000 AS Decimal(10, 4)), NULL, 1, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ExpenseSetup] OFF
GO
ALTER TABLE [dbo].[ExpensePayment] ADD  CONSTRAINT [DF_ExpensePayment_CreatedDateTime]  DEFAULT (getutcdate()) FOR [CreatedDateTime]
GO
ALTER TABLE [dbo].[CurrencyConversionRate]  WITH CHECK ADD  CONSTRAINT [FK_CurrencyConversionRate_Currency_BaseCurrencyId] FOREIGN KEY([BaseCurrencyId])
REFERENCES [dbo].[Currency] ([CurrencyId])
GO
ALTER TABLE [dbo].[CurrencyConversionRate] CHECK CONSTRAINT [FK_CurrencyConversionRate_Currency_BaseCurrencyId]
GO
ALTER TABLE [dbo].[CurrencyConversionRate]  WITH CHECK ADD  CONSTRAINT [FK_CurrencyConversionRate_Currency_ToCurrencyId] FOREIGN KEY([ToCurrencyId])
REFERENCES [dbo].[Currency] ([CurrencyId])
GO
ALTER TABLE [dbo].[CurrencyConversionRate] CHECK CONSTRAINT [FK_CurrencyConversionRate_Currency_ToCurrencyId]
GO
ALTER TABLE [dbo].[ExpenseSetup]  WITH CHECK ADD  CONSTRAINT [FK_ExpenseSetup_Account_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([AcountId])
GO
ALTER TABLE [dbo].[ExpenseSetup] CHECK CONSTRAINT [FK_ExpenseSetup_Account_AccountId]
GO
ALTER TABLE [dbo].[ExpenseSetup]  WITH CHECK ADD  CONSTRAINT [FK_ExpenseSetup_Currency_CurrencyId] FOREIGN KEY([CurrencyId])
REFERENCES [dbo].[Currency] ([CurrencyId])
GO
ALTER TABLE [dbo].[ExpenseSetup] CHECK CONSTRAINT [FK_ExpenseSetup_Currency_CurrencyId]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prepaid = 0, Postpaid = 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ExpenseSetup', @level2type=N'COLUMN',@level2name=N'PrepaidOrPostpaid'
GO
