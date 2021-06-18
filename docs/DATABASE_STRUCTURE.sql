CREATE DATABASE [Reservations]
GO
USE [Reservations]
GO
/****** Object:  Table [dbo].[OutboxMessage]    Script Date: 18/06/2021 11:15:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutboxMessage](
	[Application] [varchar](50) NOT NULL,
	[Event] [varchar](50) NOT NULL,
	[CorrelationId] [uniqueidentifier] NOT NULL,
	[Payload] [varchar](max) NOT NULL,
	[State] [smallint] NOT NULL,
	[EmitedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[Id] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleReservation]    Script Date: 18/06/2021 11:15:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleReservation](
	[ReserveId] [uniqueidentifier] NOT NULL,
	[VehicleId] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[ReservedAt] [datetime2](7) NOT NULL,
	[ReservationExpiresOn] [datetime2](7) NOT NULL,
	[Value] [decimal](18, 2) NOT NULL,
	[Status] [smallint] NOT NULL,
 CONSTRAINT [PK_VehicleReservation] PRIMARY KEY CLUSTERED 
(
	[ReserveId] ASC,
	[VehicleId] ASC,
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OutboxMessage] ADD  DEFAULT (newid()) FOR [Id]
GO
