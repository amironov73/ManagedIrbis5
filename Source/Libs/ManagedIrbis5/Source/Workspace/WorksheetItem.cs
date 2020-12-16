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

/* WorksheetItem.cs -- строчка в рабочем листе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.IO;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace
{
    //
    // РЛ полей содержит таблицу  с элементами для ввода/корректировки,
    // имеющую следующие столбцы:
    //
    // Метка - числовая метка поля.
    // Вводится произвольно или выбирается из таблицы полного
    // описания полей, вызываемой по кнопке или команде <F2>.
    // Таблицу полного описания полей предварительно следует загрузить.
    //
    // Наименование - наименование поля.
    // Вводится произвольно или выбирается из таблицы полного
    // описания полей, вызываемой по кнопке или команде <F2>.
    // Таблицу полного описания полей предварительно следует загрузить.
    //
    // Повторяемость поля.
    // Выбирается из предлагаемого списка (1 - повторяющееся,
    // 0 - неповторяющееся).
    //
    // Индекс контекстной помощи
    // - ссылка на текст в Инструкции каталогизатора.
    //
    // Режим ввода
    // - использование расширенных средств ввода. Выбирается
    // из предлагаемого списка. Знак «!» перед значением расширенного
    // средства ввода определяет его обязательное использование.
    // Если указать в качестве режима ввода «!0», то это будет
    // означать запрет на корректировку соответствующего поля.
    //
    // Доп.инф
    // - дополнительная информация для Расширенных средств ввода
    // (АРМ Каталогизатор).
    // Значение и структура данного параметра зависит от значения
    // предыдущего параметра – РЕЖИМ ВВОДА
    //
    // ФЛК
    // - формат ФЛК поля (Приложение 4 п. 12). Указывается в виде
    // непосредственного формата или в виде имени предварительно
    // созданного формата (без расширения) с предшествующим символом «@».
    //
    // Подсказка
    // - текст помощи (инструкции), сопровождающий ввод в поле.
    //
    // Значение по умолчанию (статическое)
    // - значение поля по умолчанию при создании новой записи
    // (статическое значение по умолчанию). Указывается непосредственно
    // или через параметр инициализационного файла в виде:
    // @<SECTION>,<NAME>,<DEFAULT>, где:
    // <SECTION>  - секция инициализационного файла;
    // <NAME>        - имя параметра;
    // <DEFAULT>  - значение параметра по умолчанию.
    //
    // Доп.инф. [резерв]
    // - используется при определенных режимах ввода (1, 2, 3, 6, 10, 11).
    // Определяет правила объединения данных при групповом вводе
    // в одно поле. Может иметь вид:
    // RXXX   - вставлять разделители XXX справа от каждого отобранного
    // элемента, кроме последнего;
    // LXXX    - вставлять разделители XXX слева от каждого отобранного
    // элемента;
    // DXXYY - каждый отобранный элемент заключать слева разделителями
    // XX и справа - YY.
    // Если параметр остается пустым - групповой ввод в одно поле запрещен.
    //

    //
    // Столбцы РЛ подполей:
    // 1. Разделитель - односимвольный идентификатор подполя
    // (латиница или цифра, нет разницы между строчными
    // и прописными буквами).
    // 2. Наименование - название подполя.
    // 3. Повторяемость - единственное допустимое значение
    // 0 - неповторяющееся.
    // 4. Индекс контекстной помощи- то же, что и для РЛ полей.
    // 5. Режим ввода - то же, что и для РЛ полей.
    // 6. Доп.инф. - то же, что и для РЛ полей.
    // 7. ФЛК - не используется.
    // 8. Умолчание - не используется.
    // 9. Подсказка - то же, что и для РЛ полей.
    // 10. [резерв] - то же, что и для РЛ полей.
    //

    /// <summary>
    /// Строчка в рабочем листе.
    /// </summary>
    [DebuggerDisplay("{Tag} {Title} [{Repeatable}][{EditMode}]")]
    public sealed class WorksheetItem
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Числовая метка поля.
        /// </summary>
        public string? Tag { get; set; }

        /// <summary>
        /// Наименование поля.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Повторяемость поля.
        /// </summary>
        public bool Repeatable { get; set; }

        /// <summary>
        /// Индекс контекстной помощи.
        /// </summary>
        public string? Help { get; set; }

        /// <summary>
        /// Режим ввода.
        /// </summary>
        public string? EditMode { get; set; }

        /// <summary>
        /// Дополнительная информация для расширенных
        /// средств ввода.
        /// </summary>
        public string? InputInfo { get; set; }

        /// <summary>
        /// ФЛК.
        /// </summary>
        public string? FormalVerification { get; set; }

        /// <summary>
        /// Подсказка - текст помощи (инструкции),
        /// сопровождающий ввод в поле.
        /// </summary>
        public string? Hint { get; set; }

        /// <summary>
        /// Знчение по умолчанию при создании
        /// новой записи.
        /// </summary>
        public string? DefaultValue { get; set; }

        /// <summary>
        /// Используется при определенных режимах ввода.
        /// </summary>
        public string? Reserved { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование в текстовое представление.
        /// </summary>
        public void Encode
            (
                TextWriter writer
            )
        {
            writer.WriteLine(Tag);
            writer.WriteLine(Title);
            writer.WriteLine(Repeatable ? "1" : "0");
            writer.WriteLine(Help);
            writer.WriteLine(EditMode);
            writer.WriteLine(InputInfo);
            writer.WriteLine(FormalVerification);
            writer.WriteLine(Hint);
            writer.WriteLine(DefaultValue);
            writer.WriteLine(Reserved);
        } // method Encode

        /// <summary>
        /// Декодирование текстового представления.
        /// </summary>
        public void Decode
            (
                TextReader reader
            )
        {
            Tag = reader.RequireLine().EmptyToNull();
            Title = reader.RequireLine().Trim().EmptyToNull();
            Repeatable = int.Parse(reader.RequireLine()) != 0;
            Help = reader.RequireLine().Trim().EmptyToNull();
            EditMode = reader.RequireLine().EmptyToNull();
            InputInfo = reader.RequireLine().EmptyToNull();
            FormalVerification = reader.RequireLine().Trim().EmptyToNull();
            Hint = reader.RequireLine().Trim().EmptyToNull();
            DefaultValue = reader.RequireLine().Trim().EmptyToNull();
            Reserved = reader.RequireLine().Trim().EmptyToNull();
        } // method Decode

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {

            Tag = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Repeatable = reader.ReadBoolean();
            Help = reader.ReadNullableString();
            EditMode = reader.ReadNullableString();
            InputInfo = reader.ReadNullableString();
            FormalVerification = reader.ReadNullableString();
            Hint = reader.ReadNullableString();
            DefaultValue = reader.ReadNullableString();
            Reserved = reader.ReadNullableString();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Tag)
                .WriteNullable(Title)
                .Write(Repeatable);
            writer
                .WriteNullable(Help)
                .WriteNullable(EditMode);
            writer
                .WriteNullable(InputInfo)
                .WriteNullable(FormalVerification)
                .WriteNullable(Hint)
                .WriteNullable(DefaultValue)
                .WriteNullable(Reserved);
        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<WorksheetItem>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Tag, "Tag")
                .NotNullNorEmpty(Title, "Title")
                .NotNullNorEmpty(EditMode, "EditMode");

            return verifier.Result;
        }

        #endregion

    } // class WorksheetItem

} // namespace ManagedIrbis.Workspace
