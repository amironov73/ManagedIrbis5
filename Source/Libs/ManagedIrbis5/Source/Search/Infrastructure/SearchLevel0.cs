// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchLevel0.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Term, reference or parenthesis.
    /// </summary>
    sealed class SearchLevel0
        : ISearchTree
    {
        #region Properties

        /// <inheritdoc cref="ISearchTree.Parent"/>
        public ISearchTree? Parent { get; set; }

        /// <summary>
        /// Term.
        /// </summary>
        public SearchTerm? Term { get; set; }

        /// <summary>
        /// Reference.
        /// </summary>
        public SearchReference? Reference { get; set; }

        /// <summary>
        /// Parenthesis.
        /// </summary>
        public SearchLevel7? Parenthesis { get; set; }

        #endregion

        #region ISearchTree members

        /// <inheritdoc cref="ISearchTree.Children" />
        public ISearchTree[] Children
        {
            get
            {
                if (!ReferenceEquals(Term, null))
                {
                    return new ISearchTree[] { Term };
                }

                if (!ReferenceEquals(Reference, null))
                {
                    return new ISearchTree[] { Reference };
                }

                return new ISearchTree[] { Parenthesis.ThrowIfNull(nameof(Parenthesis)) };
            }
        }

        /// <inheritdoc cref="ISearchTree.Value" />
        public string? Value
        {
            get { return null; }
        }

        /// <inheritdoc cref="ISearchTree.Find"/>
        public TermLink[] Find
            (
                SearchContext context
            )
        {
            TermLink[] result;

            if (!ReferenceEquals(Term, null))
            {
                result = Term.Find(context);
            }
            else if (!ReferenceEquals(Reference, null))
            {
                result = Reference.Find(context);
            }
            else if (!ReferenceEquals(Parenthesis, null))
            {
                result = Parenthesis.Find(context);
            }
            else
            {
                Magna.Error
                    (
                        "SearchLevel0::Find: "
                        + "unexpected situation"
                    );

                throw new IrbisException("Unexpected SearchLevel0");
            }

            return result;
        }

        /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
        public void ReplaceChild
            (
                ISearchTree fromChild,
                ISearchTree? toChild
            )
        {
            Sure.NotNull(fromChild, nameof(fromChild));

            fromChild.Parent = null;

            var term = fromChild as SearchTerm;
            if (!ReferenceEquals(term, null))
            {
                Term = (SearchTerm?) toChild;
            }

            var reference = fromChild as SearchReference;
            if (!ReferenceEquals(reference, null))
            {
                Reference = (SearchReference?) toChild;
            }

            var level7 = fromChild as SearchLevel7;
            if (!ReferenceEquals(level7, null))
            {
                Parenthesis = (SearchLevel7?) toChild;
            }

            if (!ReferenceEquals(toChild, null))
            {
                toChild.Parent = this;
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() =>
            (
                Term
                ?? (object?)Reference
                ?? Parenthesis
            )
            .ToVisibleString();

        #endregion

    } // class SearchLevel0

} // namespace ManagedIrbis.Infrastructure
