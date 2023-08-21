// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* CreateEmbeddingRequest.cs -- запрос на создание текстовой инверсии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Запрос на создание текстовой инверсии.
/// </summary>
[PublicAPI]
public sealed class CreateEmbeddingRequest
{
    #region Properties

    /// <summary>
    /// Имя текстовой инверсии.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Количество векторов на токен.
    /// </summary>
    [JsonProperty ("num_vectors_per_token")]
    public int NumberOfVectorsPerToken { get; set; }

    /// <summary>
    /// Перезаписать старую инверсию, если она существует?
    /// </summary>
    [JsonProperty ("overwrite_old")]
    public bool OverwriteOld { get; set; }

    /// <summary>
    /// Инициализирующий текст.
    /// </summary>
    [JsonProperty ("init_text", NullValueHandling = NullValueHandling.Ignore)]
    public string? InitializationText { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание запроса из командной строки.
    /// </summary>
    public static CreateEmbeddingRequest FromCommandLine
        (
            string[] args
        )
    {
        var result = new CreateEmbeddingRequest
        {
            Name = Guid.NewGuid().ToString ("N"),
            NumberOfVectorsPerToken = 7,
            InitializationText = "*"
        };

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "--init":
                    result.InitializationText = args[++i];
                    break;

                case "--name":
                    result.Name = args[++i];
                    break;

                case "--overwrite":
                    result.OverwriteOld = true;
                    break;

                case "--vectors":
                    result.NumberOfVectorsPerToken = args[++i].SafeToInt32 (7);
                    break;
            }
        }

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonConvert.SerializeObject (this);

    #endregion
}
