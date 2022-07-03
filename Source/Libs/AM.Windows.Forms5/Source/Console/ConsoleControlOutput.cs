// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ConsoleControlOutput.cs -- выходной поток для консольного контрола
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

using AM.Text.Output;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Выходной поток для консольного контрола <see cref="ConsoleControl"/>.
/// </summary>
public sealed class ConsoleControlOutput
    : AbstractOutput
{
    #region Properties

    /// <summary>
    /// Консольный контрол.
    /// </summary>
    public ConsoleControl ConsoleControl { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConsoleControlOutput
        (
            ConsoleControl console
        )
    {
        Sure.NotNull (console);

        ConsoleControl = console;
    }

    #endregion

    #region AbstractOutput members

    /// <inheritdoc cref="AbstractOutput.HaveError" />
    public override bool HaveError { get; set; }

    /// <inheritdoc cref="AbstractOutput.Clear" />
    public override AbstractOutput Clear()
    {
        ConsoleControl.Clear();
        HaveError = false;

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.Configure" />
    public override AbstractOutput Configure
        (
            string configuration
        )
    {
        // пустое тело метода

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.Write(string)" />
    public override AbstractOutput Write
        (
            string text
        )
    {
        ConsoleControl.Write (text);

        return this;
    }

    /// <inheritdoc cref="AbstractOutput.WriteError(string)" />
    public override AbstractOutput WriteError
        (
            string text
        )
    {
        ConsoleControl.Write (Color.Red, text);
        HaveError = true;

        return this;
    }

    #endregion
}
