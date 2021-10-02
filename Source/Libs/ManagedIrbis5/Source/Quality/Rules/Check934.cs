// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Check934.cs -- год выпуска журнала
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Год выпуска журнала/газеты.
    /// </summary>
    public sealed class Check934
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                Field field
            )
        {
            MustNotContainSubfields(field);

        } // method CheckField

        #endregion

        #region QualityRule members

        /// <inheritdoc cref="QualityRule.FieldSpec"/>
        public override string FieldSpec => "934";

        /// <inheritdoc cref="QualityRule.CheckRecord"/>
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
                        934,
                        5,
                        "Отсутствует год выпуска журнала: поле 934"
                    );
            }

            foreach (var field in fields)
            {
                CheckField(field);
            }

            return EndCheck();

        } // method CheckRecord

        #endregion

    } // class Check934

} // namespace ManagedIrbis.Quality.Rules
