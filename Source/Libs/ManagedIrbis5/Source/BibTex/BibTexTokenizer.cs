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

/* BibTexTokenizer.cs -- токенизатор BibTex
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace ManagedIrbis.BibTex
{
    /// <summary>
    /// Токенизатор BibTex.
    /// </summary>
    public sealed class BibTexTokenizer
    {
        #region Public methods

        /// <summary>
        /// Разбиение текста на токены.
        /// </summary>
        public Token[] Tokenize
            (
                string text
            )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
