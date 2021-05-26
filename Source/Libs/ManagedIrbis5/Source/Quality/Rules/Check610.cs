﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Check610.cs -- ключевые слова
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Ключевые слова.
    /// </summary>
    public sealed class Check610
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                Field field
            )
        {
            MustNotContainSubfields(field);

            var text = field.Value;
            if (text.SafeContains('.'))
            {
                AddDefect
                    (
                        field,
                        1,
                        "Ключевые слова с сокращениями"
                    );
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc cref="QualityRule.FieldSpec" />
        public override string FieldSpec => "610";

        /// <inheritdoc cref="QualityRule.CheckRecord" />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            Field[] fields = GetFields();
            if (fields.Length == 0)
            {
                AddDefect
                    (
                        610,
                        5,
                        "Отсутствуют ключевые слова: поле 610"
                    );
            }

            MustBeUniqueField
                (
                    fields
                );

            foreach (Field field in fields)
            {
                CheckField(field);
            }


            return EndCheck();
        }

        #endregion

    } // class Check610

} // namespace ManagedIrbis.Quality.Rules
