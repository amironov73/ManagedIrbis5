// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MagazineArticleInfo.cs -- информация о статье из журнала
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Информация о статье из журнала/сборника.
    /// Рабочий лист ASP.
    /// </summary>
    [XmlRoot("article")]
    public sealed class MagazineArticleInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Авторы, поля 70x и 710x.
        /// </summary>
        [XmlElement("author")]
        [Description("Авторы")]
        [DisplayName("Авторы")]
        [JsonPropertyName("authors")]
        public AuthorInfo[]? Authors { get; set; }

        /// <summary>
        /// Заглавие, поле 200.
        /// </summary>
        [XmlElement("title")]
        [Description("Заглавие")]
        [DisplayName("Заглавие")]
        [JsonPropertyName("title")]
        public TitleInfo? Title { get; set; }

        /// <summary>
        /// Издание, в котором опубликована статья, поле 463.
        /// </summary>
        [XmlElement("source")]
        [Description("Издание, в котором опубликована статья")]
        [DisplayName("Издание, в котором опубликована статья")]
        [JsonPropertyName("sources")]
        public SourceInfo[]? Sources { get; set; }

        #endregion

        #region Private members

        private static int[] _authorTags = {700, 701, 702};

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ASP-записи.
        /// </summary>
        public static MagazineArticleInfo ParseAsp
            (
                Record record
            )
        {
            var result = new MagazineArticleInfo();
            result.Authors = AuthorInfo.ParseRecord(record, _authorTags);
            var field200 = record.Fields.GetFirstField(200);
            if (!ReferenceEquals(field200, null))
            {
                result.Title = TitleInfo.ParseField200(field200);
            }
            result.Sources = SourceInfo.ParseRecord(record);

            return result;
        }

        /// <summary>
        /// Разбор NJ-записи.
        /// </summary>
        public static MagazineArticleInfo[] ParseIssue
            (
                Record record
            )
        {
            var result = new List<MagazineArticleInfo>();
            foreach (var field in record.Fields.GetField(922))
            {
                MagazineArticleInfo article = ParseField330(field);
                result.Add(article);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор PAZK/SPEC-записи.
        /// </summary>
        public static MagazineArticleInfo[] ParseBook
            (
                Record record
            )
        {
            var result = new List<MagazineArticleInfo>();
            foreach (var field in record.Fields.GetField(330))
            {
                MagazineArticleInfo article = ParseField330(field);
                result.Add(article);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Разбор поля (330 или 922).
        /// </summary>
        public static MagazineArticleInfo ParseField330
            (
                Field field
            )
        {
            var result = new MagazineArticleInfo
            {
                Authors = AuthorInfo.ParseField330(field),
                Title = TitleInfo.ParseField330(field)
            };

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        public Field ToField()
        {
            // TODO implement

            throw new NotImplementedException();
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Authors = reader.ReadNullableArray<AuthorInfo>();
            Title = reader.RestoreNullable<TitleInfo>();
            Sources = reader.ReadNullableArray<SourceInfo>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullableArray(Authors)
                .WriteNullable(Title)
                .WriteNullableArray(Sources);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<MagazineArticleInfo>(this, throwOnError);

            verifier
                .NotNull(Title, "Title");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => Title?.ToString() ?? string.Empty;

        #endregion

    } // class MagazineArticleInfo

} // namespace ManagedIrbis.Magazines
