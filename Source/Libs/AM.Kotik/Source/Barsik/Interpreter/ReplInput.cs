// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Local

/* ReplInput.cs -- прием пользовательского ввода для REPL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Прием пользовательского ввода для REPL
/// </summary>
public sealed class ReplInput
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReplInput
        (
            TextReader reader,
            TextWriter writer,
            Interpreter interpreter
        )
    {
        Sure.NotNull (reader);
        Sure.NotNull (writer);
        Sure.NotNull (interpreter);

        _reader = reader;
        _writer = writer;
        _interpreter = interpreter;
    }

    #endregion

    #region Private members

    private readonly Interpreter _interpreter;
    private readonly TextReader _reader;
    private readonly TextWriter _writer;

    private bool CheckExpression
        (
            StringBuilder expression
        )
    {
        try
        {
            _interpreter.Grammar.ParseStatement
                (
                    expression.ToString(),
                    _interpreter.Tokenizer
                );
        }
        catch
        {
            return false;
        }

        return true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Ввод строки.
    /// </summary>
    public string ReadLine()
    {
        var builder = new StringBuilder();
        var first = true;
        while (true)
        {
            var prompt = first ? _interpreter.Settings.MainPrompt
                : _interpreter.Settings.SecondaryPrompt;
            if (!string.IsNullOrEmpty (prompt))
            {
                _writer.Write (prompt);
            }

            first = false;

            var line = _reader.ReadLine();
            if (string.IsNullOrEmpty (line))
            {
                break;
            }

            if (builder.Length != 0)
            {
                builder.AppendLine();
            }

            builder.Append (line);
            if (CheckExpression (builder))
            {
                break;
            }
        }

        return builder.ToString();
    }

    #endregion
}
