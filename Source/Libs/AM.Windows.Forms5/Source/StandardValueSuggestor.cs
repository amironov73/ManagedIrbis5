// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StandardValueSuggestor.cs -- стандартный подсказчик значений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;

using Microsoft.VisualBasic;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Стандартный подсказчик значений.
/// </summary>
public class StandardValueSuggestor
    : IValueSuggestor
{
    #region Properties

    ///<summary>
    /// Заранее заданные значения.
    ///</summary>
    public ICollection StandardValues { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StandardValueSuggestor()
    {
        StandardValues = new Collection();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StandardValueSuggestor
        (
            ICollection values
        )
    {
        StandardValues = values;
    }

    #endregion

    #region IValueSuggestor members

    /// <inheritdoc cref="IValueSuggestor.GetSuggestedValues"/>
    public virtual ICollection GetSuggestedValues()
    {
        ArrayList result = new (StandardValues);
        result.Insert (0, null);

        return result;
    }

    #endregion
}
