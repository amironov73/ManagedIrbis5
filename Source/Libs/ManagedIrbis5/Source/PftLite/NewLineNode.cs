// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NewLineNode.cs -- перевод строки
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Перевод строки.
/// </summary>
internal sealed class NewLineNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NewLineNode
        (
            char mode
        )
    {
        _mode = mode;
    }

    #endregion

    #region Private members

    private readonly char _mode;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        context.Write ('\n');
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"newline {_mode}";
    }

    #endregion
}
