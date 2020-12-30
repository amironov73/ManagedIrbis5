// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RecordDescriptor.cs -- описатель преобразования записи в указанный тип данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json;
using System.Text.Json.Serialization;

using AM;
using AM.Json;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Описатели преобразования записи в указанный тип данных.
    /// </summary>
    public class RecordDescriptor
    {
        #region Properties

        /// <summary>
        /// Дескрипторы полей.
        /// </summary>
        [JsonPropertyName("fields")]
        public FieldDescriptor[]? Fields { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор JSON-строки.
        /// </summary>
        public static RecordDescriptor FromJson
            (
                string json
            )
        {
            var options = MagnaTypeConverter.CreateOptions();
            var result = JsonSerializer.Deserialize<RecordDescriptor>(json, options)
                .ThrowIfNull(nameof(JsonSerializer.Deserialize));

            return result;
        } // method FromJson

        /// <summary>
        /// Преобразование в JSON-строку.
        /// </summary>
        public string ToJson
            (
                bool indented = false
            )
        {
            var options = MagnaTypeConverter.CreateOptions();
            options.WriteIndented = indented;
            var result = JsonSerializer.Serialize(this, options);

            return result;
        } // method ToJson

        #endregion


    } // class RecordDescriptor

} // namespace ManagedIrbis.Mapping
