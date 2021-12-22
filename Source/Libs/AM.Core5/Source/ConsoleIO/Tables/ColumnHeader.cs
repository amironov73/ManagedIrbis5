// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* ColumnHeader.cs -- заголовок колонки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.ConsoleIO.Tables;

/// <summary>
/// Заголовок колонки.
/// </summary>
public class ColumnHeader
{
    #region Properties

    /// <summary>
    /// Текст заголовка.
    /// </summary>
    public string Title { get; init; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="title">Текст заголовка колонки</param>
    public ColumnHeader (string title)
    {
        Title = title;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Оператор неявного преобразования строки в заголовок колонки.
    /// </summary>
    public static implicit operator ColumnHeader (string value)
    {
        return new (value);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Title;
    }

    #endregion
}
