USE [master]
GO
/****** Object:  Database [Books]    Script Date: 12/8/2018 1:44:37 PM ******/
CREATE DATABASE [Books]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Books', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQL2016D\MSSQL\DATA\Books.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Books_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL13.SQL2016D\MSSQL\DATA\Books_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Books] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Books].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Books] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Books] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Books] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Books] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Books] SET ARITHABORT OFF 
GO
ALTER DATABASE [Books] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Books] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Books] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Books] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Books] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Books] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Books] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Books] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Books] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Books] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Books] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Books] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Books] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Books] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Books] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Books] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Books] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Books] SET RECOVERY FULL 
GO
ALTER DATABASE [Books] SET  MULTI_USER 
GO
ALTER DATABASE [Books] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Books] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Books] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Books] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Books] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Books', N'ON'
GO
ALTER DATABASE [Books] SET QUERY_STORE = OFF
GO
USE [Books]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [Books]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[MetaData]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetaData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DeviceId] [varchar](40) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Payload] [nvarchar](max) NULL,
 CONSTRAINT [PK_Metadata] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[DeleteBookById]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--CREATE PROCEDURE GetAllBooks
--AS
--SELECT * FROM Book order by DateOfPublication desc
--GO



--CREATE PROCEDURE GetBookById
--@BookId varchar(40)
--AS
--SELECT top 1 * From BOOK Where BookId=@BookId 
--GO

CREATE PROCEDURE [dbo].[DeleteBookById]
@BookId varchar(40)
AS
Delete From BOOK Where BookId=@BookId 
GO
/****** Object:  StoredProcedure [dbo].[GetAllBooks]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllBooks]
AS
SELECT * FROM Book order by DateOfPublication desc
GO
/****** Object:  StoredProcedure [dbo].[GetAllMetaData]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[GetAllMetaData]
as
begin
select * from MetaData
end
GO
/****** Object:  StoredProcedure [dbo].[GetBookById]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetBookById]
@BookId varchar(40)
AS
SELECT top 1 * From BOOK Where BookId=@BookId 
GO
/****** Object:  StoredProcedure [dbo].[InsertBook]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertBook]
@BookId varchar(40),
@Name varchar(250),
@NumberOfPages int,
@DateOfPublication datetime,
@Authors nvarchar(500),
@CreatedDate datetime,
@UpdatedDate datetime

AS
Insert into Book(BookId,Name,NumberOfPages,DateOfPublication,Authors,CreatedDate,UpdatedDate)
 values(@BookId,@Name,@NumberOfPages,@DateOfPublication,@Authors,@CreatedDate,@UpdatedDate)

GO
/****** Object:  StoredProcedure [dbo].[InsertMetaData]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[InsertMetaData]
@DeviceId varchar(40),
@TimeStamp Datetime,
@Payload Nvarchar(max) null

AS
Insert into MetaData(DeviceId,TimeStamp,payload)
 values(@DeviceId,@TimeStamp,@Payload)

GO
/****** Object:  StoredProcedure [dbo].[UpdateBook]    Script Date: 12/8/2018 1:44:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateBook]
@BookId varchar(40),
@Name varchar(250),
@NumberOfPages int,
@DateOfPublication datetime,
@Authors nvarchar(500),
@CreatedDate datetime,
@UpdatedDate datetime
AS

UPDATE Book 
Set 

Book.Name=@Name,
NumberOfPages=@NumberOfPages,
DateOfPublication=@DateOfPublication,
@Authors=@Authors,
@CreatedDate=@CreatedDate,
@UpdatedDate=@UpdatedDate

Where BookId=@BookId


GO
USE [master]
GO
ALTER DATABASE [Books] SET  READ_WRITE 
GO
