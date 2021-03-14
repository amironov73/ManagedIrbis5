// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* CheckWhitespace.cs -- проверка употребления пробелов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Проверка употребления пробелов в полях/подполях
    /// </summary>
    public sealed class CheckWhitespace
        : QualityRule
    {

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "!100,330,905,907,919,920,3005"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var fields = GetFields();
            foreach (var field in fields)
            {
                CheckWhitespace(field);
            }

            return EndCheck();
        }

        #endregion

    } // class CheckWhitespace

} // namespace ManagedIrbis.Quality.Rules
