// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* BiblioDocument.cs -- формируемый документ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using AM;
using AM.Json;

using JetBrains.Annotations;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Формируемый документ.
/// </summary>
[PublicAPI]
public class BiblioDocument
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Главы документа.
    /// </summary>
    [JsonProperty ("chapters")]
    public ChapterCollection Chapters { get; private set; }

    /// <summary>
    /// Common settings.
    /// </summary>
    [JsonIgnore]
    public JObject CommonSettings { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public BiblioDocument()
    {
        Chapters = new (null);
        CommonSettings = new ();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение словарей.
    /// </summary>
    public void BuildDictionaries
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin build dictionaries");

        foreach (var chapter in Chapters)
        {
            if (chapter.IsActive)
            {
                chapter.BuildDictionary (context);
            }
        }

        log.WriteLine("End build dictionaries");
    }

    /// <summary>
    /// Построение элементов, из которых состоит документ
    /// (проще говоря, построение глав).
    /// </summary>
    public void BuildItems
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin build items");

        foreach (var chapter in Chapters)
        {
            if (chapter.IsActive)
            {
                chapter.BuildItems (context);
            }
        }

        log.WriteLine("End build items");
    }

    /// <summary>
    /// Сбор библиографических записей.
    /// </summary>
    public virtual void GatherRecords
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin gather records");

        foreach (var chapter in Chapters)
        {
            if (chapter.IsActive)
            {
                chapter.GatherRecords (context);
            }
        }

        log.WriteLine ("End gather records");
    }

    /// <summary>
    /// Сбор терминов для словарей.
    /// </summary>
    public virtual void GatherTerms
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin gather terms");

        foreach (var chapter in Chapters)
        {
            if (chapter.IsActive)
            {
                chapter.GatherTerms (context);
            }
        }

        log.WriteLine ("End gather terms");
    }

    /// <summary>
    /// Инициализация документа.
    /// </summary>
    public virtual void Initialize
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin initialize the document");

        foreach (var chapter in Chapters)
        {
            // Give the chapter a chance
            chapter.Initialize (context);
        }

        log.WriteLine ("End initialize the document");
    }

    /// <summary>
    /// Загрузка описания документа из указанного файла.
    /// </summary>
    public static BiblioDocument LoadFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var contents = File.ReadAllText
            (
                fileName,
                IrbisEncoding.Utf8
            );
        var obj = JObject.Parse (contents);

        NewtonsoftUtility.ExpandTypes
            (
                obj,
                "ManagedIrbis.Biblio",
                "ManagedIrbis5"
            );

        var serializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        var result = obj.ToObject<BiblioDocument> (serializer)
            .ThrowIfNull();
        var common = (JObject?) obj.SelectToken ("$.common")
                     ?? new JObject();
        result.CommonSettings = common;

        return result;
    }

    /// <summary>
    /// Нумерация/перенумерация элементов.
    /// </summary>
    public virtual void NumberItems
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin number items");
        context.ItemCount = 0;

        foreach (var chapter in Chapters)
        {
            if (chapter.IsActive)
            {
                chapter.NumberItems (context);
            }
        }

        log.WriteLine ("Total items: {0}", context.ItemCount);
        log.WriteLine ("End number items");
    }

    /// <summary>
    /// Рендер элементов документа.
    /// </summary>
    public virtual void RenderItems
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin render items");

        foreach (var chapter in Chapters)
        {
            if (chapter.IsActive)
            {
                chapter.Render (context);
            }
        }

        log.WriteLine ("End render items");
    }

    #endregion

    #region IVerifiable

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<BiblioDocument> (this, throwOnError);

        verifier
            .VerifySubObject (Chapters);

        return verifier.Result;
    }

    #endregion
}
