// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Check908.cs -- авторский знак
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Авторский знак.
    /// </summary>
    public sealed class Check908
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
            if (text.IsEmpty())
            {
                AddDefect
                    (
                        field,
                        5,
                        "Неверный формат поля 908: Авторский знак"
                    );
            }
            else
            {
                var firstLetter = text![0];
                var isGood = firstLetter is >= 'A' and <= 'Z' or >= 'А' and <= 'Я';
                if (!isGood)
                {
                    AddDefect
                        (
                            field,
                            1,
                            "Неверный формат поля 908: Авторский знак"
                        );
                }
                else
                {
                    var regex = @"[А-Я]\s\d{2}";

                    if (firstLetter is >= 'A' and <= 'Z')
                    {
                        regex = @"[A-Z]\d{2}";
                    }
                    if (firstLetter is 'З' or 'О' or 'Ч')
                    {
                        regex = @"[ЗОЧ]-\d{2}";
                    }

                    if (!Regex.IsMatch(text, regex))
                    {
                        AddDefect
                            (
                                field,
                                1,
                                "Неверный формат поля 908: Авторский знак"
                            );
                    }
                }
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec => "908";

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var fields = GetFields();
            if (fields.Length > 1)
            {

                AddDefect
                    (
                        908,
                        5,
                        "Повторяется поле 908: Авторский знак"
                    );
            }
            foreach (var field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion

    } // class Check908

} // namespace ManagedIrbis.Quality.Rules
