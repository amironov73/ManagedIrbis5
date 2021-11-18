// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Umid.cs -- Unique Material Identifier.
 * Ars Magna project, http://arsmagna.ru
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // https://ru.wikipedia.org/wiki/UMID
    //
    // UMID (англ. Unique Material Identifier) - уникальный идентификатор
    // аудиовизуального материала, определённый в SMPTE 330M.
    // Это специальный глобально-уникальный 64-байтный код
    // (пример: 359ABAEB603805D808004602022F7EA5), автоматически
    // генерируемый локально по ходу порождения материала и внедряемый
    // в медиа-файл или медиа-поток. Призван упростить дальнейший поиск,
    // отслеживание, предоставление доступа к медиа-материалу.
    // Основной задачей UMID является идентификация материала
    // в системах хранения данных, в течение всего процесса последующей
    // обработки, вещания/распространения. В частности, для связи материала
    // и соответствующих ему метаданных. Каждый отдельно отснятый фрагмент
    // получает свой UMID.
    //
    // Формат UMID состоит из двух частей по 32 байта каждая:
    //
    // * обязательная базовая часть, которая содержит:
    //   * универсальный идентификатор маркера SMPTE-UMID
    //   * длина UMID
    //   * номер, идентифицирующий копию
    //   * уникальный номер, идентифицирующий фрагмент или материал
    //
    // * сигнатура:
    //   * дата и время создания с точностью до фрейма
    //   * пространственные координаты камеры при съёмке оригинала
    //   * код страны
    //   * код организации
    //   * код, используемый производителем.
    //
    // UMID сыграл большую роль в распространении открытых форматов,
    // таких как MXF (Material Exchange Format) и AAF (Advanced Authoring
    // Format), и сегодня поддерживается ведущими производителями
    // в аудиовизульной индустрии.
    //

    /// <summary>
    /// Unique Material Identifier
    /// </summary>
    [XmlRoot ("umid")]
    public sealed class Umid
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Идентификатор.
        /// </summary>
        [XmlElement ("identifier")]
        [JsonPropertyName ("identifier")]
        [Description ("Идентификатор")]
        [DisplayName ("Идентификатор")]
        public string? Identifier { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстового представления.
        /// </summary>
        public void ParseText
            (
                ReadOnlySpan<char> text
            )
        {
            Sure.NotEmpty (text);

            var navigator = new ValueTextNavigator(text);
            Identifier = navigator.GetRemainingText().Trim().ThrowIfEmpty().ToString();
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Identifier = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Identifier);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Umid> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Identifier);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Identifier.ToVisibleString();
        }

        #endregion
    }
}
