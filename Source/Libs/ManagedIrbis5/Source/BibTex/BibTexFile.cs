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

/* BibTexFile.cs -- файл с BibTex-записями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.BibTex;

//
// BibTeX использует bib-файлы специального текстового формата
// для хранения списков библиографических записей. Каждая запись
// описывает ровно одну публикацию - статью, книгу, диссертацию,
// и т. д.
//

/// <summary>
/// Файл с BibTex-записями.
/// </summary>
[XmlRoot ("bibtex")]
public sealed class BibTexFile
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Записи.
    /// </summary>
    [XmlElement ("record")]
    [JsonPropertyName ("records")]
    [Description ("Записи")]
    public List<BibTexRecord> Records { get; } = new ();

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
    /// Нужно ли сериализовать свойство <see cref="Records"/>?
    /// </summary>
    [ExcludeFromCodeCoverage]
    [EditorBrowsable (EditorBrowsableState.Never)]
    public bool ShouldSerializeFields()
    {
        return Records.Count != 0;
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

        reader.ReadList (Records);
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteList (Records);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BibTexFile> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Records);

        foreach (var record in Records)
        {
            verifier.VerifySubObject (record);
        }

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"Records: {Records.Count}";
    }

    #endregion
}
