// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BiblioDocument.cs -- формируемый документ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using Newtonsoft.Json;

using AM;
using AM.Json;

using ManagedIrbis.Infrastructure;

using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio;

/// <summary>
/// Формируемый документ.
/// </summary>
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
        Chapters = new ChapterCollection (null);
        CommonSettings = new ();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Build dictionaries.
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
            if (chapter.Active)
            {
                chapter.BuildDictionary (context);
            }
        }

        log.WriteLine("End build dictionaries");
    }

    /// <summary>
    /// Build items.
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
            if (chapter.Active)
            {
                chapter.BuildItems (context);
            }
        }

        log.WriteLine("End build items");
    }

    /// <summary>
    /// Gather records.
    /// </summary>
    public virtual void GatherRecords
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin gather records");

        foreach (BiblioChapter chapter in Chapters)
        {
            if (chapter.Active)
            {
                chapter.GatherRecords (context);
            }
        }

        log.WriteLine ("End gather records");
    }

    /// <summary>
    /// Gather terms.
    /// </summary>
    public virtual void GatherTerms
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin gather terms");

        foreach (BiblioChapter chapter in Chapters)
        {
            if (chapter.Active)
            {
                chapter.GatherTerms (context);
            }
        }

        log.WriteLine ("End gather terms");
    }

    /// <summary>
    /// Initialize the document.
    /// </summary>
    public virtual void Initialize
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin initialize the document");

        foreach (BiblioChapter chapter in Chapters)
        {
            // Give the chapter a chance
            chapter.Initialize (context);
        }

        log.WriteLine ("End initialize the document");
    }

    /// <summary>
    /// Load the file.
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
                "ManagedIrbis"
            );

        // File.WriteAllText("_dump.json", obj.ToString());

        var serializer = new JsonSerializer
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };

        var result = obj.ToObject<BiblioDocument> (serializer)
            .ThrowIfNull();
        var common = (JObject?) obj.SelectToken ("$.common")
                     ?? new JObject();
        result.CommonSettings = common;

        return result;
    }

    /// <summary>
    /// Number items.
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

        foreach (BiblioChapter chapter in Chapters)
        {
            if (chapter.Active)
            {
                chapter.NumberItems (context);
            }
        }

        log.WriteLine ("Total items: {0}", context.ItemCount);
        log.WriteLine ("End number items");
    }

    /// <summary>
    /// Render items.
    /// </summary>
    public virtual void RenderItems
        (
            BiblioContext context
        )
    {
        Sure.NotNull (context);

        var log = context.Log;
        log.WriteLine ("Begin render items");

        foreach (BiblioChapter chapter in Chapters)
        {
            if (chapter.Active)
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
