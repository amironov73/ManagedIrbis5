// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ExemplarInfo.cs -- информация об экземпляре (поле 910).
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
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

using VerificationException = System.Security.VerificationException;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

/// <summary>
/// Информация об экземпляре (поле 910).
/// </summary>
[XmlRoot ("exemplar")]
public sealed class ExemplarInfo
    : IHandmadeSerializable,
    IVerifiable
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
    /// Идентификатор для LiteDB.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public int Id { get; set; }

    /// <summary>
    /// Статус экземпляра. Подполе a.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("status")]
    [JsonPropertyName ("status")]
    [DisplayName ("Статус экземпляра")]
    public string? Status { get; set; }

    /// <summary>
    /// Инвентарный номер. Подполе b.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("number")]
    [JsonPropertyName ("number")]
    [DisplayName ("Инвентарный номер")]
    public string? Number { get; set; }

    /// <summary>
    /// Дата поступления. Подполе c.
    /// </summary>
    [SubField ('c')]
    [XmlAttribute ("date")]
    [JsonPropertyName ("date")]
    [DisplayName ("Дата поступления")]
    public string? Date { get; set; }

    /// <summary>
    /// Место хранения. Подполе d.
    /// </summary>
    [SubField ('d')]
    [XmlAttribute ("place")]
    [JsonPropertyName ("place")]
    [DisplayName ("Место хранения")]
    public string? Place { get; set; }

    /// <summary>
    /// Наименование коллекции. Подполе q.
    /// </summary>
    [SubField ('q')]
    [XmlAttribute ("collection")]
    [JsonPropertyName ("collection")]
    [DisplayName ("Наименование коллекции")]
    public string? Collection { get; set; }

    /// <summary>
    /// Расстановочный шифр. Подполе r.
    /// </summary>
    [SubField ('r')]
    [XmlAttribute ("shelf-index")]
    [JsonPropertyName ("shelf-index")]
    [DisplayName ("Расстановочный шифр")]
    public string? ShelfIndex { get; set; }

    /// <summary>
    /// Цена экземпляра. Подполе e.
    /// </summary>
    [SubField ('e')]
    [XmlAttribute ("price")]
    [JsonPropertyName ("price")]
    [DisplayName ("Цена экземпляра")]
    public string? Price { get; set; }

    /// <summary>
    /// Штрих-код/радиометка. Подполе h.
    /// </summary>
    [SubField ('h')]
    [XmlAttribute ("barcode")]
    [JsonPropertyName ("barcode")]
    [DisplayName ("Штрих-код/радиометка")]
    public string? Barcode { get; set; }

    /// <summary>
    /// Число экземпляров. Подполе 1.
    /// </summary>
    [SubField ('1')]
    [XmlAttribute ("amount")]
    [JsonPropertyName ("amount")]
    [DisplayName ("Число экземпляров")]
    public string? Amount { get; set; }

    /// <summary>
    /// Специальное назначение фонда. Подполе t.
    /// </summary>
    [SubField ('t')]
    [XmlAttribute ("purpose")]
    [JsonPropertyName ("purpose")]
    [DisplayName ("Специальное назначение фонда")]
    public string? Purpose { get; set; }

    /// <summary>
    /// Коэффициент многоразового использования. Подполе =.
    /// </summary>
    [SubField ('=')]
    [XmlAttribute ("coefficient")]
    [JsonPropertyName ("coefficient")]
    [DisplayName ("Коэффициент многоразового использования")]
    public string? Coefficient { get; set; }

    /// <summary>
    /// Экземпляры не на баланс. Подполе 4.
    /// </summary>
    [SubField ('4')]
    [XmlAttribute ("off-balance")]
    [JsonPropertyName ("off-balance")]
    [DisplayName ("Экземпляры не на баланс")]
    public string? OffBalance { get; set; }

    /// <summary>
    /// Номер записи КСУ. Подполе u.
    /// </summary>
    [SubField ('u')]
    [XmlAttribute ("ksu-number1")]
    [JsonPropertyName ("ksu-number1")]
    [DisplayName ("Номер записи КСУ")]
    public string? KsuNumber1 { get; set; }

    /// <summary>
    /// Номер акта. Подполе y.
    /// </summary>
    [SubField ('y')]
    [XmlAttribute ("act-number1")]
    [JsonPropertyName ("act-number1")]
    [DisplayName ("Номер акта")]
    public string? ActNumber1 { get; set; }

    /// <summary>
    /// Канал поступления. Подполе f.
    /// </summary>
    [SubField ('f')]
    [XmlAttribute ("channel")]
    [JsonPropertyName ("channel")]
    [DisplayName ("Канал поступления")]
    public string? Channel { get; set; }

    /// <summary>
    /// Число выданных экземпляров. Подполе 2.
    /// </summary>
    [SubField ('2')]
    [XmlAttribute ("on-hand")]
    [JsonPropertyName ("on-hand")]
    [DisplayName ("Число выданных экземпляров")]
    public string? OnHand { get; set; }

    /// <summary>
    /// Номер акта списания. Подполе v.
    /// </summary>
    [SubField ('v')]
    [XmlAttribute ("act-number2")]
    [JsonPropertyName ("act-number2")]
    [DisplayName ("Номер акта списания")]
    public string? ActNumber2 { get; set; }

    /// <summary>
    /// Количество списываемых экземпляров. Подполе x.
    /// </summary>
    [SubField ('x')]
    [XmlAttribute ("write-off")]
    [JsonPropertyName ("write-off")]
    [DisplayName ("Количество списываемых экземпляров")]
    public string? WriteOff { get; set; }

    /// <summary>
    /// Количество экземпляров для докомплектования. Подполе k.
    /// </summary>
    [SubField ('k')]
    [XmlAttribute ("completion")]
    [JsonPropertyName ("completion")]
    [DisplayName ("Количество экземпляров для докомплектования")]
    public string? Completion { get; set; }

    /// <summary>
    /// Номер акта передачи в другое подразделение. Подполе w.
    /// </summary>
    [SubField ('w')]
    [XmlAttribute ("act-number3")]
    [JsonPropertyName ("act-number3")]
    [DisplayName ("Номер акта передачи")]
    public string? ActNumber3 { get; set; }

    /// <summary>
    /// Количество передаваемых экземпляров. Подполе z.
    /// </summary>
    [SubField ('z')]
    [XmlAttribute ("moving")]
    [JsonPropertyName ("moving")]
    [DisplayName ("Количество передаваемых экземпляров")]
    public string? Moving { get; set; }

    /// <summary>
    /// Новое место хранения. Подполе m.
    /// </summary>
    [SubField ('m')]
    [XmlAttribute ("new-place")]
    [JsonPropertyName ("new-place")]
    [DisplayName ("Новое место хранения")]
    public string? NewPlace { get; set; }

    /// <summary>
    /// Дата проверки фонда. Подполе s.
    /// </summary>
    [SubField ('s')]
    [XmlAttribute ("checked-date")]
    [JsonPropertyName ("checked-date")]
    [DisplayName ("Дата проверки фонда")]
    public string? CheckedDate { get; set; }

    /// <summary>
    /// Число проверенных экземпляров. Подполе 0.
    /// </summary>
    [SubField ('0')]
    [XmlAttribute ("checked-amount")]
    [JsonPropertyName ("checked-amount")]
    [DisplayName ("Число проверенных экземпляров")]
    public string? CheckedAmount { get; set; }

    /// <summary>
    /// Реальное место нахождения книги. Подполе !.
    /// </summary>
    [SubField ('!')]
    [XmlAttribute ("real-place")]
    [JsonPropertyName ("real-place")]
    [DisplayName ("Реальное место нахождения книги")]
    public string? RealPlace { get; set; }

    /// <summary>
    /// Шифр подшивки. Подполе p.
    /// </summary>
    [SubField ('p')]
    [XmlAttribute ("binding-index")]
    [JsonPropertyName ("binding-index")]
    [DisplayName ("Шифр подшивки")]
    public string? BindingIndex { get; set; }

    /// <summary>
    /// Инвентарный номер подшивки. Подполе i.
    /// </summary>
    [SubField ('i')]
    [XmlAttribute ("binding-number")]
    [JsonPropertyName ("binding-number")]
    [DisplayName ("Инвентарный номер подшивки")]
    public string? BindingNumber { get; set; }

    /// <summary>
    /// Год издания. Берётся не из подполя.
    /// </summary>
    [XmlAttribute ("year")]
    [JsonPropertyName ("year")]
    [Browsable (false)]
    public string? Year { get; set; }

    /// <summary>
    /// Прочие подполя, не попавшие в вышеперечисленные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public SubField[]? OtherSubFields { get; set; }

    /// <summary>
    /// MFN записи, из которой заимствован экземпляр.
    /// </summary>
    [XmlAttribute ("mfn")]
    [JsonPropertyName ("mfn")]
    [Browsable (false)]
    public int Mfn { get; set; }

    /// <summary>
    /// Краткое библиографическое описание экземпляра.
    /// </summary>
    [XmlAttribute ("description")]
    [JsonPropertyName ("description")]
    [Browsable (false)]
    public string? Description { get; set; }

    /// <summary>
    /// ББК.
    /// </summary>
    [XmlAttribute ("bbk")]
    [JsonPropertyName ("bbk")]
    [Browsable (false)]
    public string? Bbk { get; set; }

    /// <summary>
    /// Номер выпуска (для журналов).
    /// </summary>
    [XmlAttribute ("issue")]
    [JsonPropertyName ("issue")]
    [Browsable (false)]
    public string? Issue { get; set; }

    /// <summary>
    /// Номер по порядку (для списков).
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public int SequentialNumber { get; set; }

    /// <summary>
    /// Информация для упорядочения в списках.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public string? OrderingData { get; set; }

    /// <summary>
    /// Флаг - пометка.
    /// </summary>
    [XmlAttribute ("marked")]
    [JsonPropertyName ("marked")]
    [Browsable (false)]
    public bool Marked { get; set; }

    /// <summary>
    /// Record for just in case.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Record? Record { get; set; }

    /// <summary>
    /// Associated <see cref="Field"/>.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Field? Field { get; set; }

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
    /// Apply to the <see cref="Field"/>.
    /// </summary>
    public Field ApplyToField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        return field
            .SetSubFieldValue ('a', Status)
            .SetSubFieldValue ('b', Number)
            .SetSubFieldValue ('c', Date)
            .SetSubFieldValue ('d', Place)
            .SetSubFieldValue ('q', Collection)
            .SetSubFieldValue ('r', ShelfIndex)
            .SetSubFieldValue ('e', Price)
            .SetSubFieldValue ('h', Barcode)
            .SetSubFieldValue ('1', Amount)
            .SetSubFieldValue ('t', Purpose)
            .SetSubFieldValue ('=', Coefficient)
            .SetSubFieldValue ('4', OffBalance)
            .SetSubFieldValue ('u', KsuNumber1)
            .SetSubFieldValue ('y', ActNumber1)
            .SetSubFieldValue ('f', Channel)
            .SetSubFieldValue ('2', OnHand)
            .SetSubFieldValue ('v', ActNumber2)
            .SetSubFieldValue ('x', WriteOff)
            .SetSubFieldValue ('k', Completion)
            .SetSubFieldValue ('w', ActNumber3)
            .SetSubFieldValue ('z', Moving)
            .SetSubFieldValue ('m', NewPlace)
            .SetSubFieldValue ('s', CheckedDate)
            .SetSubFieldValue ('0', CheckedAmount)
            .SetSubFieldValue ('!', RealPlace)
            .SetSubFieldValue ('p', BindingIndex)
            .SetSubFieldValue ('i', BindingNumber);
    }

    /// <summary>
    /// Подсчет количества свободных экземпляров.
    /// </summary>
    public static int GetFreeCount
        (
            IEnumerable<ExemplarInfo> exemplars
        )
    {
        Sure.NotNull (exemplars);

        var result = 0;

        foreach (var exemplar in exemplars)
        {
            result += exemplar.GetFreeCount();
        }

        return result;
    }

    /// <summary>
    /// Подсчет количества свободных экземпляров.
    /// </summary>
    public static int GetFreeCount
        (
            Record record,
            int tag = ExemplarTag
        )
    {
        Sure.NotNull (record);

        return GetFreeCount (ParseRecord (record, tag));
    }

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
            case 'U':
            case 'u': // вуз
            case 'C':
            case 'c': // библиотечная сеть
                var total = Amount
                    .SafeToInt32(); // всего экземпляров по месту хранения
                var nonFree = OnHand
                    .SafeToInt32(); // из них числятся выданными
                var free = total - nonFree;
                return free;

            // электронный экземпляр
            case 'E':
            case 'e':
                return 1;

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
    }

    /// <summary>
    /// Подсчет общего количества экземпляров.
    /// </summary>
    public static int GetTotalCount
        (
            IEnumerable<ExemplarInfo> exemplars
        )
    {
        Sure.NotNull (exemplars);

        var result = 0;

        foreach (var exemplar in exemplars)
        {
            result += exemplar.GetTotalCount();
        }

        return result;
    }

    /// <summary>
    /// Подсчет общего количества экземпляров.
    /// </summary>
    public static int GetTotalCount
        (
            Record record,
            int tag = ExemplarTag
        )
    {
        Sure.NotNull (record);

        return GetTotalCount (ParseRecord (record, tag));
    }

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
            case 'U':
            case 'u': // вуз
            case 'C':
            case 'c': // библиотечная сеть
                var count = Amount.SafeToInt32();
                return count;

            // электронный экземпляр
            case 'E':
            case 'e':
                return 1;

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
    }

    /// <summary>
    /// Доступен ли экземпляр для системы книговыдачи?
    /// </summary>
    public bool IsAvailable() => Status.FirstChar() switch
    {
        '0' or '1' or '5' or '9' => true,
        'E' or 'e' => true, // электронный экземпляр
        'U' or 'u' or 'C' or 'c' => GetFreeCount() > 0,
        _ => false
    };

    /// <summary>
    /// Электронный экземпляр?
    /// </summary>
    public bool IsElectronic() => Status.FirstChar() is 'E' or 'e';

    /// <summary>
    /// Свободен ли экземпляр (можно ли выдать читателю)?
    /// </summary>
    public bool IsFree() => Status.FirstChar() switch
    {
        '0' => true,
        'E' or 'e' => true, // электронный экземпляр
        'U' or 'u' or 'C' or 'c' => GetFreeCount() > 0,
        _ => false
    };

    /// <summary>
    /// Экземпляр индивидуального учета?
    /// </summary>
    public bool IsIndividualAccounting() => Status.FirstChar()
        is '0' or '1' or '2' or '3' or '4' or '5' or '6' or '9';

    /// <summary>
    /// Разбор указанного поля библиографической записи.
    /// </summary>
    public static ExemplarInfo ParseField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var result = new ExemplarInfo
        {
            Status = field.GetFirstSubFieldValue ('a'),
            Number = field.GetFirstSubFieldValue ('b'),
            Date = field.GetFirstSubFieldValue ('c'),
            Place = field.GetFirstSubFieldValue ('d'),
            Collection = field.GetFirstSubFieldValue ('q'),
            ShelfIndex = field.GetFirstSubFieldValue ('r'),
            Price = field.GetFirstSubFieldValue ('e'),
            Barcode = field.GetFirstSubFieldValue ('h'),
            Amount = field.GetFirstSubFieldValue ('1'),
            Purpose = field.GetFirstSubFieldValue ('t'),
            Coefficient = field.GetFirstSubFieldValue ('='),
            OffBalance = field.GetFirstSubFieldValue ('4'),
            KsuNumber1 = field.GetFirstSubFieldValue ('u'),
            ActNumber1 = field.GetFirstSubFieldValue ('y'),
            Channel = field.GetFirstSubFieldValue ('f'),
            OnHand = field.GetFirstSubFieldValue ('2'),
            ActNumber2 = field.GetFirstSubFieldValue ('v'),
            WriteOff = field.GetFirstSubFieldValue ('x'),
            Completion = field.GetFirstSubFieldValue ('k'),
            ActNumber3 = field.GetFirstSubFieldValue ('w'),
            Moving = field.GetFirstSubFieldValue ('z'),
            NewPlace = field.GetFirstSubFieldValue ('m'),
            CheckedDate = field.GetFirstSubFieldValue ('s'),
            CheckedAmount = field.GetFirstSubFieldValue ('0'),
            RealPlace = field.GetFirstSubFieldValue ('!'),
            BindingIndex = field.GetFirstSubFieldValue ('p'),
            BindingNumber = field.GetFirstSubFieldValue ('i'),
            OtherSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Разбор записи на экземпляры.
    /// </summary>
    public static ExemplarInfo[] ParseRecord
        (
            Record record,
            int tagNumber = ExemplarTag
        )
    {
        Sure.NotNull (record);
        Sure.Positive (tagNumber);

        var result = record.Fields
            .GetField (tagNumber)
            .Select (ParseField)
            .ToArray();

        foreach (var exemplar in result)
        {
            exemplar.Mfn = record.Mfn;
            exemplar.Description = record.Description;
            exemplar.Record = record;
        }

        return result;
    }

    /// <summary>
    /// Should serialize <see cref="OtherSubFields"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    [EditorBrowsable (EditorBrowsableState.Never)]
    public bool ShouldSerializeOtherSubFields()
    {
        return !OtherSubFields.IsNullOrEmpty();
    }

    /// <summary>
    /// Преобразование экземпляра обратно в поле записи.
    /// </summary>
    public Field ToField()
    {
        var result = new Field (ExemplarTag)
            .AddNonEmpty ('a', Status)
            .AddNonEmpty ('b', Number)
            .AddNonEmpty ('c', Date)
            .AddNonEmpty ('d', Place)
            .AddNonEmpty ('q', Collection)
            .AddNonEmpty ('r', ShelfIndex)
            .AddNonEmpty ('e', Price)
            .AddNonEmpty ('h', Barcode)
            .AddNonEmpty ('1', Amount)
            .AddNonEmpty ('t', Purpose)
            .AddNonEmpty ('=', Coefficient)
            .AddNonEmpty ('4', OffBalance)
            .AddNonEmpty ('u', KsuNumber1)
            .AddNonEmpty ('y', ActNumber1)
            .AddNonEmpty ('f', Channel)
            .AddNonEmpty ('2', OnHand)
            .AddNonEmpty ('v', ActNumber2)
            .AddNonEmpty ('x', WriteOff)
            .AddNonEmpty ('k', Completion)
            .AddNonEmpty ('w', ActNumber3)
            .AddNonEmpty ('z', Moving)
            .AddNonEmpty ('m', NewPlace)
            .AddNonEmpty ('s', CheckedDate)
            .AddNonEmpty ('0', CheckedAmount)
            .AddNonEmpty ('!', RealPlace)
            .AddNonEmpty ('p', BindingIndex)
            .AddNonEmpty ('i', BindingNumber);

        result.AddRange (OtherSubFields);

        return result;
    }

    /// <summary>
    /// Сравнение инвентарных номеров экземпляров.
    /// </summary>
    public static int CompareNumbers
        (
            ExemplarInfo first,
            ExemplarInfo second
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);

        var one = new NumberText (first.Number);
        var two = new NumberText (second.Number);
        var result = one.CompareTo (two);

        return result;
    }

    /// <summary>
    /// Объединение экземпляров.
    /// </summary>
    public static ExemplarInfo[]? MergeExemplars
        (
            params ExemplarInfo[]?[] arrays
        )
    {
        Sure.NotNull (arrays);

        var result = new Dictionary<string, ExemplarInfo>();
        foreach (var array in arrays)
        {
            if (array is not null)
            {
                foreach (var item in array)
                {
                    var key = item.Number;
                    if (!key.IsEmpty())
                    {
                        result[key] = item;
                    }
                }
            }
        }

        return result.Count == 0 ? null : result.Values.ToArray();
    }

    /// <summary>
    /// Проверка, что статус задан верно (т. е. подлежит интерпретации).
    /// </summary>
    public bool VerifyStatus()
    {
        var status = Status;
        var result = false;
        if (!string.IsNullOrEmpty (status)
            && status.Length == 1)
        {
            var code = status[0];
            result = code is '0' or '1' or '2' or '3' or '4'
                or '5' or '6' or '8' or '9' or 'u' or 'U'
                or 'c' or 'C' or 'p' or 'P' or 'e' or 'r' or 'R';
        }

        return result;
    }

    /// <summary>
    /// Проверка, что статус задан верно (т. е. подлежит интерпретации).
    /// </summary>
    public bool VerifyStatus
        (
            bool throwOnError
        )
    {
        var result = VerifyStatus();

        if (!result && throwOnError)
        {
            var message = $"Bad status '{Status.ToVisibleString()}' for exemplar '{Number}'";
            throw new VerificationException (message);
        }

        return result;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

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
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WritePackedInt32 (Id)
            .WriteNullable (Status)
            .WriteNullable (Number)
            .WriteNullable (Date)
            .WriteNullable (Place)
            .WriteNullable (Collection)
            .WriteNullable (ShelfIndex)
            .WriteNullable (Price)
            .WriteNullable (Barcode)
            .WriteNullable (Amount)
            .WriteNullable (Purpose)
            .WriteNullable (Coefficient)
            .WriteNullable (OffBalance)
            .WriteNullable (KsuNumber1)
            .WriteNullable (ActNumber1)
            .WriteNullable (Channel)
            .WriteNullable (OnHand)
            .WriteNullable (ActNumber2)
            .WriteNullable (WriteOff)
            .WriteNullable (Completion)
            .WriteNullable (ActNumber3)
            .WriteNullable (Moving)
            .WriteNullable (NewPlace)
            .WriteNullable (CheckedDate)
            .WriteNullable (CheckedAmount)
            .WriteNullable (RealPlace)
            .WriteNullable (BindingIndex)
            .WriteNullable (BindingNumber)
            .WriteNullable (Year)
            .WriteNullable (Description)
            .WriteNullable (Bbk)
            .WriteNullable (Issue)
            .WriteNullable (OrderingData);
        writer.Write (Mfn);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ExemplarInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Status)
            .Assert (Status.IsOneOf (ExemplarStatus.ListValues()))
            .NotNullNorEmpty (Place);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var result = $"{Number.ToVisibleString()} ({Place.ToVisibleString()}) [{Status.ToVisibleString()}]";

        if (!string.IsNullOrEmpty (BindingNumber))
        {
            result = result + " <binding " + BindingNumber + ">";
        }

        return result;
    }

    #endregion
}
