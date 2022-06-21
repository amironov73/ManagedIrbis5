// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DateTimePickerExtensions.cs -- методы расширения для DateTimePicker
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="DateTimePicker"/>.
/// </summary>
public static class DateTimePickerExtensions
{
    #region Public methods

    /// <summary>
    /// Установка нестандартного формата отображения.
    /// </summary>
    public static TDateTimePicker CustomFormat<TDateTimePicker>
        (
            this TDateTimePicker dateTimePicker,
            string format
        )
        where TDateTimePicker: DateTimePicker
    {
        Sure.NotNull (dateTimePicker);
        Sure.NotNullNorEmpty (format);

        dateTimePicker.Format = DateTimePickerFormat.Custom;
        dateTimePicker.CustomFormat = format;

        return dateTimePicker;
    }

    /// <summary>
    /// Установка стандартного формата отображения.
    /// </summary>
    public static TDateTimePicker Format<TDateTimePicker>
        (
            this TDateTimePicker dateTimePicker,
            DateTimePickerFormat format
        )
        where TDateTimePicker : DateTimePicker
    {
        Sure.NotNull (dateTimePicker);
        Sure.Defined (format);

        dateTimePicker.Format = format;

        return dateTimePicker;
    }

    #endregion
}
