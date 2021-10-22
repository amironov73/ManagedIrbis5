// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* BindingConfiguration.cs -- конфигурация менеджера подшивок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Json;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Конфигурация менеджера подшивок.
    /// </summary>
    public sealed class BindingConfiguration
    {
        #region Properties

        /// <summary>
        /// Статусы экземпляров, которые можно подшивать.
        /// </summary>
        [JsonPropertyName ("goodStatus")]
        [XmlArrayItem ("goodStatus")]
        [Description ("Статусы экземпляров, которые можно подшивать")]
        public string[]? GoodStatus { get; set; } = { "0" };

        /// <summary>
        /// Рабочие листы, которые можно подшивать.
        /// </summary>
        [JsonPropertyName ("goodWorksheet")]
        [XmlArrayItem ("goodWorksheet")]
        [Description ("Рабочие листы, которые можно подшивать")]
        public string[]? GoodWorksheet { get; set; }

        /// <summary>
        /// Фонды, которые нельзя подшивать.
        /// </summary>
        [JsonPropertyName ("badPlaces")]
        [XmlArrayItem ("badPlace")]
        [Description ("Фонды, которые нельзя подшивать")]
        public string[]? BadPlaces { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка места хранения на возможность добавления в подшивку.
        /// </summary>
        public bool CheckPlace (string? place) =>
            !string.IsNullOrEmpty (place)
            && (BadPlaces is null || !BadPlaces.ContainsNoCase (place));

        /// <summary>
        /// Проверка статуса экземпляра на возможность добавления в подшивку.
        /// </summary>
        public bool CheckStatus (string? status) =>
            !string.IsNullOrEmpty (status)
            && (GoodStatus is null || GoodStatus.Contains (status));

        /// <summary>
        /// Проверка рабочего листа на возможность добавления в подшивку.
        /// </summary>
        public bool CheckWorksheet (string? worksheet) =>
            !string.IsNullOrEmpty (worksheet)
            && (GoodWorksheet is null || GoodWorksheet.ContainsNoCase (worksheet));

        /// <summary>
        /// Получение конфигурации по умолчанию.
        /// </summary>
        public static BindingConfiguration GetDefault() => new ();

        /// <summary>
        /// Чтение конфигурации из указанного файла.
        /// </summary>
        public static BindingConfiguration LoadConfiguration (string fileName) =>
            JsonUtility.ReadObjectFromFile<BindingConfiguration> (fileName);

        /// <summary>
        /// Запись конфигурации в указанный файл.
        /// </summary>
        public void SaveConfiguration (string fileName) => JsonUtility.SaveObjectToFile (this, fileName);

        #endregion

    } // class BindingConfiguration

} // namespace ManagedIrbis.Magazines
