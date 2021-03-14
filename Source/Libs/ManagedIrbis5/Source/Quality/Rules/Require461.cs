// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require461.cs -- общие сведения о многотомнике
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Общие сведения о многотомнике.
    /// </summary>
    public sealed class Require461
        : QualityRule
    {
        #region Private members

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "461"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            Field[] fields = GetFields();
            if (IsPazk())
            {
                if (fields.Length != 0)
                {
                    AddDefect
                        (
                            461,
                            10,
                            "Присутствует поле 461 при рабочем листе PAZK"
                        );
                }
                goto DONE;
            }

            if (IsSpec())
            {
                if (fields.Length == 0)
                {
                    AddDefect
                        (
                            461,
                            10,
                            "Не заполнено поле 461: Основные сведения"
                        );
                }
                else
                {
                    Field field = fields[0];
                    if (field.HaveNotSubField('c'))
                    {
                        AddDefect
                            (
                                field,
                                10,
                                "Отсутутсвует подполе 461^C: Заглавие"
                            );
                    }
                }
            }

            DONE: return EndCheck();
        }

        #endregion

    } // class Require461

} // namespace ManagedIrbis.Quality.Rules
