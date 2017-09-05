CREATE TABLE [dbo].[Cupido](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](50) NOT NULL,
	[ChannelId] [varchar](50) NOT NULL,
	[Gender] [varchar](50) NOT NULL,
	[Age] [int] NOT NULL,
	[PicUrl] [varchar](1000) NOT NULL,
	[NormalPoints] [int] NOT NULL,
	[SuperModelPoints] [int] NOT NULL,
	[HairBald] [bit] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Updated] [datetime] NOT NULL,
	[IsValid] [bit] NOT NULL,
 CONSTRAINT [PK_Cupido] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)
GO

ALTER TABLE [dbo].[Cupido] ADD  CONSTRAINT [DF_Cupido_HairBald]  DEFAULT ((0)) FOR [HairBald]
GO

ALTER TABLE [dbo].[Cupido] ADD  CONSTRAINT [DF_Cupido_IsValid]  DEFAULT ((1)) FOR [IsValid]
GO
