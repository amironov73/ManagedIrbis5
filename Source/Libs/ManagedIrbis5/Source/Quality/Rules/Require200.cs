﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require200.cs -- основное заглавие
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
    /// Основное заглавие.
    /// </summary>
    public sealed class Require200
        : QualityRule
    {
        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "200"; }
        }

        /// <inheritdoc />
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
                        200,
                        10,
                        "Не заполнено поле 200: Заглавие"
                    );
            }
            else if (fields.Length != 1)
            {
                AddDefect
                    (
                        200,
                        10,
                        "Повторяется поле 200: Заглавие"
                    );
            }
            else
            {
                Field field = fields[0];
                if (IsSpec())
                {
                    if (field.HaveNotSubField('v'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Отсутутсвует подполе 200^v: Обозначение и номер тома"
                            );
                    }
                }
                else
                {
                    if (field.HaveSubField('v'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Присутствует подполе 200^v: Обозначение и номер тома"
                            );
                    }
                    if (field.HaveNotSubField('a'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Отсутутсвует подполе 200^a: Заглавие"
                            );
                    }
                }
            }

            return EndCheck();
        }

        #endregion

    } // class Require200

} // namespace ManagedIrbis.Quality.Rules
