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
using System.Threading.Tasks;

using AM;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

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

    #endregion

    #region Public methods

    public async Task<IAsyncConnection> CreateClient()
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
            bool error = false
        )
    {
        text = HtmlText.ToPlainText (text).SafeTrim();
        Last = text;
    }

    public async void HandleReader
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
        await using var connection = await CreateClient();
        var searchParameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = $"\"RI={readerId}\""
        };
        var found = await connection.SearchAsync (searchParameters);
        if (found?.Length != 1)
        {
            ShowHtml("Читатель не найден", error: true);
            return;
        }

        var recordParameters = new ReadRecordParameters
        {
            Database = connection.EnsureDatabase(),
            Mfn = found[0].Mfn
        };
        var record = await connection.ReadRecordAsync (recordParameters);
        if (record is null)
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
            Mfns = new [] { found[0].Mfn }
        };
        await connection.FormatRecordsAsync (formatParameters);
        var html = formatParameters.Result.AsSingle();
        ShowHtml (html);
    }

    #endregion
}
