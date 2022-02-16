CREATE TABLE [dbo].[books]
 (
    [id]       [int]          IDENTITY(1,1) NOT NULL,

    [catalog]  [varchar](50)                NOT NULL,
    [number]   [nvarchar](50)               NULL,
    [card]     [nvarchar](50)               NULL,
    [place]    [nvarchar](50)               NOT NULL,
    [barcode]  [varchar](50)                NULL,
    [rfid]     [varchar](50)                NULL,

    [ticket]   [nvarchar](50)               NULL,
    [moment]   [smalldatetime]              NULL,
    [deadline] [smalldatetime]              NULL,
    [operator] [int]                        NULL,

    [prolong]  [int]                        NULL,
    [alert]    [nvarchar](50)               NULL,
    [pilot]    [bit]                        NOT NULL,
    [price]    [money]                      NULL,
    [seen]     [smalldatetime]              NULL,
    [seenby]   [int]                        NULL,

    CONSTRAINT [PK_books] PRIMARY KEY CLUSTERED ([id] ASC),

    CONSTRAINT [IX_books] UNIQUE NONCLUSTERED
    (
        [catalog] ASC,
        [number] ASC,
        [card] ASC,
        [place] ASC,
        [barcode] ASC,
        [rfid] ASC
    )

)

GO

