// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* IrbisCorpRecord.cs -- запись, полученная от ИРБИС-корпорации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

using HtmlAgilityPack;

using JetBrains.Annotations;

using ManagedIrbis;

#endregion

namespace RestfulIrbis.IrbisCorp;

/// <summary>
/// Библиографическая запись, полученная от ИРБИС-корпорации.
/// </summary>
[PublicAPI]
public sealed class IrbisCorpRecord
{
    #region Properties

    /// <summary>
    /// Идентификатор (скорее всего, MFN) записи.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Расформатированное библиографическое описание.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Поля.
    /// </summary>
    public Field[]? Fields { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Декодирование HTML.
    /// </summary>
    public bool Decode
        (
            HtmlNode node
        )
    {
        Sure.NotNull (node);

        // добыть описание легко
        Description = node.Descendants()
            .FirstOrDefault (x => x.Id == "bo")?.InnerHtml;

        // с полями чуть сложнее
        var text = node.Descendants()
            .FirstOrDefault (x => x.Id == "all")?.InnerText;
        if (!string.IsNullOrEmpty (text))
        {
            var lines = text.SplitLines();
            var fields = new List<Field>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace (line))
                {
                    var trimmed = line.Trim();
                    var parts = trimmed.Split (':', 2);
                    if (parts.Length == 2)
                    {
                        var code = parts[0][1..].ParseInt32();
                        var field = new Field (code, parts[1]);
                        fields.Add (field);
                    }
                }
            }

            Fields = fields.ToArray();
        }

        return Fields is { Length: > 0 };
    }

    #endregion

    #region Object directives

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Id.ToVisibleString();

    #endregion
}
