// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NewLineNode.cs -- перевод строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace MicroPft.Ast;

/*

  Команда '/' приводит к размещению последующих данных с начала следующей строки.
  Однако подряд расположенные команды '/', хотя и являются синтаксически правильными,
  но имеют тот же смысл, что и одна команда '/', т.е. команда '/' никогда не создает
  пустых строк.

  Команда '#'выполняет те же действия, что и '/', но переход на новую строку является
  безусловным. Можно использовать комбинацию '/#' для создания одной (и только одной)
  пустой строки.

  Команда '%' подавляет все последовательно расположенные пустые строки (если они
  имеются) между текущей строкой и последней непустой строкой. В облегченном форматтере
  не поддерживается. Вместо '%' ничего не выводится.

 */

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

    private char _mode;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        if (_mode == '#')
        {
            context.Write ('\n');
        }
        else if (_mode == '/')
        {
            if (context.Output.ColumnNumber != 0)
            {
                context.Write ('\n');
            }
        }
    }

    #endregion

    #region MereSerializer members

    /// <inheritdoc cref="PftNode.MereSerialize"/>
    public override void MereSerialize
        (
            BinaryWriter writer
        )
    {
        writer.Write (_mode);
    }

    /// <inheritdoc cref="PftNode.MereDeserialize"/>
    public override void MereDeserialize
        (
            BinaryReader reader
        )
    {
        _mode = reader.ReadChar();
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
