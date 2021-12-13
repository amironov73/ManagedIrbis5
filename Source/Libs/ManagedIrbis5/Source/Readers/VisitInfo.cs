// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* VisitInfo.cs -- информация о посещении/выдаче
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Информация о посещении/выдаче.
    /// </summary>
    [XmlRoot("visit")]
    [DebuggerDisplay("{DateGivenString} {Index} {Description}")]
    public sealed class VisitInfo
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 40;

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "1249abcdefghikluv";

        #endregion

        #region Properties

        /// <summary>
        /// Identifier for LiteDB.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public int Id { get; set; }

        /// <summary>
        /// подполе G, имя БД каталога.
        /// </summary>
        [SubField('g')]
        [XmlAttribute("database")]
        [JsonPropertyName("database")]
        public string? Database { get; set; }

        /// <summary>
        /// подполе A, шифр документа.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("index")]
        [JsonPropertyName("index")]
        public string? Index { get; set; }

        /// <summary>
        /// подполе B, инвентарный номер экземпляра
        /// </summary>
        [SubField('b')]
        [XmlAttribute("inventory")]
        [JsonPropertyName("inventory")]
        public string? Inventory { get; set; }

        /// <summary>
        /// подполе H, штрих-код экземпляра.
        /// </summary>
        [SubField('h')]
        [XmlAttribute("barcode")]
        [JsonPropertyName("barcode")]
        public string? Barcode { get; set; }

        /// <summary>
        /// подполе K, место хранения экземпляра
        /// </summary>
        [SubField('k')]
        [XmlAttribute("sigla")]
        [JsonPropertyName("sigla")]
        public string? Sigla { get; set; }

        /// <summary>
        /// подполе D, дата выдачи
        /// </summary>
        [SubField('d')]
        [XmlAttribute("dateGiven")]
        [JsonPropertyName("dateGiven")]
        public string? DateGivenString { get; set; }

        /// <summary>
        /// подполе V, место выдачи
        /// </summary>
        [SubField('v')]
        [XmlAttribute("department")]
        [JsonPropertyName("department")]
        public string? Department { get; set; }

        /// <summary>
        /// подполе E, дата предполагаемого возврата
        /// </summary>
        [SubField('e')]
        [XmlAttribute("dateExpected")]
        [JsonPropertyName("dateExpected")]
        public string? DateExpectedString { get; set; }

        /// <summary>
        /// подполе F, дата фактического возврата
        /// </summary>
        [SubField('f')]
        [XmlAttribute("dateReturned")]
        [JsonPropertyName("dateReturned")]
        public string? DateReturnedString { get; set; }

        /// <summary>
        /// подполе L, дата продления
        /// </summary>
        [SubField('l')]
        [XmlAttribute("dateProlong")]
        [JsonPropertyName("dateProlong")]
        public string? DateProlongString { get; set; }

        /// <summary>
        /// подполе U, признак утерянной книги
        /// </summary>
        [SubField('u')]
        [XmlAttribute("lost")]
        [JsonPropertyName("lost")]
        public string? Lost { get; set; }

        /// <summary>
        /// подполе C, краткое библиографическое описание
        /// </summary>
        [SubField('c')]
        [XmlAttribute("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// подполе I, ответственное лицо
        /// </summary>
        [SubField('i')]
        [XmlAttribute("responsible")]
        [JsonPropertyName("responsible")]
        public string? Responsible { get; set; }

        /// <summary>
        /// подполе 1, время начала визита в библиотеку
        /// </summary>
        [SubField('1')]
        [XmlAttribute("timeIn")]
        [JsonPropertyName("timeIn")]
        public string? TimeIn { get; set; }

        /// <summary>
        /// подполе 2, время окончания визита в библиотеку
        /// </summary>
        [SubField('2')]
        [XmlAttribute("timeOut")]
        [JsonPropertyName("timeOut")]
        public string? TimeOut { get; set; }

        /// <summary>
        /// Счетчик продлений.
        /// </summary>
        /// <remarks>
        /// http://irbis.gpntb.ru/read.php?3,105310,108175#msg-108175
        /// </remarks>
        [SubField('4')]
        [XmlAttribute("prolong")]
        [JsonPropertyName("prolong")]
        public string? Prolong { get; set; }

        /// <summary>
        /// Не посещение ли?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsVisit => string.IsNullOrEmpty(Index);

        /// <summary>
        /// Возвращена ли книга?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsReturned
        {
            get
            {
                if (string.IsNullOrEmpty(DateReturnedString))
                {
                    return false;
                }

                return !DateReturnedString.StartsWith("*");
            }
        }

        /// <summary>
        /// Дата возврата
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateReturned => IrbisDate.ConvertStringToDate(DateReturnedString);

        /// <summary>
        /// Дата выдачи/посещения.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateGiven => IrbisDate.ConvertStringToDate(DateGivenString);

        /// <summary>
        /// Ожидаемая дата возврата
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public DateTime DateExpected => IrbisDate.ConvertStringToDate(DateExpectedString);

        /// <summary>
        /// Книга просрочена?
        /// </summary>
        [JsonIgnore]
        public bool Expired
        {
            get
            {
                var today = IrbisDate.TodayText;
                return string.CompareOrdinal(DateExpectedString, today) < 0;
            }
        }

        /// <summary>
        /// Счетчик продлений.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public int ProlongCount
        {
            get => Prolong.SafeToInt32();
            set => Prolong = value.ToInvariantString();
        }

        /// <summary>
        /// Год издания книги.
        /// </summary>
        [XmlAttribute("year")]
        [JsonPropertyName("year")]
        public string? Year { get; set; }

        /// <summary>
        /// Цена книги.
        /// </summary>
        [XmlAttribute("price")]
        [JsonPropertyName("price")]
        public string? Price { get; set; }

        /// <summary>
        /// Примечание в произвольной форме, введенное оператором АРМ "Книговыдача".
        /// Подполе 9 (появилось в 2018.1).
        /// </summary>
        [XmlAttribute("comment")]
        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        /// <summary>
        /// Поле, в котором хранится посещение/выдача.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public Field? Field { get; set; }

        /// <summary>
        /// Ссылка на читателя, сделавшего посещение.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public ReaderInfo? Reader { get; set; }

        /// <summary>
        /// Unknown subfields.
        /// </summary>
        [XmlElement("unknown")]
        [JsonPropertyName("unknown")]
        [Browsable(false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Private members

        // ReSharper disable once InconsistentNaming
        private string? FM ( char code ) => Field?.GetFirstSubFieldValue(code);

        private void _Parse()
        {
            Database = FM('g');
            Index = FM('a');
            Inventory = FM('b');
            Barcode = FM('h');
            Sigla = FM('k');
            DateGivenString = FM('d');
            Department = FM('v');
            DateExpectedString = FM('e');
            DateReturnedString = FM('f');
            DateProlongString = FM('l');
            Lost = FM('u');
            Description = FM('c');
            Responsible = FM('i');
            TimeIn = FM('1');
            TimeOut = FM('2');
            Comment = FM('9');
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get price for the book.
        /// </summary>
        public string? GetBookPrice
            (
                Record bookRecord
            )
        {
            var fields = bookRecord.Fields
                .GetField(910);

            string? result = null;

            foreach (var field in fields)
            {
                var exemplar = ExemplarInfo.ParseField(field);

                if (!string.IsNullOrEmpty(Inventory))
                {
                    if (exemplar.Number.SameString(Inventory))
                    {
                        if (!string.IsNullOrEmpty(Barcode))
                        {
                            if (exemplar.Barcode.SameString(Barcode))
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

            if (string.IsNullOrEmpty(result))
            {
                result = bookRecord.FM(10, 'd');
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
            var result = Utility.NonEmpty(bookRecord.FM(210, 'd'),
                         bookRecord.FM(934));

            return result;
        }

        /// <summary>
        /// Parses the specified field.
        /// </summary>
        public static VisitInfo Parse
            (
                Field field
            )
        {
            // TODO Support for unknown subfields

            var result = new VisitInfo
            {
                Field = field,
                UnknownSubFields = field.Subfields.GetUnknownSubFields(KnownCodes)
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
                .AddNonEmpty ('b', Inventory)
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

        } // method ToField

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.WritePackedInt32(Id);
            writer.WriteNullable(Database);
            writer.WriteNullable(Index);
            writer.WriteNullable(Inventory);
            writer.WriteNullable(Barcode);
            writer.WriteNullable(Sigla);
            writer.WriteNullable(DateGivenString);
            writer.WriteNullable(Department);
            writer.WriteNullable(DateExpectedString);
            writer.WriteNullable(DateReturnedString);
            writer.WriteNullable(DateProlongString);
            writer.WriteNullable(Lost);
            writer.WriteNullable(Description);
            writer.WriteNullable(Responsible);
            writer.WriteNullable(TimeIn);
            writer.WriteNullable(TimeOut);
            writer.WriteNullable(Prolong);
            writer.WriteNullable(Year);
            writer.WriteNullable(Price);
            writer.WriteNullableArray(UnknownSubFields);
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
            writer.WritePackedInt32(visits.Length);
            foreach (var visit in visits)
            {
                visit.SaveToStream(writer);
            }
        }

        /// <summary>
        /// Сохранение в файл.
        /// </summary>
        public static void SaveToFile (string fileName, VisitInfo[] visits)
            => visits.SaveToFile(fileName);

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Id = reader.ReadPackedInt32();
            Database = reader.ReadNullableString();
            Index = reader.ReadNullableString();
            Inventory = reader.ReadNullableString();
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
            VisitInfo[] result = SerializationUtility
                .RestoreArrayFromFile<VisitInfo>(fileName);

            return result;
        }


        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result
                .AppendFormat("Посещение: \t\t\t{0}", IsVisit)
                .AppendLine()
                .AppendFormat("Описание: \t\t\t{0}", Description.ToVisibleString())
                .AppendLine()
                .AppendFormat("Шифр документа: \t\t{0}", Index.ToVisibleString())
                .AppendLine()
                .AppendFormat("Штрих-код: \t\t\t{0}", Barcode.ToVisibleString())
                .AppendLine()
                .AppendFormat("Место хранения: \t\t{0}", Sigla.ToVisibleString())
                .AppendLine()
                .AppendFormat("Дата выдачи: \t\t\t{0:d}", DateGiven)
                .AppendLine()
                .AppendFormat("Место выдачи: \t\t\t{0}", Department.ToVisibleString())
                .AppendLine()
                .AppendFormat("Ответственное лицо: \t\t{0}", Responsible.ToVisibleString())
                .AppendLine()
                .AppendFormat("Дата предполагаемого возврата: \t{0:d}", DateExpected)
                .AppendLine()
                .AppendFormat("Возвращена: \t\t\t{0}", IsReturned)
                .AppendLine()
                .AppendFormat("Дата возврата: \t\t\t{0:d}", DateReturned)
                .AppendLine()
                .AppendFormat("Счетчик продлений: \t\t\t{0}", Prolong.ToVisibleString())
                .AppendLine()
                .AppendLine(new string('-', 60));

            return result.ToString();
        }

        #endregion
    }
}
