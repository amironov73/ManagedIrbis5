// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IrbisDate.cs -- работа с датой, специфичная для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using AM;
using AM.PlatformAbstraction;
using AM.Runtime;

#endregion

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Работа с датой, специфичная для ИРБИС.
/// </summary>
public partial class IrbisDate
    : IHandmadeSerializable
{
    #region Constants

    /// <summary>
    /// Формат конверсии по умолчанию.
    /// </summary>
    public const string DefaultFormat = "yyyyMMdd";

    #endregion

    #region Properties

    /// <summary>
    /// Формат конверсии.
    /// </summary>
    public static string ConversionFormat = DefaultFormat;

    /// <summary>
    /// Текущее время в формате ИРБИС.
    /// </summary>
    public static string NowText =>
        PlatformAbstractionLayer.Current.Now().ToString ("HH:mm:ss");

    /// <summary>
    /// Сегодняшняя дата в формате ИРБИС.
    /// </summary>
    public static string TodayText => new IrbisDate().Text;

    /// <summary>
    /// Дата в виде текста.
    /// </summary>
    public string Text { get; private set; }

    /// <summary>
    /// Дата как дата.
    /// </summary>
    public DateTime Date { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <remarks>
    /// Инициализирует сегодняшней датой.
    /// </remarks>
    public IrbisDate()
    {
        Date = DateTime.Today;
        Text = ConvertDateToString (Date);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisDate
        (
            string? text
        )
    {
        Text = text ?? string.Empty;
        Date = ConvertStringToDate (text);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisDate
        (
            DateTime date
        )
    {
        Date = date;
        Text = ConvertDateToString (date);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Преобразование даты в строку.
    /// </summary>
    public static string ConvertDateToString (DateTime date)
        => date.ToString (ConversionFormat);

    /// <summary>
    /// Преобразование строки в дату.
    /// </summary>
    public static DateTime ConvertStringToDate
        (
            string? date
        )
    {
        if (ReferenceEquals (date, null) || date.Length < 4)
        {
            return DateTime.MinValue;
        }

        date = date.Replace (" ", string.Empty);
        if (date.Length > 8)
        {
            var match = Date8Regex().Match(date);
            if (match.Success)
            {
                date = match.Value;
            }
        }

        DateTime.TryParseExact
            (
                date,
                ConversionFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out var result
            );

        return result;
    }

    /// <summary>
    /// Преобразование строки в дату.
    /// </summary>
    public static DateTime ConvertStringToDate
        (
            ReadOnlySpan<char> date
        )
    {
        // TODO: реализовать оптимально

        if (date.Length < 4)
        {
            return DateTime.MinValue;
        }

        if (date.Contains (' '))
        {
            date = date.ToString().Replace (" ", string.Empty);
        }

        if (date.Length > 8)
        {
            var match = Date8Regex().Match (date.ToString());
            if (match.Success)
            {
                date = match.Value;
            }
        }

        DateTime.TryParseExact
            (
                date,
                ConversionFormat,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out var result
            );

        return result;
    }

    /// <summary>
    /// Явное преобразование строки в промежуток времени.
    /// </summary>
    public static TimeSpan ConvertStringToTime
        (
            string? time
        )
    {
        if (ReferenceEquals (time, null) || time.Length < 4)
        {
            return new TimeSpan();
        }

        var hours = int.Parse (time.Substring (0, 2));
        var minutes = int.Parse (time.Substring (2, 2));
        var seconds = time.Length < 6
            ? 0
            : int.Parse (time.Substring (4, 2));
        var result = new TimeSpan (hours, minutes, seconds);

        return result;

    }

    /// <summary>
    /// Явное преобразование промежутка времени в строку.
    /// </summary>
    public static string ConvertTimeToString
        (
            TimeSpan time
        )
    {
        return string.Format
            (
                CultureInfo.InvariantCulture,
                "{0:00}{1:00}{2:00}",
                time.Hours,
                time.Minutes,
                time.Seconds
            );

    } // method ConvertTimeToString

    /// <summary>
    /// Неявное преобразование из строки.
    /// </summary>
    public static implicit operator IrbisDate (string text) => new (text);

    /// <summary>
    /// Неявное преобразование из даты.
    /// </summary>
    public static implicit operator IrbisDate (DateTime date) => new (date);

    /// <summary>
    /// Неявное преобразование в строку.
    /// </summary>
    public static implicit operator string (IrbisDate date) => date.Text;

    /// <summary>
    /// Неявное преобразование в дату.
    /// </summary>
    public static implicit operator DateTime (IrbisDate date) => date.Date;

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Text = reader.ReadString();
        Date = ConvertStringToDate (Text);

    }

    /// <see cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream (BinaryWriter writer) => writer.Write (Text);

    #endregion

    #region Private members

    [GeneratedRegex(@"\d{8}")]
    private static partial Regex Date8Regex();

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() => Text.ToVisibleString();
    #endregion
}
