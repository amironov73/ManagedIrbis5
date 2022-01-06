// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DebugTextWriter.cs -- пишет в отладчик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Пишет в отладчик (если программа не под отладчиком,
/// то вывод просто пропадает).
/// </summary>
public sealed class DebugTextWriter
    : TextWriter
{
    #region TextWriter members

    /// <inheritdoc cref="TextWriter.Encoding"/>
    public override Encoding Encoding { get; } = Encoding.Unicode;

    /// <inheritdoc cref="TextWriter.Write(string?)"/>
    public override void Write
        (
            string? value
        )
    {
        if (!string.IsNullOrEmpty (value))
        {
            Debug.Write (value);
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(string?)"/>
    public override void WriteLine
        (
            string? value
        )
    {
        Debug.WriteLine (value);
    }

    #endregion
}
