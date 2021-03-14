// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require903.cs -- шифр документа в базе
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Шифр документа в базе.
    /// </summary>
    public sealed class Require903
        : QualityRule
    {
        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "903"; }
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
                        903,
                        20,
                        "Отсутствует поле 903: Шифр документа"
                    );
            }
            else if (fields.Length > 1)
            {
                AddDefect
                    (
                        903,
                        20,
                        "Повторяется поле 903: Шифр документа"
                    );
            }
            foreach (Field field in fields)
            {
                MustNotContainSubfields
                    (
                        field
                    );
            }

            return EndCheck();
        }

        #endregion

    } // class Require903

} // namespace ManagedIrbis.Quality.Rules
