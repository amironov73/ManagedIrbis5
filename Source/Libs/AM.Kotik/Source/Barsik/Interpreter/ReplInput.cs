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
            IGrammar? grammar
        )
    {
        Sure.NotNull (reader);
        Sure.NotNull (writer);

        _reader = reader;
        _writer = writer;
        _grammar = grammar;
    }

    #endregion

    #region Private members

    private readonly IGrammar? _grammar;
    private readonly TextReader _reader;
    private readonly TextWriter _writer;

    #endregion

    #region Public methods

    /// <summary>
    /// Ввод строки.
    /// </summary>
    public string? ReadLine
        (
            string? firstPrompt,
            string? continuePrompt
        )
    {
        _grammar.NotUsed();
        continuePrompt.NotUsed();

        if (!string.IsNullOrEmpty (firstPrompt))
        {
            _writer.Write (firstPrompt);
        }

        return _reader.ReadLine();
    }

    #endregion
}
