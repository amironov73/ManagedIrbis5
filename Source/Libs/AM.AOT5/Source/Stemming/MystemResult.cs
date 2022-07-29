// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MystemResult.cs -- результат анализа для одного слова
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM.Text;

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
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (Text);

        if (Analysis is not null)
        {
            foreach (var analysis in Analysis)
            {
                builder.Append ("; ");
                builder.Append (analysis);
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }
}
