// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ChapterCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Biblio
{
    /// <summary>
    ///
    /// </summary>

    public sealed class ChapterCollection
        : NonNullCollection<BiblioChapter>,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Parent.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public BiblioChapter? Parent
        {
            get => _parent;
            internal set => SetParent(value);
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterCollection()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ChapterCollection
            (
                BiblioChapter? parent
            )
        {
            Parent = parent;
        }

        #endregion

        #region Private members

        private BiblioChapter? _parent;

        internal void SetParent
            (
                BiblioChapter? parent
            )
        {
            _parent = parent;
            foreach (BiblioChapter chapter in this)
            {
                chapter.Parent = parent;
            }
        }

        #endregion

        #region NonNullCollection<T> members

        /// <inheritdoc cref="NonNullCollection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                BiblioChapter item
            )
        {
            base.InsertItem(index, item);
            item.Parent = _parent;
        }

        /// <inheritdoc cref="NonNullCollection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                BiblioChapter item
            )
        {
            base.SetItem(index, item);
            item.Parent = _parent;
        }

        /// <inheritdoc cref="Collection{T}.ClearItems" />
        protected override void ClearItems()
        {
            foreach (BiblioChapter chapter in this)
            {
                chapter.Parent = null;
            }

            base.ClearItems();
        }

        /// <inheritdoc cref="Collection{T}.RemoveItem" />
        protected override void RemoveItem
            (
                int index
            )
        {
            if (index >= 0 && index < Count)
            {
                BiblioChapter chapter = this[index];
                chapter.Parent = null;
            }

            base.RemoveItem(index);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<ChapterCollection> verifier
                = new Verifier<ChapterCollection>(this, throwOnError);

            foreach (BiblioChapter chapter in this)
            {
                verifier.VerifySubObject(chapter, "chapter");
            }

            return verifier.Result;
        }

        #endregion
    }
}
