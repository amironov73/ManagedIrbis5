// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FormatParser.cs -- парсит форматные строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AM.Kotik.Tokenizers;
using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит форматные строки.
/// </summary>
public sealed class FormatParser
    : Parser<FormatSpecification[]>
{
    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse 
        (
            ParseState state, 
            [MaybeNullWhen (false)] out FormatSpecification[] result
        )
    {
        result = default;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }
        
        var current = state.Current;
        if (current.Kind != TokenKind.Format)
        {
            return DebugSuccess (state, false);
        }

        var list = new List<FormatSpecification>();
        var navigator = new TextNavigator (current.Value ?? string.Empty);
        while (!navigator.IsEOF)
        {
            string? value = null;
            string? format = null;
            var prefix = navigator.ReadUntil ('{').EmptyToNull();
            if (navigator.PeekChar() == '{')
            {
                // TODO обрабатывать удвоение {{
                
                navigator.ReadChar();
                var textBetween = navigator.ReadUntil ('}').EmptyToNull();
                if (string.IsNullOrEmpty (textBetween) || navigator.PeekChar() != '}')
                {
                    throw new SyntaxException (state);
                }

                navigator.ReadChar(); // съели закрыающую скобку

                if (textBetween.Contains (':'))
                {
                    var parts = textBetween.Split (':', 2);
                    value = parts[0];
                    format = parts[1];
                }
                else
                {
                    value = textBetween;
                }
            }

            var item = new FormatSpecification
            {
                Prefix = prefix,
                Value = value,
                Format = format
            };
            list.Add (item);
        }

        state.Advance();
        result = list.ToArray();
        
        return true;
    }

    #endregion
}
