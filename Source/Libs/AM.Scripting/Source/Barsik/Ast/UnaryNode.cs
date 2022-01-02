// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* UnaryNode.cs -- унарная операция
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Унарная операция.
/// </summary>
internal abstract class UnaryNode
    : AtomNode
{
    #region Private members

    protected dynamic Increment
        (
            dynamic value
        )
    {
        if (value is string text)
        {
            var number = new NumberText (text);
            number.Increment();

            return number.ToString();
        }

        return (value + 1);
    }

    protected dynamic Decrement
        (
            dynamic value
        )
    {
        if (value is string text)
        {
            var number = new NumberText (text);
            number.Increment();

            return number.ToString();
        }

        return (value - 1);
    }

    #endregion
}
