// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Check621.cs -- ББК
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// ББК.
    /// </summary>
    public sealed class Check621
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

        #region QualityRules members

        /// <inheritdoc cref="QualityRule.FieldSpec"/>
        public override string FieldSpec => "621";

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
                    621,
                    5,
                    "Отсутствует ББК: поле 621"
                );
            }

            foreach (var field in fields)
            {
                CheckField(field);
            }


            return EndCheck();

        } // method CheckRecord

        #endregion

    } // class Check621

} // namespace ManagedIrbis.Quality.Rules
