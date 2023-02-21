// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Local

/* RadInput.cs -- ввод с помощью RadConsole
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text;
using System.Threading;

using RadLine;
using Spectre.Console;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Пользовательский ввод с помощью RadConsole.
/// </summary>
public sealed class RadInput
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RadInput()
    {
        _editor = new LineEditor()
        {
            MultiLine = true
        };
    }

    #endregion

    #region Private members

    private readonly LineEditor _editor;

    #endregion

    #region Public methods


    /// <summary>
    /// Ввод строки.
    /// </summary>
    public string? ReadLine()
    {
        var result = _editor.ReadLine (CancellationToken.None)
            .GetAwaiter().GetResult();
        if (!string.IsNullOrWhiteSpace (result))
        {
            _editor.History.Add (result);
        }

        return result;
    }


    #endregion
}
