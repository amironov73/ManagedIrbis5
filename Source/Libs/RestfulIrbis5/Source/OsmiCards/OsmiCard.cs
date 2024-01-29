// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* OsmiCard.cs -- карточка пользователя системы OSMI Cards.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;
using System.Text.Json.Serialization;

using AM;
using AM.Collections;

using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace RestfulIrbis.OsmiCards;

/// <summary>
/// Карточка пользователя системы OSMI Cards.
/// </summary>
public sealed class OsmiCard
{
    #region Properties

    /// <summary>
    /// Массив пар "ключ-значение".
    /// </summary>
    [JsonPropertyName("values")]
    public OsmiValue[]? Values { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Convert <see cref="JObject"/> to
    /// <see cref="OsmiCard"/>.
    /// </summary>
    public static OsmiCard? FromJObject
        (
            JObject jObject
        )
    {
        Sure.NotNull (jObject);

        var values = jObject["values"];
        if (ReferenceEquals (values, null))
        {
            return null;
        }

        var result = new OsmiCard
        {
            Values = values.ToObject<OsmiValue[]>()
        };

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var values = Values;
        if (values.IsNullOrEmpty())
        {
            return "(null)";
        }

        var builder = new StringBuilder();
        foreach (var value in values)
        {
            builder.AppendLine (value.ToString());
        }

        return builder.ToString();
    }

    #endregion
}
