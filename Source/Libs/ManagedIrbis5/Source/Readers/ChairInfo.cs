// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ChairInfo.cs -- кафедра обслуживания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Информация о кафедре обслуживания.
/// </summary>
[Serializable]
[XmlRoot ("chair")]
public sealed class ChairInfo
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Имя меню с кафедрами по умолчанию.
    /// </summary>
    public const string ChairMenu = "kv.mnu";

    /// <summary>
    /// Имя меню с местами хранения по умолчанию.
    /// </summary>
    public const string PlacesMenu = "mhr.mnu";

    #endregion

    #region Properties

    /// <summary>
    /// Код.
    /// </summary>
    [XmlAttribute ("code")]
    [JsonPropertyName ("code")]
    [DisplayName ("Код")]
    [Description ("Условное обозначение, например, АБ")]
    public string? Code { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    [XmlAttribute ("title")]
    [JsonPropertyName ("title")]
    [DisplayName ("Название")]
    [Description ("Название кафедры, например \"Абонемент\"")]
    public string? Title { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ChairInfo()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="code">Код кафедры.</param>
    public ChairInfo
        (
            string code
        )
    {
        Sure.NotNullNorEmpty (code);

        Code = code;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="code">Код кафедры.</param>
    /// <param name="title">Название.</param>
    public ChairInfo
        (
            string code,
            string title
        )
    {
        Sure.NotNull (code);
        Sure.NotNull (title);

        Code = code;
        Title = title;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текста меню-файла.
    /// </summary>
    public static ChairInfo[] Parse
        (
            string? text,
            bool addAllItem
        )
    {
        text ??= string.Empty;

        var result = new List<ChairInfo>();
        var lines = text.SplitLines();
        for (var i = 0; i < lines.Length; i += 2)
        {
            if (lines[i].StartsWith ("*"))
            {
                break;
            }

            var item = new ChairInfo
            {
                Code = lines[i],
                Title = lines[i + 1]
            };
            result.Add (item);
        }

        if (addAllItem)
        {
            result.Add
                (
                    new ChairInfo
                    {
                        Code = "*",
                        Title = "Все подразделения"
                    }
                );
        }

        return result
            .OrderBy (item => item.Code)
            .ToArray();
    }

    /// <summary>
    /// Загрузка перечня кафедр обслуживания с сервера.
    /// </summary>
    public static ChairInfo[] Read
        (
            ISyncProvider connection,
            string fileName = ChairMenu,
            bool addAllItem = true
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (fileName);

        var specification = new FileSpecification
        {
            Path = IrbisPath.MasterFile,
            Database = connection.Database,
            FileName = fileName
        };
        var content = connection.ReadTextFile (specification);

        if (string.IsNullOrEmpty (content))
        {
            Magna.Logger.LogError
                (
                    nameof (ChairInfo) + "::" + nameof (Read)
                    + ": file is missing or empty: {FileName}",
                    fileName
                );

            // throw new IrbisException($"file is missing or empty {fileName}");
        }

        var result = Parse (content, addAllItem);

        return result;
    }

    /// <summary>
    /// Should serialize the <see cref="Title"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    public bool ShouldSerializeTitle() => !string.IsNullOrEmpty (Title);

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteNullable (Code);
        writer.WriteNullable (Title);
    }

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Code = reader.ReadNullableString();
        Title = reader.ReadNullableString();
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ChairInfo> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Code)
            .NotNullNorEmpty (Title);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return string.IsNullOrEmpty (Title)
            ? Code.ToVisibleString()
            : $"{Code.ToVisibleString()} - {Title.ToVisibleString()}";
    }

    #endregion
}
