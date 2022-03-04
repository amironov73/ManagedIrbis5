// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BeriInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.Serialization;

using AM;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Mapping;
using ManagedIrbis.Readers;

using Newtonsoft.Json;

#endregion

namespace BeriChitai.Data;

public class BeriInfo
{
    #region Constants

    /// <summary>
    /// Известные коды подполей.
    /// </summary>
    public const string KnownCodes = "abcde";

    /// <summary>
    /// Тег полей.
    /// </summary>
    public const int BeriTag = 9190;

    #endregion

    #region Properties

    /// <summary>
    /// Дата бронирования, подполе a.
    /// </summary>
    [SubField ('a')]
    [XmlAttribute ("booking-date")]
    [JsonProperty ("bookingDate")]
    public string? BookingDate { get; set; }

    /// <summary>
    /// Читательский билет, подполе b.
    /// </summary>
    [SubField ('b')]
    [XmlAttribute ("ticket")]
    [JsonProperty ("ticket")]
    public string? Ticket { get; set; }

    /// <summary>
    /// Дата выдачи, подполе c.
    /// </summary>
    [SubField ('c')]
    [XmlAttribute ("issue-date")]
    [JsonProperty ("issueDate")]
    public string? IssueDate { get; set; }

    /// <summary>
    /// Примечания в произвольной форме, подполе d.
    /// </summary>
    [SubField ('d')]
    [XmlAttribute ("notes")]
    [JsonProperty ("notes")]
    public string? Notes { get; set; }

    /// <summary>
    /// Населенный пункт, подполе e.
    /// </summary>
    [SubField ('e')]
    [XmlAttribute ("locality")]
    [JsonProperty ("locality")]
    public string? Locality { get; set; }

    /// <summary>
    /// Шифр в базе. Поле 903.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public string? Index { get; set; }

    /// <summary>
    /// Биб. описание.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public string? Description { get; set; }

    public string ShortDescription
    {
        get
        {
            var date = IrbisDate.ConvertStringToDate (BookingDate)
                .ToString ("dd MMM yyyy");
            var result = $"№ {Index}: {date}";

            return result;
        }
    }

    /// <summary>
    /// Соответствующая запись.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public Record? Record { get; set; }

    /// <summary>
    /// Соответствующее поле записи.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public Field? Field { get; set; }

    /// <summary>
    /// Читатель, заказавший книгу.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public ReaderInfo? Reader { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public object? UserData { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Применение к указанному полю.
    /// </summary>
    public void ApplyToField
        (
            Field field
        )
    {
        Sure.NotNull (field);

        field
            .SetSubFieldValue ('a', BookingDate)
            .SetSubFieldValue ('b', Ticket)
            .SetSubFieldValue ('c', IssueDate)
            .SetSubFieldValue ('d', Notes)
            .SetSubFieldValue ('e', Locality);
    }

    /// <summary>
    /// Разбор поля.
    /// </summary>
    public static BeriInfo Parse
        (
            Field field
        )
    {
        Sure.NotNull (field);

        var result = new BeriInfo
        {
            BookingDate = field.GetFirstSubFieldValue ('a'),
            Ticket = field.GetFirstSubFieldValue ('b'),
            IssueDate = field.GetFirstSubFieldValue ('c'),
            Notes = field.GetFirstSubFieldValue ('d'),
            Locality = field.GetFirstSubFieldValue ('e'),
            Field = field
        };

        return result;
    }

    /// <summary>
    /// Разбор записи.
    /// </summary>
    public static BeriInfo[] Parse
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var result = new List<BeriInfo>();
        foreach (var field in record.Fields.GetField (BeriTag))
        {
            var info = Parse (field);
            info.Record = record;
            info.Index = record.FM (903);
            result.Add (info);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Превращение обратно в поле.
    /// </summary>
    public Field ToField()
    {
        var result = new Field (BeriTag)
            .AddNonEmpty ('a', BookingDate)
            .AddNonEmpty ('b', Ticket)
            .AddNonEmpty ('c', IssueDate)
            .AddNonEmpty ('d', Notes)
            .AddNonEmpty ('e', Locality);

        return result;
    }

    #endregion
}
