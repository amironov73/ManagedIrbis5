    // This is an open source non-commercial project. Dear PVS-Studio, please check it.
    // PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

    // ReSharper disable CheckNamespace
    // ReSharper disable CommentTypo
    // ReSharper disable StringLiteralTypo

    /* ScriptInfo.cs -- информация о скрипте
     * Ars Magna project, http://arsmagna.ru
     */

    #region Using directives

    using JetBrains.Annotations;

    using Newtonsoft.Json;

    #endregion

    #nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Информация о скрипте.
/// </summary>
[PublicAPI]
public sealed class ScriptInfo
{
    #region Properties

    /// <summary>
    /// Наименование скрипта.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Всегда активен?
    /// </summary>
    [JsonProperty ("is_alwayson", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool IsAlwaysOn { get; set; }

    /// <summary>
    /// Предназначен для режима "картинка в картинку"?
    /// </summary>
    [JsonProperty ("is_img2img", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool IsImageToImage { get; set; }

    /// <summary>
    /// Аргументы скрипта.
    /// </summary>
    [JsonProperty ("args", NullValueHandling = NullValueHandling.Ignore)]
    public ScriptArgument[]? Arguments { get; set; }

    ///<summary>
    /// Скрипты, доступные для режима "картинка в картинку".
    /// </summary>
    [JsonProperty ("img2img", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? ImageToImage { get; set; }

    #endregion
}
