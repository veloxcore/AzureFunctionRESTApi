CREATE TABLE [dbo].[Book](
	[BookId] [varchar](40) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[NumberOfPages] [int] NOT NULL,
	[DateOfPublication] [datetime] NOT NULL,
	[Authors] [nvarchar](500) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]