// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
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
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Работа с датой, специфичная для ИРБИС.
    /// </summary>
    public class IrbisDate
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
        /// Text representation of today date.
        /// </summary>
        public static string TodayText => new IrbisDate().Text;

        /// <summary>
        /// В виде текста.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// В виде даты.
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
            Text = ConvertDateToString(Date);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public IrbisDate
            (
                string text
            )
        {
            // Sure.NotNullNorEmpty(text, nameof(text));

            Text = text;
            Date = ConvertStringToDate(text);
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
            Text = ConvertDateToString(date);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Преобразование даты в строку.
        /// </summary>
        public static string ConvertDateToString(DateTime date)
            => date.ToString(ConversionFormat);

        /// <summary>
        /// Преобразование строки в дату.
        /// </summary>
        public static DateTime ConvertStringToDate
            (
                string? date
            )
        {
            if (ReferenceEquals(date, null) || date.Length < 4)
            {
                return DateTime.MinValue;
            }

            if (date.Length > 8)
            {
                var match = Regex.Match(date, @"\d{8}");
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
        /// Convert string to time.
        /// </summary>
        public static TimeSpan ConvertStringToTime
            (
                string? time
            )
        {
            if (ReferenceEquals(time, null) || time.Length < 4)
            {
                return new TimeSpan();
            }

            var hours = int.Parse(time.Substring(0, 2));
            var minutes = int.Parse(time.Substring(2, 2));
            var seconds = time.Length < 6
                ? 0
                : int.Parse(time.Substring(4, 2));
            var result = new TimeSpan(hours, minutes, seconds);

            return result;
        }

        /// <summary>
        /// Convert time to string.
        /// </summary>
        public static string ConvertTimeToString (TimeSpan time)
        {
            return String.Format
                (
                    CultureInfo.InvariantCulture,
                    "{0:00}{1:00}{2:00}",
                    time.Hours,
                    time.Minutes,
                    time.Seconds
                );
        }

        /// <summary>
        /// Неявное преобразование.
        /// </summary>
        public static implicit operator IrbisDate (string text) => new IrbisDate(text);

        /// <summary>
        /// Неявное преобразование.
        /// </summary>
        public static implicit operator IrbisDate (DateTime date) => new IrbisDate(date);

        /// <summary>
        /// Неявное преобразование.
        /// </summary>
        public static implicit operator string (IrbisDate date) => date.Text;

        /// <summary>
        /// Неявное преобразование.
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
            Date = ConvertStringToDate(Text);
        }

        /// <see cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream (BinaryWriter writer) => writer.Write(Text);

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Text.ToVisibleString();

        #endregion
    }
}