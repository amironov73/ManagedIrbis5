// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* CheckEmptySubfields.cs -- обнаружение пустых подполей
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
    /// Обнаружение пустых подполей.
    /// </summary>
    public sealed class CheckEmptySubfields
        : QualityRule
    {
        #region Private members

        private void _CheckField
            (
                Field field
            )
        {
            foreach (SubField subField in field.Subfields)
            {
                if (subField.Value.IsEmpty)
                {
                    AddDefect
                        (
                            field,
                            subField,
                            3,
                            "Пустое подполе {0}^{1}",
                            field.Tag,
                            subField.Code
                        );
                }
            }
        }

        #endregion

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

            Field[] fields = GetFields();
            foreach (Field field in fields)
            {
                _CheckField
                    (
                        field
                    );
            }

            return EndCheck();
        }

        #endregion

    } // class CheckEmptySubfields

} // namespace ManagedIrbis.Quality.Rules
