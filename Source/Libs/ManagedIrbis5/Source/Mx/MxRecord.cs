// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MxRecord.cs -- информация о найденной записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Mx;

/// <summary>
/// Информация о найденной (и, возможно, расформатированной) записи.
/// </summary>
public sealed class MxRecord
{
    #region Properties

    /// <summary>
    /// Порядковый номер.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// MFN.
    /// </summary>
    public int Mfn { get; set; }

    /// <summary>
    /// Шифр записи в базе (поле 903, если есть).
    /// </summary>
    public string? Index { get; set; }

    /// <summary>
    /// Собственно запись.
    /// </summary>
    public Record? Record { get; set; }

    /// <summary>
    /// Библиографическое описание - расформатированная запись.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Информация для сортировки.
    /// </summary>
    public string? Order { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    public object? UserData { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        if (string.IsNullOrEmpty (Description))
        {
            return Mfn.ToInvariantString();
        }

        return Description!;
    }

    #endregion
}
