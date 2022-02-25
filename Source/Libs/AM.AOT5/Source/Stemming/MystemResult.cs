// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MystemResult.cs -- результат анализа для одного слова
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace AM.AOT.Stemming;

/// <summary>
/// Результат анализа для одного слова.
/// </summary>
public sealed class MystemResult
{
    /// <summary>
    /// Анализируемый текст.
    /// </summary>
    [JsonPropertyName ("text")]
    public string? Text { get; set; }

    /// <summary>
    /// Массив анализов (при неоднозначности слова
    /// может иметь более одного элемента).
    /// </summary>
    public MystemAnalysis[]? Analysis { get; set; }

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append (Text);

        if (Analysis is not null)
        {
            foreach (MystemAnalysis analysis in Analysis)
            {
                result.Append ("; ");
                result.Append (analysis);
            }
        }

        return result.ToString();
    } // method ToString
} // class MystemResult

// namespace AM.AOT.Stemming
