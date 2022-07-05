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

/* BindingConfiguration.cs -- конфигурация для менеджера подшивок
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Json;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Конфигурация для менеджера подшивок.
/// </summary>
[XmlRoot ("binding")]
public sealed class BindingConfiguration
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Статусы экземпляров, которые можно подшивать.
    /// </summary>
    [XmlElement ("good-status")]
    [JsonPropertyName ("goodStatus")]
    [DisplayName ("Хорошие статусы")]
    [Description ("Статусы экземпляров, которые можно подшивать")]
    public string[]? GoodStatus { get; set; } = { "0" };

    /// <summary>
    /// Рабочие листы, которые можно подшивать.
    /// </summary>
    [XmlElement ("good-worksheet")]
    [JsonPropertyName ("goodWorksheet")]
    [DisplayName ("Хорошие рабочие листы")]
    [Description ("Рабочие листы, которые можно подшивать")]
    public string[]? GoodWorksheet { get; set; } = { "NJ" };

    /// <summary>
    /// Фонды, которые нельзя подшивать.
    /// </summary>
    [XmlArrayItem ("bad-place")]
    [JsonPropertyName ("badPlace")]
    [DisplayName ("Плохие фонды")]
    [Description ("Фонды, которые нельзя подшивать")]
    public string[]? BadPlace { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка места хранения на возможность добавления в подшивку.
    /// </summary>
    public bool CheckPlace
        (
            string? place
        )
    {
        return !string.IsNullOrEmpty (place)
               && (BadPlace is null || !BadPlace.ContainsNoCase (place));
    }

    /// <summary>
    /// Проверка статуса экземпляра на возможность добавления в подшивку.
    /// </summary>
    public bool CheckStatus
        (
            string? status
        )
    {
        return !string.IsNullOrEmpty (status)
               && (GoodStatus is null || GoodStatus.Contains (status));
    }

    /// <summary>
    /// Проверка рабочего листа на возможность добавления в подшивку.
    /// </summary>
    public bool CheckWorksheet
        (
            string? worksheet
        )
    {
        return !string.IsNullOrEmpty (worksheet)
               && (GoodWorksheet is null || GoodWorksheet.ContainsNoCase (worksheet));
    }

    /// <summary>
    /// Получение конфигурации по умолчанию.
    /// </summary>
    public static BindingConfiguration GetDefault()
    {
        return new ();
    }

    /// <summary>
    /// Чтение конфигурации из указанного файла.
    /// </summary>
    public static BindingConfiguration LoadConfiguration
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        return JsonUtility.ReadObjectFromFile<BindingConfiguration> (fileName);
    }

    /// <summary>
    /// Запись конфигурации в указанный файл.
    /// </summary>
    public void SaveConfiguration
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        JsonUtility.SaveObjectToFile (this, fileName);
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

        GoodStatus = reader.ReadNullableStringArray();
        GoodWorksheet = reader.ReadNullableStringArray();
        BadPlace = reader.ReadNullableStringArray();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullableArray (GoodStatus)
            .WriteNullableArray (GoodWorksheet)
            .WriteNullableArray (BadPlace);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BindingConfiguration> (this, throwOnError);

        verifier
            .NotNullNorEmpty (GoodStatus)
            .NotNullNorEmpty (GoodWorksheet);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("GoodStatus: ");
        builder.AppendEnumerable (GoodStatus);
        builder.Append (", ");

        builder.Append ("GoodWorksheet: ");
        builder.AppendEnumerable (GoodWorksheet);
        builder.Append (", ");

        builder.Append ("BadPlace: ");
        builder.AppendEnumerable (BadPlace);

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
