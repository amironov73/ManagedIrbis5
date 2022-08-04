// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MagazineInfo.cs -- информация о журнале в целом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Информация о журнале в целом.
/// </summary>
[XmlRoot ("magazine")]
public sealed class MagazineInfo
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Code for magazine kind.
    /// </summary>
    public const string MagazineKindCode = "a";

    /// <summary>
    /// Code for newspaper kind.
    /// </summary>
    public const string NewspaperKindCode = "c";

    #endregion

    #region Properties

    /// <summary>
    /// Код документа в базе. Поле 903.
    /// </summary>
    [XmlAttribute ("index")]
    [JsonPropertyName ("index")]
    [DisplayName ("Шифр в базе")]
    public string? Index { get; set; }

    /// <summary>
    /// Библиографическое описание.
    /// </summary>
    [XmlAttribute ("description")]
    [JsonPropertyName ("description")]
    [DisplayName ("Описание")]
    public string? Description { get; set; }

    /// <summary>
    /// Заглавие. Поле 200^a.
    /// </summary>
    [XmlAttribute ("title")]
    [JsonPropertyName ("title")]
    [DisplayName ("Заглавие")]
    public string? Title { get; set; }

    /// <summary>
    /// Подзаголовочные сведения.
    /// Поле 200^e.
    /// </summary>
    [XmlAttribute ("sub-title")]
    [JsonPropertyName ("sub-title")]
    [DisplayName ("Подзаголовочные сведения")]
    public string? SubTitle { get; set; }

    /// <summary>
    /// Обозначение и выпуск серии.
    /// Поле 923^1.
    /// </summary>
    [XmlAttribute ("series-number")]
    [JsonPropertyName ("series-number")]
    [DisplayName ("Выпуск серии")]
    public string? SeriesNumber { get; set; }

    /// <summary>
    /// Заголовок серии.
    /// Поле 923^i.
    /// </summary>
    [XmlAttribute ("series-title")]
    [JsonPropertyName ("series-title")]
    [DisplayName ("Серия")]
    public string? SeriesTitle { get; set; }

    /// <summary>
    /// Расширенное заглавие.
    /// Включает заголовок выпуск и заголовок серии.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public string ExtendedTitle
    {
        get
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.Append (Title);
            if (!string.IsNullOrEmpty (SeriesNumber))
            {
                builder.Append (' ');
                builder.Append (SeriesNumber);
            }

            if (!string.IsNullOrEmpty (SeriesTitle))
            {
                builder.Append (' ');
                builder.Append (SeriesTitle);
            }

            if (!string.IsNullOrEmpty (SubTitle))
            {
                builder.Append (':');
                builder.Append (' ');
                builder.Append (SubTitle);
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }
    }

    /// <summary>
    /// Тип издания. Поле 110^t
    /// </summary>
    [XmlAttribute ("magazine-type")]
    [JsonPropertyName ("magazine-type")]
    [DisplayName ("Тип")]
    public string? MagazineType { get; set; }

    /// <summary>
    /// Вид издания. Поле 110^b
    /// </summary>
    /// <remarks>
    /// Журнал = 'a'
    /// Газета = 'c'
    /// </remarks>
    [XmlAttribute ("magazine-kind")]
    [JsonPropertyName ("magazine-kind")]
    [DisplayName ("Вид")]
    public string? MagazineKind { get; set; }

    /// <summary>
    /// Периодичность (число). Поле 110^x
    /// </summary>
    [XmlAttribute ("periodicity")]
    [JsonPropertyName ("periodicity")]
    [DisplayName ("Периодичность (число)")]
    public string? Periodicity { get; set; }

    /// <summary>
    /// Кумуляция. Поле 909
    /// </summary>
    [XmlElement ("cumulation")]
    [JsonPropertyName ("cumulation")]
    [DisplayName ("Кумуляция")]
    public MagazineCumulation[]? Cumulation { get; set; }

    /// <summary>
    /// Сведения о заказах. Поле 901.
    /// </summary>
    [XmlElement ("order")]
    [JsonPropertyName ("orders")]
    [DisplayName ("Сведения о заказах")]
    public OrderedExemplarInfo[]? OrdererExemplars { get; set; }

    /// <summary>
    /// Сведения о заказах (поквартальные). Поле 938.
    /// </summary>
    [XmlElement ("quarterly")]
    [JsonPropertyName ("quarterly")]
    [DisplayName ("Сведения о заказах (поквартальные)")]
    public QuarterlyOrderInfo[]? QuarterlyOrders { get; set; }

    /// <summary>
    /// MFN записи журнала.
    /// </summary>
    [XmlElement ("mfn")]
    [JsonPropertyName ("mfn")]
    [DisplayName ("MFN")]
    public int Mfn { get; set; }

    /// <summary>
    /// Is newspapaper?
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public bool IsNewspaper => MagazineKind.SameString (NewspaperKindCode);

    /// <summary>
    /// Record.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public Record? Record { get; set; }

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
    /// Формирование шифра документа по свойству
    /// <see cref="Index"/>, году и номеру.
    /// </summary>
    public string BuildIssueIndex
        (
            string year,
            string number
        )
    {
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (number);

        return Index.ThrowIfNullOrEmpty() + "/" + year + "/" + number;
    }

    /// <summary>
    /// Формирование шифра документа по свойству
    /// <see cref="Index"/>, году, тому и номеру.
    /// </summary>
    public string BuildIssueIndex
        (
            string year,
            string volume,
            string number
        )
    {
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (volume);
        Sure.NotNullNorEmpty (number);

        return Index.ThrowIfNullOrEmpty() + "/" + year + "/" + volume + "/" + number;
    }

    /// <summary>
    /// Формирование шифра документа по свойству
    /// <see cref="Index"/>, году, тому и номеру.
    /// </summary>
    public string BuildIssueIndex
        (
            YearVolumeNumber yearVolumeNumber
        )
    {
        Sure.VerifyNotNull (yearVolumeNumber);

        return Index.ThrowIfNullOrEmpty() + "/" + yearVolumeNumber;
    }

    /// <summary>
    /// Разбор указанной библиографической записи.
    /// </summary>
    public static MagazineInfo? Parse
        (
            Record record
        )
    {
        Sure.VerifyNotNull (record);

        // TODO: реализовать оптимально

        var result = new MagazineInfo
        {
            Index = record.FM (903),
            Title = record.FM (200, 'a'),
            SubTitle = record.FM (200, 'e'),
            Cumulation = MagazineCumulation.ParseRecord (record),
            OrdererExemplars = OrderedExemplarInfo.ParseRecord (record),
            QuarterlyOrders = QuarterlyOrderInfo.ParseRecord (record),
            SeriesNumber = record.FM (923, 'h'),
            SeriesTitle = record.FM (923, 'i'),
            MagazineType = record.FM (110, 't'),
            MagazineKind = record.FM (110, 'b'),
            Periodicity = record.FM (110, 'x'),
            Record = record,
            Mfn = record.Mfn
        };

        if (string.IsNullOrEmpty (result.Title)
            || string.IsNullOrEmpty (result.Index)
            //|| string.IsNullOrEmpty(result.MagazineKind)
            //|| string.IsNullOrEmpty(result.MagazineType)
           )
        {
            return null;
        }

        return result;
    }

    /// <summary>
    /// Should serialize the <see cref="Mfn"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeMfn()
    {
        return Mfn != 0;
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Index = reader.ReadNullableString();
        Description = reader.ReadNullableString();
        Title = reader.ReadNullableString();
        SubTitle = reader.ReadNullableString();
        SeriesNumber = reader.ReadNullableString();
        SeriesTitle = reader.ReadNullableString();
        MagazineType = reader.ReadNullableString();
        MagazineKind = reader.ReadNullableString();
        Periodicity = reader.ReadNullableString();
        Mfn = reader.ReadPackedInt32();
        QuarterlyOrders = reader.ReadNullableArray<QuarterlyOrderInfo>();

        // TODO Handle Cumulation array
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Index)
            .WriteNullable (Description)
            .WriteNullable (Title)
            .WriteNullable (SubTitle)
            .WriteNullable (SeriesNumber)
            .WriteNullable (SeriesTitle)
            .WriteNullable (MagazineType)
            .WriteNullable (MagazineKind)
            .WriteNullable (Periodicity)
            .WritePackedInt32 (Mfn)
            .WriteNullableArray (QuarterlyOrders);

        // TODO Handle Cumulation array
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    [Pure]
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<MagazineInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Title);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return ExtendedTitle;
    }

    #endregion
}
