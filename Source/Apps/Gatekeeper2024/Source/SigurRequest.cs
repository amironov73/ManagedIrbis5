// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* SigurRequest.cs -- данные, передаваемые в теле запроса от Sigur
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Gatekeeper2024;

/*
    Пример данных, которые будут переданы в теле запроса внешней системе
    при идентификации сотрудника считывателем, подключенным к
    контроллеру СКУД:

    {
        "type": "NORMAL",
        "keyHex": "5AB860",
        "direction": 2,
        "accessPoint": 1
    }

 */

/// <summary>
/// Данные, передаваемые Sigur в теле POST-запроса
/// при проходе читателя или сотрудника через турникет.
/// </summary>
[PublicAPI]
public sealed class SigurRequest
{
    #region Properties

    /// <summary>
    /// В описываемой ситуации всегда имеет значение
    /// "NORMAL". Другие значения соответствуют специальным
    /// логикам доступа, таким как «доступ в сопровождении» и др.
    /// </summary>
    [JsonPropertyName ("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Номер идентификатора, принятого от считывателя на
    /// контроллер. Передается в шестнадцатеричном виде,
    /// может иметь различную длину — от 3 до 7 байт. Длина
    /// определяется тем, сколько данных считыватель
    /// присылает на контроллер. Так, 3 байта соответствуют
    /// протоколу Wiegand-26, 4 байта — протоколу Wiegand-34,
    /// 7 байт — протоколу Wiegand-58.
    /// </summary>
    [JsonPropertyName ("keyHex")]
    public string? KeyHex { get; set; }

    /// <summary>
    /// Направление запрошенного доступа:
    /// <list type="table">
    ///     <listheader>
    ///         <term>Число</term>
    ///         <description>Значение</description>
    ///     </listheader>
    ///     <item>
    ///         <term>1</term>
    ///         <description>выход</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>вход</description>
    ///     </item>
    ///     <item>
    ///         <term>3</term>
    ///         <description>неизвестно</description>
    ///     </item>
    /// </list>
    /// </summary>
    [JsonPropertyName ("direction")]
    public int Direction { get; set; }

    /// <summary>
    /// Номер точки доступа, где произошла идентификация
    /// сотрудника/читателя. Соответствует номеру точки доступа
    /// на вкладке «Оборудование» ПО «Клиент».
    /// </summary>
    [JsonPropertyName ("accessPoint")]
    public int AccessPoint { get; set; }

    /// <summary>
    /// Момент получения запроса.
    /// </summary>
    [JsonPropertyName ("arrived")]
    public DateTimeOffset Arrived { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => KeyHex ?? "(none)";

    #endregion
}
