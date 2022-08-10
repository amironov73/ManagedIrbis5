// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* VisitInfo.cs -- информация о посещении/выдаче
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Информация о посещении/выдаче.
/// </summary>
[XmlRoot ("visit")]
[DebuggerDisplay ("{DateGivenString} {Index} {Description}")]
public sealed class VisitInfo
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Метка поля.
    /// </summary>
    public const int Tag = 40;

    /// <summary>
    /// Коды известных подполей.
    /// </summary>
    public const string KnownCodes = "1249abcdefghikluv";

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
    /// Имя БД каталога, подполе G.
    /// </summary>
    [SubField ('g')]
    [XmlAttribute ("database")]
    [JsonPropertyName ("database")]
    [DisplayName ("База")]
    [Description ("Имя БД каталога")]
    public string? Database { get; set; }

    /// <summary>
    /// Шифр документа, подполе A.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("index")]
    [JsonPropertyName ("index")]
    [DisplayName ("Шифр документа")]
    [Description ("Шифр документа в базе")]
    public string? Index { get; set; }

    /// <summary>
    /// Инвентарный номер экземпляра, подполе B.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("inventory")]
    [JsonPropertyName ("inventory")]
    [DisplayName ("Инвентарный номер")]
    [Description ("Инвентарный номер документа")]
    public string? InventoryNumber { get; set; }

    /// <summary>
    /// Штрих-код экземпляра, подполе H.
    /// </summary>
    [SubField ('h')]
    [XmlAttribute ("barcode")]
    [JsonPropertyName ("barcode")]
    [DisplayName ("Штрих-код")]
    [Description ("Штрих-код экземпляра")]
    public string? Barcode { get; set; }

    /// <summary>
    /// Место хранения экземпляра, подполе K.
    /// </summary>
    [SubField ('k')]
    [XmlAttribute ("sigla")]
    [JsonPropertyName ("sigla")]
    [DisplayName ("Место хранения")]
    [Description ("Место хранения экземпляра (фонд)")]
    public string? Sigla { get; set; }

    /// <summary>
    /// Дата выдачи в формате ГГГГММДД, подполе D.
    /// </summary>
    [SubField ('d')]
    [XmlAttribute ("dateGiven")]
    [JsonPropertyName ("dateGiven")]
    [DisplayName ("Дата выдачи")]
    [Description ("Дата выдачи в формате ГГГГММДД")]
    public string? DateGivenString { get; set; }

    /// <summary>
    /// Место выдачи, подполе V.
    /// </summary>
    [SubField ('v')]
    [XmlAttribute ("department")]
    [JsonPropertyName ("department")]
    [DisplayName ("Место выдачи")]
    [Description ("Место выдачи (кафедра)")]
    public string? Department { get; set; }

    /// <summary>
    /// Дата предполагаемого возврата, подполе E.
    /// </summary>
    [SubField ('e')]
    [XmlAttribute ("dateExpected")]
    [JsonPropertyName ("dateExpected")]
    [DisplayName ("Дата предполагаемого возврата")]
    [Description ("Дата предполагаемого возврата в формате ГГГГММДД")]
    public string? DateExpectedString { get; set; }

    /// <summary>
    /// Дата фактического возврата, подполе F.
    /// </summary>
    [SubField ('f')]
    [XmlAttribute ("dateReturned")]
    [JsonPropertyName ("dateReturned")]
    [DisplayName ("Дата фактического возврата")]
    [Description ("Дата фактического возврата в формате ГГГГММДД")]
    public string? DateReturnedString { get; set; }

    /// <summary>
    /// Дата продления, подполе L.
    /// </summary>
    [SubField ('l')]
    [XmlAttribute ("dateProlong")]
    [JsonPropertyName ("dateProlong")]
    [DisplayName ("Дата продления")]
    [Description ("Дата продления в формате ГГГГММДД")]
    public string? DateProlongString { get; set; }

    /// <summary>
    /// Признак утерянной книги, подполе U.
    /// </summary>
    [SubField ('u')]
    [XmlAttribute ("lost")]
    [JsonPropertyName ("lost")]
    [DisplayName ("Утеряно")]
    [Description ("Признак утерянной книги")]
    public string? Lost { get; set; }

    /// <summary>
    /// Краткое библиографическое описание, подполе C.
    /// </summary>
    [SubField ('c')]
    [XmlAttribute ("description")]
    [JsonPropertyName ("description")]
    [DisplayName ("Описание")]
    [Description ("Краткое библиографическое описание")]
    public string? Description { get; set; }

    /// <summary>
    /// Ответственное лицо, подполе I.
    /// </summary>
    [SubField ('i')]
    [XmlAttribute ("responsible")]
    [JsonPropertyName ("responsible")]
    [DisplayName ("Ответственное лицо")]
    [Description ("Ответственное лицо, оформившее посещение или выдачу")]
    public string? Responsible { get; set; }

    /// <summary>
    /// Время начала визита в библиотеку, подполе 1.
    /// </summary>
    [SubField ('1')]
    [XmlAttribute ("timeIn")]
    [JsonPropertyName ("timeIn")]
    [DisplayName ("Начало")]
    [Description ("Время начала визита в библиотеку")]
    public string? TimeIn { get; set; }

    /// <summary>
    /// Время окончания визита в библиотеку, подполе 2.
    /// </summary>
    [SubField ('2')]
    [XmlAttribute ("timeOut")]
    [JsonPropertyName ("timeOut")]
    [DisplayName ("Окончание")]
    [Description ("Время окончания визита в библиотеку")]
    public string? TimeOut { get; set; }

    /// <summary>
    /// Счетчик продлений, подполе 4.
    /// </summary>
    /// <remarks>
    /// http://irbis.gpntb.ru/read.php?3,105310,108175#msg-108175
    /// </remarks>
    [SubField ('4')]
    [XmlAttribute ("prolong")]
    [JsonPropertyName ("prolong")]
    [DisplayName ("Продления")]
    [Description ("Счетчик продлений")]
    public string? Prolong { get; set; }

    /// <summary>
    /// Не посещение ли?
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public bool IsVisit => string.IsNullOrEmpty (Index);

    /// <summary>
    /// Возвращена ли книга?
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public bool IsReturned => !string.IsNullOrEmpty (DateReturnedString)
                              && !DateReturnedString.StartsWith ("*");

    /// <summary>
    /// Дата возврата
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public DateTime DateReturned => IrbisDate.ConvertStringToDate (DateReturnedString);

    /// <summary>
    /// Дата выдачи/посещения.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public DateTime DateGiven => IrbisDate.ConvertStringToDate (DateGivenString);

    /// <summary>
    /// Ожидаемая дата возврата в формате ГГГГММДД.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public DateTime DateExpected => IrbisDate.ConvertStringToDate (DateExpectedString);

    /// <summary>
    /// Книга просрочена?
    /// </summary>
    [JsonIgnore]
    [Browsable (false)]
    public bool Expired
    {
        get
        {
            var today = IrbisDate.TodayText;
            return string.CompareOrdinal (DateExpectedString, today) < 0;
        }
    }

    /// <summary>
    /// Счетчик продлений.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public int ProlongCount
    {
        get => Prolong.SafeToInt32();
        set => Prolong = value.ToInvariantString();
    }

    /// <summary>
    /// Год издания книги.
    /// </summary>
    [XmlAttribute ("year")]
    [JsonPropertyName ("year")]
    [DisplayName ("Год издания")]
    [Description ("Год издания книги (для вычисления штрафов)")]
    public string? Year { get; set; }

    /// <summary>
    /// Цена книги.
    /// </summary>
    [XmlAttribute ("price")]
    [JsonPropertyName ("price")]
    [DisplayName ("Цена")]
    [Description ("Цена книги (для вычисления штрафов)")]
    public string? Price { get; set; }

    /// <summary>
    /// Примечание в произвольной форме, введенное оператором АРМ "Книговыдача".
    /// Подполе 9 (появилось в 2018.1).
    /// </summary>
    [XmlAttribute ("comment")]
    [JsonPropertyName ("comment")]
    [DisplayName ("Примечания")]
    [Description ("Примечания в произвольной форме")]
    public string? Comment { get; set; }

    /// <summary>
    /// Поле, в котором хранится посещение/выдача.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Field? Field { get; set; }

    /// <summary>
    /// Ссылка на читателя, сделавшего посещение.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public ReaderInfo? Reader { get; set; }

    /// <summary>
    /// Неизвестные подполя.
    /// </summary>
    [XmlElement ("unknown")]
    [JsonPropertyName ("unknown")]
    [Browsable (false)]
    public SubField[]? UnknownSubFields { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public object? UserData { get; set; }

    #endregion

    #region Private members

    private string? FM (char code) => Field?.GetFirstSubFieldValue (code);

    private void _Parse()
    {
        Database = FM ('g');
        Index = FM ('a');
        InventoryNumber = FM ('b');
        Barcode = FM ('h');
        Sigla = FM ('k');
        DateGivenString = FM ('d');
        Department = FM ('v');
        DateExpectedString = FM ('e');
        DateReturnedString = FM ('f');
        DateProlongString = FM ('l');
        Lost = FM ('u');
        Description = FM ('c');
        Responsible = FM ('i');
        TimeIn = FM ('1');
        TimeOut = FM ('2');
        Comment = FM ('9');
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение цены книги.
    /// </summary>
    public string? GetBookPrice
        (
            Record bookRecord
        )
    {
        Sure.NotNull (bookRecord);

        var fields = bookRecord.Fields.GetField (910);
        string? result = null;
        foreach (var field in fields)
        {
            if (!string.IsNullOrEmpty (InventoryNumber))
            {
                var exemplar = ExemplarInfo.ParseField (field);
                if (exemplar.Number.SameString (InventoryNumber))
                {
                    if (!string.IsNullOrEmpty (Barcode))
                    {
                        if (exemplar.Barcode.SameString (Barcode))
                        {
                            result = exemplar.Price;
                            break;
                        }
                    }

                    result = exemplar.Price;
                    break;
                }
            }
        }

        if (string.IsNullOrEmpty (result))
        {
            result = bookRecord.FM (10, 'd');
        }

        return result;
    }

    /// <summary>
    /// Получение года издания для книги.
    /// </summary>
    public static string GetBookYear
        (
            Record bookRecord
        )
    {
        Sure.NotNull (bookRecord);

        var result = Utility.NonEmpty (bookRecord.FM (210, 'd'),
            bookRecord.FM (934));

        return result;
    }

    /// <summary>
    /// Разбор указанного поля библиографической записи,
    /// получение информации о посещении/выдачи.
    /// </summary>
    public static VisitInfo Parse
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var result = new VisitInfo
        {
            Field = field,
            UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes)
        };
        result._Parse();

        return result;
    }

    /// <summary>
    /// Формирование поля 40
    /// из данных о выдаче/посещении.
    /// </summary>
    public Field ToField()
    {
        var result = new Field (Tag)
            .AddNonEmpty ('g', Database)
            .AddNonEmpty ('a', Index)
            .AddNonEmpty ('b', InventoryNumber)
            .AddNonEmpty ('h', Barcode)
            .AddNonEmpty ('k', Sigla)
            .AddNonEmpty ('d', DateGivenString)
            .AddNonEmpty ('v', Department)
            .AddNonEmpty ('e', DateExpectedString)
            .AddNonEmpty ('f', DateReturnedString)
            .AddNonEmpty ('l', DateProlongString)
            .AddNonEmpty ('u', Lost)
            .AddNonEmpty ('c', Description)
            .AddNonEmpty ('i', Responsible)
            .AddNonEmpty ('1', TimeIn)
            .AddNonEmpty ('2', TimeOut)
            .AddNonEmpty ('9', Comment)
            .AddRange (UnknownSubFields);

        return result;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WritePackedInt32 (Id);
        writer.WriteNullable (Database);
        writer.WriteNullable (Index);
        writer.WriteNullable (InventoryNumber);
        writer.WriteNullable (Barcode);
        writer.WriteNullable (Sigla);
        writer.WriteNullable (DateGivenString);
        writer.WriteNullable (Department);
        writer.WriteNullable (DateExpectedString);
        writer.WriteNullable (DateReturnedString);
        writer.WriteNullable (DateProlongString);
        writer.WriteNullable (Lost);
        writer.WriteNullable (Description);
        writer.WriteNullable (Responsible);
        writer.WriteNullable (TimeIn);
        writer.WriteNullable (TimeOut);
        writer.WriteNullable (Prolong);
        writer.WriteNullable (Year);
        writer.WriteNullable (Price);
        writer.WriteNullableArray (UnknownSubFields);
    }

    /// <summary>
    /// Сохранение в поток.
    /// </summary>
    public static void SaveToStream
        (
            BinaryWriter writer,
            VisitInfo[] visits
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull (visits);

        writer.WritePackedInt32 (visits.Length);
        foreach (var visit in visits)
        {
            visit.SaveToStream (writer);
        }
    }

    /// <summary>
    /// Сохранение в файл.
    /// </summary>
    public static void SaveToFile
        (
            string fileName,
            VisitInfo[] visits
        )
    {
        Sure.NotNullNorEmpty (fileName);
        Sure.NotNull (visits);

        visits.SaveToFile (fileName);
    }

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Id = reader.ReadPackedInt32();
        Database = reader.ReadNullableString();
        Index = reader.ReadNullableString();
        InventoryNumber = reader.ReadNullableString();
        Barcode = reader.ReadNullableString();
        Sigla = reader.ReadNullableString();
        DateGivenString = reader.ReadNullableString();
        Department = reader.ReadNullableString();
        DateExpectedString = reader.ReadNullableString();
        DateReturnedString = reader.ReadNullableString();
        DateProlongString = reader.ReadNullableString();
        Lost = reader.ReadNullableString();
        Description = reader.ReadNullableString();
        Responsible = reader.ReadNullableString();
        TimeIn = reader.ReadNullableString();
        TimeOut = reader.ReadNullableString();
        Prolong = reader.ReadNullableString();
        Year = reader.ReadNullableString();
        Price = reader.ReadNullableString();
        UnknownSubFields = reader.ReadNullableArray<SubField>();
    }


    /// <summary>
    /// Считывание из файла.
    /// </summary>
    public static VisitInfo[] ReadFromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var result = SerializationUtility.RestoreArrayFromFile<VisitInfo> (fileName);

        return result;
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<VisitInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (InventoryNumber);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();

        builder
            .Append ($"Посещение: \t\t\t{IsVisit}")
            .AppendLine()
            .Append ($"Описание: \t\t\t{Description.ToVisibleString()}")
            .AppendLine()
            .Append ($"Шифр документа: \t\t{Index.ToVisibleString()}")
            .AppendLine()
            .Append ($"Штрих-код: \t\t\t{Barcode.ToVisibleString()}")
            .AppendLine()
            .Append ($"Место хранения: \t\t{Sigla.ToVisibleString()}")
            .AppendLine()
            .Append ($"Дата выдачи: \t\t\t{DateGiven:d}")
            .AppendLine()
            .Append ($"Место выдачи: \t\t\t{Department.ToVisibleString()}")
            .AppendLine()
            .Append ($"Ответственное лицо: \t\t{Responsible.ToVisibleString()}")
            .AppendLine()
            .Append ($"Дата предполагаемого возврата: \t{DateExpected:d}")
            .AppendLine()
            .Append ($"Возвращена: \t\t\t{IsReturned}")
            .AppendLine()
            .Append ($"Дата возврата: \t\t\t{DateReturned:d}")
            .AppendLine()
            .Append ($"Счетчик продлений: \t\t\t{Prolong.ToVisibleString()}")
            .AppendLine()
            .AppendRepeat ('-', 60)
            .AppendLine();

        return builder.ReturnShared();
    }

    #endregion
}
