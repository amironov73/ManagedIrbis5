// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* SigurResponse.cs -- данные, ожидаемые Sigur в ответе на запроо
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Gatekeeper2024;

/*

    В ответ на запрос «Sigur» ожидает получить следующий объект (пример):

    {
        "allow": false,
        "message": "Недостаточно средств на счете"
    }

 */

/// <summary>
/// Данные, ожидаемые Sigur в ответе на запрос
/// </summary>
[PublicAPI]
public sealed class SigurResponse
{
    #region Properties

    /// <summary>
    /// Разрешение или запрет доступа:
    /// <list type="table">
    ///     <listheader>
    ///         <term>Логическое значение</term>
    ///         <description>Смысл</description>
    ///     </listheader>
    /// <item>
    ///     <term>false</term>
    ///     <description>если доступ нужно запретить</description>
    /// </item>
    /// <item>
    ///     <term>true</term>
    ///     <description>если доступ нужно разрешить</description>
    /// </item>
    /// </list>
    /// </summary>
    [JsonPropertyName ("allow")]
    public bool Allow { get; set; }

    /// <summary>
    /// Если «allow» имеет значение "false": сообщение о причине
    /// запрета доступа, которое будет помещено в архив системы
    /// и отображено на вкладке «Наблюдение». Если параметр
    /// "message" отсутствует или равен пустой строке, то
    /// система зафиксирует событие по умолчанию — "Доступ
    /// запрещен. По решению внешней системы".
    /// </summary>
    [JsonPropertyName ("message")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }

    /// <summary>
    /// Необязательное пояснение.
    /// </summary>
    [JsonIgnore]
    public string? Clarification { get; set; }

    #endregion

    #region Object members

    public override string ToString() => JsonSerializer.Serialize (this);

    #endregion
}
