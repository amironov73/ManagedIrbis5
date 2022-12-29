// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* GateModel.cs -- модель данных пропускного пункта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;
using ManagedIrbis.Readers.Formatting;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace Gatekeeper;

/// <summary>
/// Модель данных пропускного пункта.
/// </summary>
internal sealed class GateModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Направление: читатели входят (in) или выходят (out).
    /// </summary>
    [Reactive]
    public string? Direction { get; set; }

    /// <summary>
    /// Наименование библиотеки.
    /// </summary>
    [Reactive]
    public string? Title { get; set; }

    /// <summary>
    /// Посещений за сегодня.
    /// </summary>
    [Reactive]
    public string? Today { get; set; }

    /// <summary>
    /// Читателей в библиотеке.
    /// </summary>
    [Reactive]
    public string? Readers { get; set; }

    /// <summary>
    /// События.
    /// </summary>
    [Reactive]
    public ObservableCollection<EventModel>? Events { get; set; }

    /// <summary>
    /// Посещений за сегодня.
    /// </summary>
    [Reactive]
    public int VisitCount { get; set; }

    /// <summary>
    /// Количество читателей в библиотеке.
    /// </summary>
    [Reactive]
    public int InsiderCount { get; set; }

    /// <summary>
    /// Обращение к охранникам.
    /// </summary>
    [Reactive]
    public string? Message { get; set; }

    /// <summary>
    /// Последний посетитель.
    /// </summary>
    [Reactive]
    public string? Last { get; set; }

    /// <summary>
    /// Штрих-код приходящего/выходящего читателя.
    /// </summary>
    [Reactive]
    public string? Barcode { get; set; }

    /// <summary>
    /// Признак ошибки.
    /// </summary>
    [Reactive]
    public bool IsError { get; set; }

    /// <summary>
    /// Признак информационного сообщения.
    /// </summary>
    [Reactive]
    public bool IsInfo { get; set; }

    #endregion

    #region Public methods

    public async Task<IAsyncConnection> CreateConnection()
    {
        var connectionString = Magna.Configuration["connection-string"];
        if (string.IsNullOrEmpty (connectionString))
        {
            throw new Exception();
        }

        var result = ConnectionFactory.Shared.CreateAsyncConnection();
        result.ParseConnectionString (connectionString);
        await result.ConnectAsync();
        if (!result.IsConnected)
        {
            throw new Exception();
        }

        return result;
    }


    public static GateModel GetTestModel()
    {
        var result = new GateModel
        {
            Message = "Смотрите, кто пришел",
            Last = "Пока никто не приходил",
            Events = new ObservableCollection<EventModel> (EventModel.GetTestEvents)
        };

        return result;
    }

    public void ShowHtml
        (
            string? text,
            bool error = false,
            bool info = false
        )
    {
        IsError = error;
        IsInfo = info;
        text = HtmlText.ToPlainText (text).SafeTrim();
        if (!string.IsNullOrEmpty (text))
        {
            text = Regex.Replace
                (
                    text,
                    @"\n{3,}",
                    "\n\n"
                );
        }

        Last = text;
    }

    public async Task AutoUpdate()
    {
        await using var connection = await CreateConnection();
        await UpdateStatistics (connection);
    }

    /// <summary>
    /// Читатель вошел.
    /// </summary>
    public async Task RegisterIn
        (
            IAsyncConnection connection,
            Record record,
            string today,
            string now
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (record);
        connection.EnsureConnected();

        record.SetValue (999, "1");
        var v40 = new Field (40)
            .Add ('1', now)
            .Add ('c', "(Посещение)")
            .Add ('d', today)
            .Add ('i', string.Empty)
            .Add ('v', "*");
        record.Add (v40);

        await connection.WriteRecordAsync (record, actualize: false, lockRecord: true, dontParse: true);
    }


    /// <summary>
    /// Читатель вышел.
    /// </summary>
    public async Task RegisterOut
        (
            IAsyncConnection connection,
            Record record,
            string today,
            string now
        )
    {
        var modified = false;
        if (record.HaveField (999))
        {
            record.RemoveField (999);
            modified = true;
        }

        var v40 = record.Fields
            .GetField (40)
            .GetField ('v', "*")
            .GetField ('d', today)
            .LastOrDefault
                (
                    field => field.HaveSubField ('1')
                             && field.HaveNotSubField ('a')
                             && field.HaveNotSubField ('2')
                );
        if (v40 is not null)
        {
            v40
                .SetSubFieldValue ('2', now)
                .SetSubFieldValue ('f', today);
            modified = true;
        }

        if (modified)
        {
            await connection.WriteRecordAsync (record, actualize: false, lockRecord: true, dontParse: true);
        }
    }

    public async Task UpdateStatistics
        (
            IAsyncConnection connection
        )
    {
        var today = IrbisDate.TodayText;
        var term = $"VS={today}/*";
        var postingsParameters = new PostingParameters
        {
            Database = connection.EnsureDatabase(),
            Terms = new[] { term }
        };
        var postings = await connection.ReadPostingsAsync (postingsParameters);
        VisitCount = postings?.Length ?? 0;

        var expression = "VIS=$";
        var searchParameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = expression
        };
        var readers = await connection.SearchAsync (searchParameters);
        InsiderCount = readers?.Length ?? 0;
    }

    public async Task HandleReader
        (
            string? readerId
        )
    {
        ShowHtml (null);
        if (string.IsNullOrWhiteSpace (readerId))
        {
            return;
        }

        readerId = readerId.Trim();
        await using var connection = await CreateConnection();
        var searchParameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = $"\"RI={readerId}\""
        };
        var found = await connection.SearchAsync (searchParameters);
        if (found?.Length != 1)
        {
            ShowHtml ("Читатель не найден", error: true);
            return;
        }

        var recordParameters = new ReadRecordParameters
        {
            Database = connection.EnsureDatabase(),
            Mfn = found[0].Mfn
        };
        var reader = await connection.ReadRecordAsync (recordParameters);
        if (reader is null)
        {
            return;
        }

        var formatName = Magna.Configuration["format"];
        if (string.IsNullOrEmpty (formatName))
        {
            return;
        }

        var formatParameters = new FormatRecordParameters
        {
            Database = connection.EnsureDatabase(),
            Format = formatName,
            Mfns = new[] { found[0].Mfn }
        };
        await connection.FormatRecordsAsync (formatParameters);
        var html = formatParameters.Result.AsSingle();
        ShowHtml (html);

        var today = IrbisDate.TodayText;
        var now = IrbisDate.NowText;
        if (Direction.SameString ("in"))
        {
            await RegisterIn (connection, reader, today, now);
        }
        else
        {
            await RegisterOut (connection, reader, today, now);
        }

        var formatter = new AsyncHardReaderFormat (Magna.Host, connection);
        var name = formatter.FullNameWithYear (reader);
        var lastEvent = new EventModel
        {
            Moment = DateTime.Now.ToString ("HH:mm:ss"),
            Action = "вошел",
            Name = name,
            Ticket = formatter.Ticket (reader)
        };
        Events ??= new ();
        Events.Insert (0, lastEvent);

        await UpdateStatistics (connection);
    }

    /// <summary>
    /// Получение модели из конфигурации
    /// </summary>
    /// <returns></returns>
    public static GateModel FromConfiguration()
    {
        var configuration = Magna.Configuration;
        return new GateModel
        {
            Title = configuration["title"],
            Message = configuration["message"],
            Today = configuration["today"],
            Readers = configuration["readers"],
            Last = configuration["last"],
            Direction = configuration["direction"],
            IsInfo = true
        };
    }

    #endregion
}
