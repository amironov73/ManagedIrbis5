// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Require60.cs -- раздел знаний
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Quality.Rules
{
    /// <summary>
    /// Раздел знаний.
    /// </summary>
    public sealed class Require60
        : QualityRule
    {
        #region Private members

        private MenuFile _menu;

        #endregion

        #region QualityRule members

        /// <inheritdoc />
        public override string FieldSpec
        {
            get { return "60"; }
        }

        /// <inheritdoc />
        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            if (IsBook())
            {
                Field[] fields = GetFields();

                if (fields.Length == 0)
                {
                    AddDefect
                        (
                            60,
                            3,
                            "Отстутсвует поле 60: Раздел знаний"
                        );
                }

                _menu = CacheMenu("rzn.mnu", _menu);

            }

            return EndCheck();
        }

        #endregion

    } // class Require60

} // namespace ManagedIrbis.Quality.Rules
