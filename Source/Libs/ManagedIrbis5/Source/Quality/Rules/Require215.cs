// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require215.cs -- количественные характеристики
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Количественные характеристики.
    /// </summary>
    public sealed class Require215
        : QualityRule
    {
        #region Private members

        private void CheckField
            (
                Field field
            )
        {
            var volume = field.GetFirstSubField('a');
            var units = field.GetFirstSubField('1');

            if (volume is null)
            {
                AddDefect
                    (
                        field,
                        5,
                        "Не заполнено подполе 215^a: Объем (цифры)"
                    );
            }

            if (units is not null)
            {
                if (units.Value.SameString("С.", "С"))
                {
                    AddDefect
                        (
                            field,
                            units,
                            1,
                            "Указана единица измерения 'С' в подполе 215^1"
                        );
                }
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec => "215";

        /// <inheritdoc />
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
                        215,
                        5,
                        "Отсутствует поле 215: Количественные характеристики"
                    );
            }
            foreach (var field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion

    } // class Require215

} // namespace ManagedIrbis.Quality.Rules
