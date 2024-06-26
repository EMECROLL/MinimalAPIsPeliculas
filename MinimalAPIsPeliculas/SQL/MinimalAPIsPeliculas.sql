USE [master]
GO
/****** Object:  Database [MinimalAPIsPeliculas]    Script Date: 29/05/2024 12:01:59 p. m. ******/
CREATE DATABASE [MinimalAPIsPeliculas]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MinimalAPIsPeliculas', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MinimalAPIsPeliculas.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MinimalAPIsPeliculas_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\MinimalAPIsPeliculas_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MinimalAPIsPeliculas].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET ARITHABORT OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET  MULTI_USER 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET QUERY_STORE = ON
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MinimalAPIsPeliculas]
GO
/****** Object:  UserDefinedTableType [dbo].[ListadoActores]    Script Date: 29/05/2024 12:01:59 p. m. ******/
CREATE TYPE [dbo].[ListadoActores] AS TABLE(
	[ActorId] [int] NULL,
	[Personaje] [nvarchar](150) NULL,
	[Orden] [int] NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[ListadoEnteros]    Script Date: 29/05/2024 12:01:59 p. m. ******/
CREATE TYPE [dbo].[ListadoEnteros] AS TABLE(
	[Id] [int] NULL
)
GO
/****** Object:  Table [dbo].[Actores]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actores](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[FechaNacimiento] [datetime2](7) NOT NULL,
	[Foto] [nvarchar](max) NULL,
 CONSTRAINT [PK_Actores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActoresPeliculas]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActoresPeliculas](
	[ActorId] [int] NOT NULL,
	[PeliculaId] [int] NOT NULL,
	[Orden] [int] NOT NULL,
	[Personaje] [nvarchar](150) NOT NULL,
 CONSTRAINT [PK_ActoresPeliculas] PRIMARY KEY CLUSTERED 
(
	[ActorId] ASC,
	[PeliculaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comentarios]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comentarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cuerpo] [nvarchar](max) NOT NULL,
	[PeliculaId] [int] NOT NULL,
	[UsuarioId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Comentarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Errores]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Errores](
	[Id] [uniqueidentifier] NOT NULL,
	[MensajeDeError] [nvarchar](max) NOT NULL,
	[StackTrace] [nvarchar](max) NULL,
	[Fecha] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Errores] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Generos]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Generos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Generos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GenerosPeliculas]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GenerosPeliculas](
	[PeliculaId] [int] NOT NULL,
	[GeneroId] [int] NOT NULL,
 CONSTRAINT [PK_GenerosPeliculas] PRIMARY KEY CLUSTERED 
(
	[PeliculaId] ASC,
	[GeneroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Peliculas]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Peliculas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [nvarchar](150) NOT NULL,
	[EnCines] [bit] NOT NULL,
	[FechaLanzamiento] [datetime2](7) NOT NULL,
	[Poster] [nvarchar](max) NULL,
 CONSTRAINT [PK_Peliculas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolesClaims]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolesClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_RolesClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuariosClaims]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuariosClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_UsuariosClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuariosLogins]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuariosLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UsuariosLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuariosRoles]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuariosRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UsuariosRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuariosTokens]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsuariosTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_UsuariosTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Errores] ADD  CONSTRAINT [DF_Errores_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_EmailConfirmed]  DEFAULT ('false') FOR [EmailConfirmed]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_PhoneNumberConfirmed]  DEFAULT ('false') FOR [PhoneNumberConfirmed]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_TwoFactorEnabled]  DEFAULT ('false') FOR [TwoFactorEnabled]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_LockoutEnabled]  DEFAULT ('false') FOR [LockoutEnabled]
GO
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [DF_Usuarios_AccessFailedCount]  DEFAULT ((0)) FOR [AccessFailedCount]
GO
ALTER TABLE [dbo].[ActoresPeliculas]  WITH CHECK ADD  CONSTRAINT [FK_ActoresPeliculas_Actores] FOREIGN KEY([ActorId])
REFERENCES [dbo].[Actores] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActoresPeliculas] CHECK CONSTRAINT [FK_ActoresPeliculas_Actores]
GO
ALTER TABLE [dbo].[ActoresPeliculas]  WITH CHECK ADD  CONSTRAINT [FK_ActoresPeliculas_Peliculas] FOREIGN KEY([PeliculaId])
REFERENCES [dbo].[Peliculas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActoresPeliculas] CHECK CONSTRAINT [FK_ActoresPeliculas_Peliculas]
GO
ALTER TABLE [dbo].[Comentarios]  WITH CHECK ADD  CONSTRAINT [FK_Comentarios_Comentarios] FOREIGN KEY([Id])
REFERENCES [dbo].[Comentarios] ([Id])
GO
ALTER TABLE [dbo].[Comentarios] CHECK CONSTRAINT [FK_Comentarios_Comentarios]
GO
ALTER TABLE [dbo].[Comentarios]  WITH CHECK ADD  CONSTRAINT [FK_Comentarios_Peliculas] FOREIGN KEY([PeliculaId])
REFERENCES [dbo].[Peliculas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comentarios] CHECK CONSTRAINT [FK_Comentarios_Peliculas]
GO
ALTER TABLE [dbo].[Comentarios]  WITH CHECK ADD  CONSTRAINT [FK_Comentarios_Usuarios] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comentarios] CHECK CONSTRAINT [FK_Comentarios_Usuarios]
GO
ALTER TABLE [dbo].[GenerosPeliculas]  WITH CHECK ADD  CONSTRAINT [FK_GenerosPeliculas_Generos] FOREIGN KEY([GeneroId])
REFERENCES [dbo].[Generos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GenerosPeliculas] CHECK CONSTRAINT [FK_GenerosPeliculas_Generos]
GO
ALTER TABLE [dbo].[GenerosPeliculas]  WITH CHECK ADD  CONSTRAINT [FK_GenerosPeliculas_Peliculas] FOREIGN KEY([PeliculaId])
REFERENCES [dbo].[Peliculas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GenerosPeliculas] CHECK CONSTRAINT [FK_GenerosPeliculas_Peliculas]
GO
ALTER TABLE [dbo].[RolesClaims]  WITH CHECK ADD  CONSTRAINT [FK_RolesClaims_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RolesClaims] CHECK CONSTRAINT [FK_RolesClaims_Roles_RoleId]
GO
ALTER TABLE [dbo].[UsuariosClaims]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosClaims_Usuarios_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuariosClaims] CHECK CONSTRAINT [FK_UsuariosClaims_Usuarios_UserId]
GO
ALTER TABLE [dbo].[UsuariosLogins]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosLogins_Usuarios_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuariosLogins] CHECK CONSTRAINT [FK_UsuariosLogins_Usuarios_UserId]
GO
ALTER TABLE [dbo].[UsuariosRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosRoles_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuariosRoles] CHECK CONSTRAINT [FK_UsuariosRoles_Roles_RoleId]
GO
ALTER TABLE [dbo].[UsuariosRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosRoles_Usuarios_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuariosRoles] CHECK CONSTRAINT [FK_UsuariosRoles_Usuarios_UserId]
GO
ALTER TABLE [dbo].[UsuariosTokens]  WITH CHECK ADD  CONSTRAINT [FK_UsuariosTokens_Usuarios_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuariosTokens] CHECK CONSTRAINT [FK_UsuariosTokens_Usuarios_UserId]
GO
/****** Object:  StoredProcedure [dbo].[Actores_Actualizar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_Actualizar]
	-- Add the parameters for the stored procedure here
	@Id int,
	@Nombre nvarchar(150),
	@FechaNacimiento datetime2,
	@Foto nvarchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Actores
	SET Nombre = @Nombre, FechaNacimiento = @FechaNacimiento, Foto = @Foto
	WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[Actores_Borrar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_Borrar] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE Actores
	WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[Actores_Cantidad]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_Cantidad] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT (*)
	FROM Actores;
END
GO
/****** Object:  StoredProcedure [dbo].[Actores_Crear]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_Crear]
	-- Add the parameters for the stored procedure here
	@Nombre nvarchar(150),
	@FechaNacimiento datetime2,
	@Foto nvarchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Actores (Nombre, FechaNacimiento, Foto)
	VALUES (@Nombre, @FechaNacimiento, @Foto);

	SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Actores_ExistePorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_ExistePorId] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF EXISTS (SELECT 1 FROM Actores WHERE Id = @Id)
		SELECT 1
	ELSE
		SELECT 0
END
GO
/****** Object:  StoredProcedure [dbo].[Actores_ObtenerPorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_ObtenerPorId]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Actores
	Where Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Actores_ObtenerPorNombre]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_ObtenerPorNombre] 
	-- Add the parameters for the stored procedure here
	@Nombre nvarchar(150)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM ACTORES
	WHERE Nombre LIKE '%' + @Nombre +'%';
END
GO
/****** Object:  StoredProcedure [dbo].[Actores_ObtenerTodos]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_ObtenerTodos] 
	-- Add the parameters for the stored procedure here
	@pagina int,
	@recordsPorPagina int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Actores
	ORDER BY Nombre
	OFFSET ((@pagina - 1) * @recordsPorPagina) ROWS FETCH NEXT @recordsPorPagina ROWS ONLY
END
GO
/****** Object:  StoredProcedure [dbo].[Actores_ObtenerVariosPorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Actores_ObtenerVariosPorId] 
	-- Add the parameters for the stored procedure here
	@actoresIds ListadoEnteros READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id
	FROM Actores
	WHERE Id IN (SELECT Id FROM @actoresIds);
END
GO
/****** Object:  StoredProcedure [dbo].[Comentarios_Actualizar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Comentarios_Actualizar] 
	-- Add the parameters for the stored procedure here
	@Cuerpo nvarchar(MAX),
	@PeliculaId int, 
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Comentarios
	SET Cuerpo = @Cuerpo, PeliculaId = @PeliculaId
	WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[Comentarios_Borrar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Comentarios_Borrar] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE Comentarios WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[Comentarios_Crear]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Comentarios_Crear] 
	-- Add the parameters for the stored procedure here
	@Cuerpo nvarchar(MAX),
	@PeliculaId int,
	@UsuarioId nvarchar(450)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Comentarios (Cuerpo, PeliculaId, UsuarioId)
	VALUES (@Cuerpo, @PeliculaId, @UsuarioId);

	SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Comentarios_ExistePorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Comentarios_ExistePorId] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF EXISTS (SELECT 1 FROM Comentarios WHERE Id = @Id)
		SELECT 1
	ELSE
		SELECT 0
END
GO
/****** Object:  StoredProcedure [dbo].[Comentarios_ObtenerPorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Comentarios_ObtenerPorId] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Comentarios
	WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[Comentarios_ObtenerTodos]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Comentarios_ObtenerTodos]
	-- Add the parameters for the stored procedure here
	@PeliculaId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Comentarios
	WHERE PeliculaId = @PeliculaId;
END
GO
/****** Object:  StoredProcedure [dbo].[Errores_Crear]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Errores_Crear] 
	-- Add the parameters for the stored procedure here
	@MensajeDeError nvarchar(max),
	@StackTrace nvarchar(max),
	@Fecha datetime2
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Errores (MensajeDeError, StackTrace, Fecha)
	VALUES (@MensajeDeError, @StackTrace, @Fecha);
END
GO
/****** Object:  StoredProcedure [dbo].[Generos_Actualizar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generos_Actualizar]
	-- Add the parameters for the stored procedure here
	@Id int,
	@Nombre nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    UPDATE Generos
    SET Nombre = @Nombre
    WHERE Id = @Id;                
END
GO
/****** Object:  StoredProcedure [dbo].[Generos_Borrar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generos_Borrar]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE Generos WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Generos_Crear]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generos_Crear] 
	-- Add the parameters for the stored procedure here
	@Nombre nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Generos (Nombre) VALUES (@Nombre);

	SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Generos_ExistePorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generos_ExistePorId]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF EXISTS (SELECT 1 FROM Generos WHERE Id = @Id)
		SELECT 1
	ELSE 
		SELECT 0
END
GO
/****** Object:  StoredProcedure [dbo].[Generos_ExistePorIdYNombre]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generos_ExistePorIdYNombre] 
	-- Add the parameters for the stored procedure here
	@Id int,
	@Nombre nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF EXISTS (SELECT 1 FROM Generos WHERE Id <> @Id AND Nombre = @Nombre)
		SELECT 1;
	ELSE
		SELECT 0;
END
GO
/****** Object:  StoredProcedure [dbo].[Generos_ObtenerPorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generos_ObtenerPorId]
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id, Nombre FROM Generos
	WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Generos_ObtenerTodos]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generos_ObtenerTodos] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id, Nombre FROM Generos ORDER BY Nombre;
END
GO
/****** Object:  StoredProcedure [dbo].[Generos_ObtenerVariosPorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Generos_ObtenerVariosPorId] 
	-- Add the parameters for the stored procedure here
	@generosIds ListadoEnteros READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id FROM Generos
	WHERE Id IN (SELECT Id FROM @generosIds);
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_Actualizar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_Actualizar] 
	-- Add the parameters for the stored procedure here
	@Titulo nvarchar(150),
	@EnCines bit,
	@FechaLanzamiento datetime2,
	@Poster nvarchar(max),
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Peliculas
	SET Titulo = @Titulo, FechaLanzamiento = @FechaLanzamiento, EnCines = @EnCines, Poster = @Poster
	WHERE Id = @Id;
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_AsignarActores]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_AsignarActores] 
	-- Add the parameters for the stored procedure here
	@PeliculaId int,
	@actores ListadoActores READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO ActoresPeliculas (ActorId, PeliculaId, Orden, Personaje)
	SELECT ActorId, @PeliculaId, Orden, Personaje FROM @actores;
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_AsignarGeneros]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_AsignarGeneros] 
	-- Add the parameters for the stored procedure here
	@peliculaId int,
	@generosIds ListadoEnteros READONLY -- Se coloca READONLY cuando utilizamos un parámetro que es una tabla
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE GenerosPeliculas WHERE PeliculaId = @peliculaId;

	INSERT INTO GenerosPeliculas (GeneroId, PeliculaId)
	SELECT Id, @peliculaId FROM @generosIds
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_Borrar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_Borrar] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE Peliculas WHERE Id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_Cantidad]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_Cantidad] 
	-- Add the parameters for the stored procedure here
	@titulo nvarchar(150) = '',
	@generoId int = 0,
	@proximosEstrenos bit = 'False',
	@enCines bit = 'False'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT (*)
	FROM Peliculas
	WHERE (Titulo LIKE '%' + @titulo + '%' OR @titulo = '')
	AND (FechaLanzamiento > GETDATE() OR @proximosEstrenos = 'False')
	AND (EnCines = 'True' OR @enCines = 'False')
	AND (Id IN (SELECT PeliculaId FROM GenerosPeliculas WHERE GeneroId = @generoId) OR @generoId = 0)
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_Crear]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_Crear] 
	-- Add the parameters for the stored procedure here
	@Titulo nvarchar(150),
	@EnCines bit,
	@FechaLanzamiento datetime2,
	@Poster nvarchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Peliculas (Titulo, EnCines, FechaLanzamiento, Poster)
	VALUES (@Titulo, @EnCines, @FechaLanzamiento, @Poster);

	SELECT SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_ExistePorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_ExistePorId] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF EXISTS (SELECT 1 FROM Peliculas WHERE Id = @Id)
		SELECT 1
	ELSE
		SELECT 0
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_Filtrar]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_Filtrar] 
	-- Add the parameters for the stored procedure here
	@pagina int,
	@recordsPorPagina int,
	@titulo nvarchar(150),
	@generoId int,
	@proximosEstrenos bit,
	@enCines bit,
	@campoOrdenar nvarchar(150),
	@ordenAscendente bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Peliculas
	WHERE (Titulo LIKE '%' + @titulo + '%' OR @titulo = '')
	AND (FechaLanzamiento > GETDATE() OR @proximosEstrenos = 'False')
	AND (EnCines = 'True' OR @enCines = 'False')
	AND (Id IN (SELECT PeliculaId FROM GenerosPeliculas WHERE GeneroId = @generoId) OR @generoId = 0)
	ORDER BY 
		CASE
			WHEN @campoOrdenar = 'Titulo' AND @ordenAscendente = 'True' THEN Titulo END ASC,
		CASE
			WHEN @campoOrdenar = 'Titulo' AND @ordenAscendente = 'False' THEN Titulo END DESC,
		CASE
			WHEN @campoOrdenar = 'FechaLanzamiento' AND @ordenAscendente = 'True' THEN FechaLanzamiento END ASC,
		CASE
			WHEN @campoOrdenar = 'FechaLanzamiento' AND @ordenAscendente = 'False' THEN FechaLanzamiento END DESC
	OFFSET ((@pagina-1)*@recordsPorPagina) ROWS FETCH NEXT @recordsPorPagina ROWS ONLY
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_ObtenerPorId]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_ObtenerPorId] 
	-- Add the parameters for the stored procedure here
	@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Peliculas
	WHERE Id = @Id;

	SELECT * FROM Comentarios
	WHERE PeliculaId = @Id;

	SELECT Id, Nombre 
	FROM Generos
	INNER JOIN GenerosPeliculas
	ON GenerosPeliculas.GeneroId = Id
	WHERE PeliculaId = @Id;

	SELECT Id, Nombre, Personaje
	FROM Actores
	INNER JOIN ActoresPeliculas
	ON ActoresPeliculas.ActorId = Id
	WHERE PeliculaId = @Id
	ORDER BY Orden
END
GO
/****** Object:  StoredProcedure [dbo].[Peliculas_ObtenerTodos]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Peliculas_ObtenerTodos] 
	-- Add the parameters for the stored procedure here
	@Pagina int,
	@RecordsPorPagina int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Peliculas
	ORDER BY Titulo
	OFFSET ((@Pagina - 1) * @RecordsPorPagina) ROWS FETCH NEXT @RecordsPorPagina ROWS ONLY
END
GO
/****** Object:  StoredProcedure [dbo].[Usuarios_BuscarPorEmail]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Usuarios_BuscarPorEmail] 
	-- Add the parameters for the stored procedure here
	@normalizedEmail nvarchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Usuarios
	WHERE NormalizedEmail = @normalizedEmail;
END
GO
/****** Object:  StoredProcedure [dbo].[Usuarios_Crear]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Usuarios_Crear] 
	-- Add the parameters for the stored procedure here
	@Id nvarchar(450),
	@Email nvarchar(256),
	@NormalizedEmail nvarchar(256),
	@UserName nvarchar(256),
	@NormalizedUserName nvarchar(256),
	@PasswordHash nvarchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Usuarios (Id, Email, NormalizedEmail, UserName, NormalizedUserName, PasswordHash)
	VALUES (@Id, @Email, @NormalizedEmail, @UserName, @NormalizedUserName, @PasswordHash);
END
GO
/****** Object:  StoredProcedure [dbo].[Usuarios_ObtenerClaims]    Script Date: 29/05/2024 12:01:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Usuarios_ObtenerClaims] 
	-- Add the parameters for the stored procedure here
	@Id nvarchar(450)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ClaimType as [Type], ClaimValue as [Value] FROM UsuariosClaims
	WHERE UserId = @Id
END
GO
USE [master]
GO
ALTER DATABASE [MinimalAPIsPeliculas] SET  READ_WRITE 
GO
