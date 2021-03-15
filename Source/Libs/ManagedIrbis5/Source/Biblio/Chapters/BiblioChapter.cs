// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BiblioChapter.cs -- глава библиографического указателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Reports;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    /// Базовый класс для глав библиографического указателя.
    /// </summary>
    public class BiblioChapter
        : IAttributable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Whether the chapter is active?
        /// </summary>
        [JsonPropertyName("active")]
        public bool Active { get; set; }

        /// <inheritdoc cref="IAttributable.Attributes" />
        [JsonPropertyName("attr")]
        public ReportAttributes Attributes { get; private set; }

        /// <summary>
        /// Children chapters.
        /// </summary>
        [JsonPropertyName("children")]
        public ChapterCollection Children { get; private set; }

        /// <summary>
        /// Title of the chapter.
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Parent chapter (if any).
        /// </summary>
        [JsonIgnore]
        public BiblioChapter? Parent { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        public ItemCollection Items { get; protected internal set; }

        /// <summary>
        /// Whether the chapter is for service purpose?
        /// </summary>
        public virtual bool IsServiceChapter { get { return false; } }

        /// <summary>
        /// Special settings associated with the chapter
        /// and its children.
        /// </summary>
        [JsonPropertyName("settings")]
        public SpecialSettings? Settings { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BiblioChapter()
        {
            Active = true;
            Attributes = new ReportAttributes();
            Children = new ChapterCollection(this);
            Settings = new SpecialSettings();
        }

        #endregion

        #region Private members

        /// <summary>
        /// Очищаем ключ упорядочивания записи.
        /// </summary>
        protected internal string CleanOrder
            (
                string text
            )
        {
            StringBuilder result = new StringBuilder(text);
            result.Replace("[", string.Empty);
            result.Replace("]", string.Empty);
            result.Replace("\"", string.Empty);
            result.Replace("«", string.Empty);
            result.Replace("»", string.Empty);
            result.Replace("...", string.Empty);

            return result.ToString();
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        protected TResult? GetProperty<TChapter, TResult>
            (
                Func<TChapter, TResult> func
            )
            where TChapter : BiblioChapter
        {
            BiblioChapter chapter = this;
            while (!ReferenceEquals(chapter, null))
            {
                TChapter subChapter = chapter as TChapter;
                if (!ReferenceEquals(subChapter, null))
                {
                    TResult result = func(subChapter);
                    if (!ReferenceEquals(result, null))
                    {
                        return result;
                    }
                }

                chapter = chapter.Parent;
            }

            return default(TResult);
        }

        /// <summary>
        /// Render children chapters.
        /// </summary>
        protected virtual void RenderChildren
            (
                BiblioContext context
            )
        {
            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.Render(context);
                }
            }
        }

        /// <summary>
        /// Render the chapter title.
        /// </summary>
        protected virtual void RenderTitle
            (
                BiblioContext context
            )
        {
            BiblioProcessor processor = context.Processor
                .ThrowIfNull("context.Processor");
            IrbisReport report = processor.Report
                .ThrowIfNull("processor.Report");

            if (!string.IsNullOrEmpty(Title))
            {
                ReportBand title = new ParagraphBand
                {
                    StyleSpecification = @"\s1\plain\f1\fs40\sb400\sa400\b "
                };
                report.Body.Add(title);
                title.Cells.Add(new SimpleTextCell(Title));
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Build dictionaries.
        /// </summary>
        public virtual void BuildDictionary
            (
                BiblioContext context
            )
        {
            /*

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build dictionaries {0}", this);

            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.BuildDictionary(context);
                }
            }

            log.WriteLine("End build dictionaries {0}", this);

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Build <see cref="BiblioItem"/>s.
        /// </summary>
        public virtual void BuildItems
            (
                BiblioContext context
            )
        {
            /*

            AbstractOutput log = context.Log;
            log.WriteLine("Begin build items {0}", this);

            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.BuildItems(context);
                }
            }

            log.WriteLine("End build items {0}", this);

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Clean gathered records.
        /// </summary>
        public virtual void CleanRecords
            (
                BiblioContext context,
                RecordCollection records
            )
        {
            /*

            BiblioDocument document = context.Document
                .ThrowIfNull("context.Document");
            JArray array = (JArray)document.CommonSettings
                .SelectToken("$.removeTags");
            int[] tags = new int[0];
            if (!ReferenceEquals(array, null))
            {
                tags = array.ToObject<int[]>();
            }
            if (tags.Length == 0)
            {
                return;
            }

            foreach (MarcRecord record in records)
            {
                foreach (int tag in tags)
                {
                    record.RemoveField(tag);
                }
            }

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gather terms.
        /// </summary>
        public virtual void GatherTerms
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather terms {0}", this);

            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.GatherTerms(context);
                }
            }

            log.WriteLine("End gather terms {0}", this);
        }

        /// <summary>
        /// Gather records.
        /// </summary>
        public virtual void GatherRecords
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin gather records {0}", this);

            foreach (BiblioChapter child in Children)
            {
                if (child.Active)
                {
                    child.GatherRecords(context);
                }
            }

            log.WriteLine("End gather records {0}", this);
        }

        /// <summary>
        /// Initialize the chapter.
        /// </summary>
        public virtual void Initialize
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin initialize {0}", this);

            foreach (BiblioChapter child in Children)
            {
                // Give the chapter a chance
                child.Initialize(context);
            }

            log.WriteLine("End initialize {0}", this);
        }

        /// <summary>
        /// Number items.
        /// </summary>
        public virtual void NumberItems
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            ItemCollection items = Items;

            if (!ReferenceEquals(items, null))
            {
                foreach (BiblioItem item in items)
                {
                    item.Number = ++context.ItemCount;
                }
            }

            foreach (BiblioChapter child in Children)
            {
                child.NumberItems(context);
            }

        }

        /// <summary>
        /// Render the chapter.
        /// </summary>
        public virtual void Render
            (
                [NotNull] BiblioContext context
            )
        {
            Code.NotNull(context, "context");

            AbstractOutput log = context.Log;
            log.WriteLine("Begin render items {0}", this);

            RenderTitle(context);
            RenderChildren(context);

            log.WriteLine("End render items {0}", this);
        }

        /// <summary>
        /// Walk over the chapter and its children.
        /// </summary>
        public void Walk
            (
                [NotNull] Action<BiblioChapter> action
            )
        {
            Code.NotNull(action, "action");

            action(this);
            foreach (BiblioChapter child in Children)
            {
                child.Walk(action);
            }
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public virtual bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<BiblioChapter>(this, throwOnError);

            verifier
                .NotNull(Children, "Children")
                .VerifySubObject(Children, "Children");

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"{GetType().Name}: {Title.ToVisibleString()}";

        #endregion

    } // class BiblioChapter

} // namespace ManagedIrbis.Biblio
