// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* GblParameter.cs -- параметр для глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    //
    // Первая строка файла задания – это число, задающее
    // количество параметров, используемых в операторах корректировки.
    //
    // Последующие пары строк, число пар должно быть равно
    // количеству параметров, используются программой
    // глобальной корректировки.
    //
    // Первая строка пары - значение параметра или пусто,
    // если пользователю предлагается задать его значение
    // перед выполнением корректировки. В этой строке можно
    // задать имя файла меню (с расширением MNU)
    // или имя рабочего листа подполей (с расширением Wss),
    // которые будут поданы для выбора значения параметра.
    // Вторая строка пары – наименование параметра,
    // которое появится в названии столбца, задающего параметр.
    //

    /// <summary>
    /// Параметр для глобальной корректировки.
    /// </summary>
    [DebuggerDisplay("{Name}: {Value}")]
    public sealed class GblParameter
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Parameter name.
        /// </summary>
        [JsonPropertyName("name")]
        [XmlAttribute("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Parameter value.
        /// </summary>
        [JsonPropertyName("value")]
        [XmlAttribute("value")]
        public string? Value { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Декодирование текстового представления параметра.
        /// </summary>
        public static GblParameter Decode
            (
                TextReader reader
            )
        {
            var result = new GblParameter
            {
                Value = reader.RequireLine(),
                Name = reader.RequireLine()
            };

            return result;
        } // method Decode

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            Value = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Name);
            writer.WriteNullable(Value);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<GblParameter>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Name, nameof(Name))
                .NotNullNorEmpty(Value, nameof(Value));

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"Name: {Name}, Value: {Value}";
        }

        #endregion
    }
}
