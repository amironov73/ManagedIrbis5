﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require210.cs -- выходные данные
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Выходные данные
    /// </summary>
    public sealed class Require210
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                Field field
            )
        {
            MustNotContainText(field);

            var city = field.GetFirstSubField('a');
            var publisher = field.GetFirstSubField('c');
            var year = field.GetFirstSubField('d');

            if (city != null)
            {
                if (city.Value.SafeContains(",", ";"))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Несколько городов в одном подполе 210^a"
                        );
                }

                if (city.Value.SafeContains("б."))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Город Б. М. в подполе 210^a"
                        );
                }
                else if (city.Value.SafeContains("."))
                {
                    AddDefect
                        (
                            field,
                            city,
                            1,
                            "Город с сокращением в подполе 210^a"
                        );
                }
            }

            if (publisher != null)
            {
                if (publisher.Value.SafeContains(",", ";"))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Несколько издательств в одном подполе 210^c"
                        );
                }

                if (publisher.Value.SafeContains("б."))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Издательство Б. И. в подполе 210^c"
                        );
                }
                else if (publisher.Value.SafeContains("."))
                {
                    AddDefect
                        (
                            field,
                            publisher,
                            1,
                            "Издательство с сокращением в подполе 210^c"
                        );
                }
            }

            if (year != null)
            {
                if (year.Value.SafeContains("б."))
                {
                    AddDefect
                        (
                            field,
                            year,
                            1,
                            "Год издания Б. Г. в подполе 210^d"
                        );
                }
            }

            if (field.HaveNotSubField('a') && field.HaveSubField('4'))
            {
                AddDefect
                    (
                        field,
                        1,
                        "Город введен в подполе 200^4: Город на издании"
                    );
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc cref="QualityRule.FieldSpec" />
        public override string FieldSpec
        {
            get { return "210"; }
        }

        /// <inheritdoc cref="QualityRule.CheckRecord" />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var fields = GetFields();
            if (fields.Length == 0)
            {
                AddDefect
                    (
                        210,
                        10,
                        "Отсутствует поле 210: Выходные данные"
                    );
            }
            foreach (var field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion

    } // class Require210

} // namespace ManagedIrbis.Quality.Rules
