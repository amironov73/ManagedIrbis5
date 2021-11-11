// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable RedundantAssignment
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* Ngroe.cs -- номер государственной регистрации обязательного экземпляра
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Identifiers
{
    /*
        ГОСТ 7.0.105-2020.
        Номер государственной регистрации обязательного экземпляра печатного издания.
        Структура, оформление, использование.
        Дата введения: "1" января 2021.

        Настоящий стандарт распространяется на способ идентификации изданий на основе
        применения номера государственной регистрации обязательного экземпляра (НГРОЭ)
        печатного издания и печатного издания в электронной форме, устанавливает
        порядок использования, структуру, состав, форму написания, расположение
        в издании, процедуру присвоения НГРОЭ.

        НГРОЭ - уникальный идентификационный номер федерального обязательного экземпляра
        печатного издания и обязательного экземпляра печатного издания в электронной
        форме, поступающих в Российскую книжную палату - филиал ИТАР-ТАСС.

        НГРОЭ присваивают:
        - печатным непериодическим издения (текстовым, нотным, картографическим, изоизданиям);
        - выпуска (номерам) печатного периодического или продолжающегося издания;
        - комбинированным или комплектным изданиям;
        - изданиям, входящим в состав комбинированных или комплектных изданий;
        - печатному изданию в электронной форме;
        - печатным изданиям в электронной форме, входящим в состав комбинированных
          или комплектных изданий.

        НГРОЭ не присваивают:
        - документам, не являющимся изданиями;
        - публикациям в сборниках и сериальных изданиях;
        - отдельному заводу (части) тиража или дополнительному тиражу, поступившим
          в Российскую книжную палау - филиал ИТАР-ТАСС в одном календарном году
          с основным тиражом.

        НГРОЭ печатного издания в электронной форме приводят в правом нижнем углу оборота
        титульного листа или на элементе издания, выполняющим его функцию, по ГОСТ Р 7.0.4.

        НГРОЭ состоит из двух аббревиатур на русском языке и двух цифровых групп, отделяемых
        друг от друга дефисами. В состав НГРОЭ входят:
        - буквенное обозначение типа издания ("КН" - книга или брошюра, "ЖЛ" - журнал,
          "ГА" - газета, "АР" - автореферат диссертации, "НО" - нотное издание, "КА" -
          картографическое издание, "АЛ" - альбом (художественный и фотоальбом),
          "РА" - книжка-раскрасска, "ИЗ" - изоиздание некнижной формы);
        - буквенное обозначение формы издания ("П" - печатное издание, "Э" - печатное
          издание в электронной форме);
        - две последние цифры обозначения года регистрации (например, "19" - для 2019 г.
        - шестизначный порядковый номер, при необходимости дополняемый ведущими нулями,
          печатного издания или печатного издания в электронной форме, поступившего
          в течение соответствующего года регистрации.

        ПРИМЕРЫ:

        - КН-П-19-025769
        - АЛ-П-18-000321
        - ЖЛ-Э-19-000002

     */

    /// <summary>
    /// Номер государственной регистрации обязательного экземпляра печатного издания.
    /// ГОСТ 7.0.105-2020.
    /// </summary>
    [XmlRoot ("ngroe")]
    public sealed class Ngroe
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Static properties

        /// <summary>
        /// Допустимые обозначения типа издания.
        /// </summary>
        public static readonly string[] GoodTypes = { "КН", "ЖЛ", "ГА", "АР", "НО", "КА", "АЛ", "РА", "ИЗ" };

        /// <summary>
        /// Допустимые обозначения формы издания.
        /// </summary>
        public static readonly string[] GoodForms = { "П", "Э" };

        #endregion

        #region Properties

        /// <summary>
        /// Буквенное обозначение типа издания: КН, ЖЛ, ГА, АР, НО, КА, АЛ, РА, ИЗ.
        /// </summary>
        [XmlElement ("type")]
        [JsonPropertyName ("type")]
        [Description ("Тип издания")]
        [DisplayName ("Тип издания")]
        public string? Type { get; set; }

        /// <summary>
        /// Буквенное обозначение формы издания: "П" или "Э".
        /// </summary>
        [XmlElement ("form")]
        [JsonPropertyName ("form")]
        [Description ("Форма издания")]
        [DisplayName ("Форма издания")]
        public string? Form { get; set; }

        /// <summary>
        /// Две последние цифры года регистрации (не издания!).
        /// </summary>
        [XmlElement ("year")]
        [JsonPropertyName ("year")]
        [Description ("Год регистрации")]
        [DisplayName ("Год регистрации")]
        public string? Year { get; set; }

        /// <summary>
        /// Шестизначный порядковый номер, при необходимости дополняемый ведущими нулями.
        /// </summary>
        [XmlElement ("number")]
        [JsonPropertyName ("number")]
        [Description ("Порядковый номер")]
        [DisplayName ("Порядковый номер")]
        public string? Number { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор строки.
        /// </summary>
        public static Ngroe Parse
            (
                ReadOnlySpan<char> text
            )
        {
            var result = new Ngroe();
            var navigator = new ValueTextNavigator (text);
            navigator.SkipWhitespace();

            var type = navigator.ReadUntil ('-').Trim();
            if (!type.IsEmpty)
            {
                result.Type = type.ToString();
                navigator.ReadChar();
            }

            var form = navigator.ReadUntil ('-').Trim();
            if (!form.IsEmpty)
            {
                result.Form = form.ToString();
                navigator.ReadChar();
            }

            var year = navigator.ReadUntil ('-').Trim();
            if (!year.IsEmpty)
            {
                result.Year = year.ToString();
                navigator.ReadChar();
            }

            var number = navigator.GetRemainingText().Trim();
            if (!number.IsEmpty)
            {
                result.Number = number.ToString();
            }

            return result;

        } // method Parse

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Type = reader.ReadNullableString();
            Form = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Number = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Type)
                .WriteNullable (Form)
                .WriteNullable (Year)
                .WriteNullable (Number);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<Ngroe> (this, throwOnError);

            verifier
                .IsOneOf (Type, GoodTypes)
                .IsOneOf (Form, GoodForms)
                .Assert (Year.SafeLength() == 2)
                .Assert (Year.ConsistOfDigits())
                .Assert (Number.SafeLength() == 6)
                .Assert (Number.ConsistOfDigits());

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"{Type.ToVisibleString()}-{Form.ToVisibleString()}-{Year.ToVisibleString()}-{Number.ToVisibleString()}";

        #endregion

    } // class Ngroe

} // namespace ManagedIrbis.Identifiers
