/* 
 * Скрипт для создания таблицы links: сокращение ссылок
 */

USE [short]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[links]
(
  [id] [int] IDENTITY(1,1) NOT NULL,   -- идентификатор
  [moment] [smalldatetime] NULL,       -- момент создания ссылки
  [fulllink] [nvarchar](250) NOT NULL, -- полная версия ссылки
  [shortlink] [nvarchar](50) NOT NULL, -- краткая версия ссылки
  [description] [ntext] NULL,          -- описание в свободной форме
  [counter] [int] NOT NULL,            -- счетчик показа

  CONSTRAINT [PK_links] PRIMARY KEY CLUSTERED 
  (
     [id] ASC
  )
  ON [PRIMARY]
) 
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[links] ADD  CONSTRAINT [DF_links_counter] DEFAULT ((0)) FOR [counter]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_links_fullink] ON [dbo].[links]
(
  [fulllink] ASC
) 
ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_links_shortlink] ON [dbo].[links]
(
  [shortlink] ASC
)
ON [PRIMARY]
GO
