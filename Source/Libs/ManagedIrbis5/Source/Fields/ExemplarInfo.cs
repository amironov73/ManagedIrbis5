// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ExemplarInfo.cs -- информация об экземпляре (поле 910).
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Информация об экземпляре (поле 910).
    /// </summary>
    [XmlRoot("exemplar")]
    public sealed class ExemplarInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "!=0124abcdefhiknpqrstuvwxyz";

        /// <summary>
        /// Тег полей, содержащих сведения об экземплярах.
        /// </summary>
        public const int ExemplarTag = 910;

        #endregion

        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Статус. Подполе a.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("status")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Инвентарный номер. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("number")]
        [JsonPropertyName("number")]
        public string? Number { get; set; }

        /// <summary>
        /// Дата поступления. Подполе c.
        /// </summary>
        [SubField('c')]
        [XmlAttribute("date")]
        [JsonPropertyName("date")]
        public string? Date { get; set; }

        /// <summary>
        /// Место хранения. Подполе d.
        /// </summary>
        [SubField('d')]
        [XmlAttribute("place")]
        [JsonPropertyName("place")]
        public string? Place { get; set; }

        /// <summary>
        /// Наименование коллекции. Подполе q.
        /// </summary>
        [SubField('q')]
        [XmlAttribute("collection")]
        [JsonPropertyName("collection")]
        public string? Collection { get; set; }

        /// <summary>
        /// Расстановочный шифр. Подполе r.
        /// </summary>
        [SubField('r')]
        [XmlAttribute("shelf-index")]
        [JsonPropertyName("shelf-index")]
        public string? ShelfIndex { get; set; }

        /// <summary>
        /// Цена экземпляра. Подполе e.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("price")]
        [JsonPropertyName("price")]
        public string? Price { get; set; }

        /// <summary>
        /// Штрих-код/радиометка. Подполе h.
        /// </summary>
        [SubField('h')]
        [XmlAttribute("barcode")]
        [JsonPropertyName("barcode")]
        public string? Barcode { get; set; }

        /// <summary>
        /// Число экземпляров. Подполе 1.
        /// </summary>
        [SubField('1')]
        [XmlAttribute("amount")]
        [JsonPropertyName("amount")]
        public string? Amount { get; set; }

        /// <summary>
        /// Специальное назначение фонда. Подполе t.
        /// </summary>
        [SubField('t')]
        [XmlAttribute("purpose")]
        [JsonPropertyName("purpose")]
        public string? Purpose { get; set; }

        /// <summary>
        /// Коэффициент многоразового использования. Подполе =.
        /// </summary>
        [SubField('=')]
        [XmlAttribute("coefficient")]
        [JsonPropertyName("coefficient")]
        public string? Coefficient { get; set; }

        /// <summary>
        /// Экземпляры не на баланс. Подполе 4.
        /// </summary>
        [SubField('4')]
        [XmlAttribute("off-balance")]
        [JsonPropertyName("off-balance")]
        public string? OffBalance { get; set; }

        /// <summary>
        /// Номер записи КСУ. Подполе u.
        /// </summary>
        [SubField('u')]
        [XmlAttribute("ksu-number1")]
        [JsonPropertyName("ksu-number1")]
        public string? KsuNumber1 { get; set; }

        /// <summary>
        /// Номер акта. Подполе y.
        /// </summary>
        [SubField('y')]
        [XmlAttribute("act-number1")]
        [JsonPropertyName("act-number1")]
        public string? ActNumber1 { get; set; }

        /// <summary>
        /// Канал поступления. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("channel")]
        [JsonPropertyName("channel")]
        public string? Channel { get; set; }

        /// <summary>
        /// Число выданных экземпляров. Подполе 2.
        /// </summary>
        [SubField('2')]
        [XmlAttribute("on-hand")]
        [JsonPropertyName("on-hand")]
        public string? OnHand { get; set; }

        /// <summary>
        /// Номер акта списания. Подполе v.
        /// </summary>
        [SubField('v')]
        [XmlAttribute("act-number2")]
        [JsonPropertyName("act-number2")]
        public string? ActNumber2 { get; set; }

        /// <summary>
        /// Количество списываемых экземпляров. Подполе x.
        /// </summary>
        [SubField('x')]
        [XmlAttribute("write-off")]
        [JsonPropertyName("write-off")]
        public string? WriteOff { get; set; }

        /// <summary>
        /// Количество экземпляров для докомплектования. Подполе k.
        /// </summary>
        [SubField('k')]
        [XmlAttribute("completion")]
        [JsonPropertyName("completion")]
        public string? Completion { get; set; }

        /// <summary>
        /// Номер акта передачи в другое подразделение. Подполе w.
        /// </summary>
        [SubField('w')]
        [XmlAttribute("act-number3")]
        [JsonPropertyName("act-number3")]
        public string? ActNumber3 { get; set; }

        /// <summary>
        /// Количество передаваемых экземпляров. Подполе z.
        /// </summary>
        [SubField('z')]
        [XmlAttribute("moving")]
        [JsonPropertyName("moving")]
        public string? Moving { get; set; }

        /// <summary>
        /// Новое место хранения. Подполе m.
        /// </summary>
        [SubField('m')]
        [XmlAttribute("new-place")]
        [JsonPropertyName("new-place")]
        public string? NewPlace { get; set; }

        /// <summary>
        /// Дата проверки фонда. Подполе s.
        /// </summary>
        [SubField('s')]
        [XmlAttribute("checked-date")]
        [JsonPropertyName("checked-date")]
        public string? CheckedDate { get; set; }

        /// <summary>
        /// Число проверенных экземпляров. Подполе 0.
        /// </summary>
        [SubField('0')]
        [XmlAttribute("checked-amount")]
        [JsonPropertyName("checked-amount")]
        public string? CheckedAmount { get; set; }

        /// <summary>
        /// Реальное место нахождения книги. Подполе !.
        /// </summary>
        [SubField('!')]
        [XmlAttribute("real-place")]
        [JsonPropertyName("real-place")]
        public string? RealPlace { get; set; }

        /// <summary>
        /// Шифр подшивки. Подполе p.
        /// </summary>
        [SubField('p')]
        [XmlAttribute("binding-index")]
        [JsonPropertyName("binding-index")]
        public string? BindingIndex { get; set; }

        /// <summary>
        /// Инвентарный номер подшивки. Подполе i.
        /// </summary>
        [SubField('i')]
        [XmlAttribute("binding-number")]
        [JsonPropertyName("binding-number")]
        public string? BindingNumber { get; set; }

        /// <summary>
        /// Год издания. Берётся не из подполя.
        /// </summary>
        [XmlAttribute("year")]
        [JsonPropertyName("year")]
        public string? Year { get; set; }

        /// <summary>
        /// Прочие подполя, не попавшие в вышеперечисленные.
        /// </summary>
        [XmlElement("other-subfields")]
        [JsonPropertyName("other-subfields")]
        public SubField[]? OtherSubFields { get; set; }

        /// <summary>
        /// MFN записи, из которой заимствован экземпляр.
        /// </summary>
        [XmlAttribute("mfn")]
        [JsonPropertyName("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Краткое библиографическое описание экземпляра.
        /// </summary>
        [XmlAttribute("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// ББК.
        /// </summary>
        [XmlAttribute("bbk")]
        [JsonPropertyName("bbk")]
        public string? Bbk { get; set; }

        /// <summary>
        /// Номер выпуска (для журналов).
        /// </summary>
        [XmlAttribute("issue")]
        [JsonPropertyName("issue")]
        public string? Issue { get; set; }

        /// <summary>
        /// Номер по порядку (для списков).
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int SequentialNumber { get; set; }

        /// <summary>
        /// Информация для упорядочения в списках.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string? OrderingData { get; set; }

        /// <summary>
        /// Флаг.
        /// </summary>
        [XmlAttribute("marked")]
        [JsonPropertyName("marked")]
        public bool Marked { get; set; }

        /// <summary>
        /// Record for just in case.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Record? Record { get; set; }

        /// <summary>
        /// Associated <see cref="Field"/>.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Field? Field { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the <see cref="Field"/>.
        /// </summary>
        /// <param name="field"></param>
        public void ApplyToField
            (
                Field field
            )
        {
            field
                .ApplySubField('a', Status)
                .ApplySubField('b', Number)
                .ApplySubField('c', Date)
                .ApplySubField('d', Place)
                .ApplySubField('q', Collection)
                .ApplySubField('r', ShelfIndex)
                .ApplySubField('e', Price)
                .ApplySubField('h', Barcode)
                .ApplySubField('1', Amount)
                .ApplySubField('t', Purpose)
                .ApplySubField('=', Coefficient)
                .ApplySubField('4', OffBalance)
                .ApplySubField('u', KsuNumber1)
                .ApplySubField('y', ActNumber1)
                .ApplySubField('f', Channel)
                .ApplySubField('2', OnHand)
                .ApplySubField('v', ActNumber2)
                .ApplySubField('x', WriteOff)
                .ApplySubField('k', Completion)
                .ApplySubField('w', ActNumber3)
                .ApplySubField('z', Moving)
                .ApplySubField('m', NewPlace)
                .ApplySubField('s', CheckedDate)
                .ApplySubField('0', CheckedAmount)
                .ApplySubField('!', RealPlace)
                .ApplySubField('p', BindingIndex)
                .ApplySubField('i', BindingNumber);
        } // method ApplyToField

        /// <summary>
        /// Вычисляет количество свободных экземпляров,
        /// описанных в данной структуре.
        /// </summary>
        /// <returns>Количество свободных экземпляров.</returns>
        public int GetFreeCount()
        {
            switch (Status.FirstChar())
            {
                // экземпляры индивидуального учета
                case '0': // свободный
                    return 1;

                // остальные статусы индивидуального учета
                // считаем занятыми
                // 1 - на руках у читателя
                // 5 - временно недоступен
                // 9 - забронирован

                // экземпляры группового учета
                case 'U': case 'u': // вуз
                case 'C': case 'c': // библиотечная сеть
                    var total = Amount
                        .SafeToInt32(); // всего экземпляров по месту хранения
                    var nonFree = OnHand
                        .SafeToInt32(); // из них числятся выданными
                    var free = total - nonFree;
                    return free;

                // прочие статусы не считаем
                // R - требуется размножение (ещё не отработал AUTOIN)
                // 8 - поступил, но не дошел до места хранения
                // 2 - экземпляр ещё не поступил
                // 3 - в переплете
                // 4 - утерян
                // 6 - списан
                // P - переплетен (входит в подшивку)
            }

            return 0;
        } // method GetFreeCount

        /// <summary>
        /// Вычисляет общее количество экземпляров,
        /// описанных в данной структуре.
        /// </summary>
        /// <returns>Общее количество экземпляров.</returns>
        public int GetTotalCount()
        {
            switch (Status.FirstChar())
            {
                // экземпляры индивидуального учета
                case '0': // свободный
                case '1': // на руках у читателя
                case '5': // временно недоступен
                case '9': // забронирован
                    return 1;

                // экземпляры группового учета:
                case 'U': case 'u': // вуз
                case 'C': case 'c': // библиотечная сеть
                    var count = Amount.SafeToInt32();
                    return count;

                // прочие статусы не считаем
                // R - требуется размножение (ещё не отработал AUTOIN)
                // 8 - поступил, но не дошел до места хранения
                // 2 - экземпляр ещё не поступил
                // 3 - в переплете
                // 4 - утерян
                // 6 - списан
                // P - переплетен (входит в подшивку)
            }

            return 0;
        } // method GetTotalCount

        /// <summary>
        /// Parses the specified field.
        /// </summary>
        public static ExemplarInfo Parse
            (
                Field field
            )
        {
            var result = new ExemplarInfo
                {
                    Status         = field.GetFirstSubFieldValue('a'),
                    Number         = field.GetFirstSubFieldValue('b'),
                    Date           = field.GetFirstSubFieldValue('c'),
                    Place          = field.GetFirstSubFieldValue('d'),
                    Collection     = field.GetFirstSubFieldValue('q'),
                    ShelfIndex     = field.GetFirstSubFieldValue('r'),
                    Price          = field.GetFirstSubFieldValue('e'),
                    Barcode        = field.GetFirstSubFieldValue('h'),
                    Amount         = field.GetFirstSubFieldValue('1'),
                    Purpose        = field.GetFirstSubFieldValue('t'),
                    Coefficient    = field.GetFirstSubFieldValue('='),
                    OffBalance     = field.GetFirstSubFieldValue('4'),
                    KsuNumber1     = field.GetFirstSubFieldValue('u'),
                    ActNumber1     = field.GetFirstSubFieldValue('y'),
                    Channel        = field.GetFirstSubFieldValue('f'),
                    OnHand         = field.GetFirstSubFieldValue('2'),
                    ActNumber2     = field.GetFirstSubFieldValue('v'),
                    WriteOff       = field.GetFirstSubFieldValue('x'),
                    Completion     = field.GetFirstSubFieldValue('k'),
                    ActNumber3     = field.GetFirstSubFieldValue('w'),
                    Moving         = field.GetFirstSubFieldValue('z'),
                    NewPlace       = field.GetFirstSubFieldValue('m'),
                    CheckedDate    = field.GetFirstSubFieldValue('s'),
                    CheckedAmount  = field.GetFirstSubFieldValue('0'),
                    RealPlace      = field.GetFirstSubFieldValue('!'),
                    BindingIndex   = field.GetFirstSubFieldValue('p'),
                    BindingNumber  = field.GetFirstSubFieldValue('i'),
                    OtherSubFields = field.Subfields
                        .Where(sub => KnownCodes
                            .IndexOf(char.ToLower(sub.Code)) < 0)
                        .ToArray(),
                    Field = field
                };

            return result;
        } // method Parse

        /// <summary>
        /// Разбор записи на экземпляры.
        /// </summary>
        public static ExemplarInfo[] Parse
            (
                Record record,
                int tagNumber = ExemplarTag
            )
        {
            var result = record.Fields
                .GetField(tagNumber)
                .Select(field => Parse(field))
                .ToArray();

            foreach (ExemplarInfo exemplar in result)
            {
                exemplar.Mfn = record.Mfn;
                exemplar.Description = record.Description;
                exemplar.Record = record;
            }

            return result;
        } // method Parse

        /// <summary>
        /// Should serialize <see cref="OtherSubFields"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOtherSubFields()
        {
            return !OtherSubFields.IsNullOrEmpty();
        } // method ShouldSerializeOtherSubFields

        /// <summary>
        /// Преобразование экземпляра обратно в поле записи.
        /// </summary>
        public Field ToField()
        {
            var result = new Field { Tag = ExemplarTag }
                .AddNonEmptySubField('a', Status)
                .AddNonEmptySubField('b', Number)
                .AddNonEmptySubField('c', Date)
                .AddNonEmptySubField('d', Place)
                .AddNonEmptySubField('q', Collection)
                .AddNonEmptySubField('r', ShelfIndex)
                .AddNonEmptySubField('e', Price)
                .AddNonEmptySubField('h', Barcode)
                .AddNonEmptySubField('1', Amount)
                .AddNonEmptySubField('t', Purpose)
                .AddNonEmptySubField('=', Coefficient)
                .AddNonEmptySubField('4', OffBalance)
                .AddNonEmptySubField('u', KsuNumber1)
                .AddNonEmptySubField('y', ActNumber1)
                .AddNonEmptySubField('f', Channel)
                .AddNonEmptySubField('2', OnHand)
                .AddNonEmptySubField('v', ActNumber2)
                .AddNonEmptySubField('x', WriteOff)
                .AddNonEmptySubField('k', Completion)
                .AddNonEmptySubField('w', ActNumber3)
                .AddNonEmptySubField('z', Moving)
                .AddNonEmptySubField('m', NewPlace)
                .AddNonEmptySubField('s', CheckedDate)
                .AddNonEmptySubField('0', CheckedAmount)
                .AddNonEmptySubField('!', RealPlace)
                .AddNonEmptySubField('p', BindingIndex)
                .AddNonEmptySubField('i', BindingNumber);

            if (OtherSubFields != null)
            {
                foreach (var subField in OtherSubFields)
                {
                    result.Add(subField.Code, subField.Value);
                }
            }

            return result;
        } // method ToField

        /// <summary>
        /// Compares two specified numbers.
        /// </summary>
        public static int CompareNumbers
            (
                ExemplarInfo first,
                ExemplarInfo second
            )
        {
            var one = new NumberText(first.Number);
            var two = new NumberText(second.Number);
            var result = one.CompareTo(two);

            return result;
        } // method CompareNumbers

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Id = reader.ReadPackedInt32();
            Status = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Date = reader.ReadNullableString();
            Place = reader.ReadNullableString();
            Collection = reader.ReadNullableString();
            ShelfIndex = reader.ReadNullableString();
            Price = reader.ReadNullableString();
            Barcode = reader.ReadNullableString();
            Amount = reader.ReadNullableString();
            Purpose = reader.ReadNullableString();
            Coefficient = reader.ReadNullableString();
            OffBalance = reader.ReadNullableString();
            KsuNumber1 = reader.ReadNullableString();
            ActNumber1 = reader.ReadNullableString();
            Channel = reader.ReadNullableString();
            OnHand = reader.ReadNullableString();
            ActNumber2 = reader.ReadNullableString();
            WriteOff = reader.ReadNullableString();
            Completion = reader.ReadNullableString();
            ActNumber3 = reader.ReadNullableString();
            Moving = reader.ReadNullableString();
            NewPlace = reader.ReadNullableString();
            CheckedDate = reader.ReadNullableString();
            CheckedAmount = reader.ReadNullableString();
            RealPlace = reader.ReadNullableString();
            BindingIndex = reader.ReadNullableString();
            BindingNumber = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Bbk = reader.ReadNullableString();
            Issue = reader.ReadNullableString();
            OrderingData = reader.ReadNullableString();
            Mfn = reader.ReadInt32();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Id)
                .WriteNullable(Status)
                .WriteNullable(Number)
                .WriteNullable(Date)
                .WriteNullable(Place)
                .WriteNullable(Collection)
                .WriteNullable(ShelfIndex)
                .WriteNullable(Price)
                .WriteNullable(Barcode)
                .WriteNullable(Amount)
                .WriteNullable(Purpose)
                .WriteNullable(Coefficient)
                .WriteNullable(OffBalance)
                .WriteNullable(KsuNumber1)
                .WriteNullable(ActNumber1)
                .WriteNullable(Channel)
                .WriteNullable(OnHand)
                .WriteNullable(ActNumber2)
                .WriteNullable(WriteOff)
                .WriteNullable(Completion)
                .WriteNullable(ActNumber3)
                .WriteNullable(Moving)
                .WriteNullable(NewPlace)
                .WriteNullable(CheckedDate)
                .WriteNullable(CheckedAmount)
                .WriteNullable(RealPlace)
                .WriteNullable(BindingIndex)
                .WriteNullable(BindingNumber)
                .WriteNullable(Year)
                .WriteNullable(Description)
                .WriteNullable(Bbk)
                .WriteNullable(Issue)
                .WriteNullable(OrderingData);
            writer.Write(Mfn);
        } // method SaveToStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            string result = $"{Number} ({Place}) [{Status}]";

            if (!string.IsNullOrEmpty(BindingNumber))
            {
                result = result + " <binding " + BindingNumber + ">";
            }

            return result;
        } // method ToString

        #endregion

    } // class ExemplarInfo

} // namespace ManagedIrbis.Fields
