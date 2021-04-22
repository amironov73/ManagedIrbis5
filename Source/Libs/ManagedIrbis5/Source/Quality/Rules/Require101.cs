// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require101.cs -- язык основного текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Язык основного текста.
    /// </summary>
    public sealed class Require101
        : QualityRule
    {
        #region Private members

        private MenuFile? _menu;

        private void CheckField
            (
                Field field
            )
        {
            MustNotContainSubfields
                (
                    field
                );
            if (!CheckForMenu(_menu, field.Value.ToString()))
            {
                AddDefect
                    (
                        field,
                        10,
                        "Поле 101 (язык основного текста) не из словаря"
                    );
            }
        }

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec => "101";

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
                        101,
                        10,
                        "Не заполнено поле 101: Язык основного текста"
                    );
            }

            MustBeUniqueField
                (
                    fields
                );

            _menu = CacheMenu("jz.mnu", _menu);
            foreach (Field field in fields)
            {
                CheckField(field);
            }

            return EndCheck();
        }

        #endregion

    } // class Require101

} // namespace ManagedIrbis.Quality.Rules
