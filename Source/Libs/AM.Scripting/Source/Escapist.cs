// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UseNameofExpression

/* Escapist.cs -- умеет разбирать строковый литерал с экранированными символами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using Sprache;

#endregion

#nullable enable

namespace AM.Scripting
{
    /// <summary>
    /// Умеет разбирать строковый литерал с экранированными символами.
    /// </summary>
    sealed class Escapist
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="limiter">Символ-ограничитель.</param>
        /// <param name="escapeSymbol">Экранирующий символ.</param>
        public Escapist
            (
                char limiter,
                char escapeSymbol
            )
        {
            _limiter = limiter;
            _escapeSymbol = escapeSymbol;
        }

        #endregion

        #region Private members

        private readonly char _limiter;
        private readonly char _escapeSymbol;

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор входного потока.
        /// </summary>
        public IResult<string> Parse
            (
                IInput input
            )
        {
            var builder = StringBuilderPool.Shared.Get();

            if (!input.AtEnd && input.Current == _limiter)
            {
                input = input.Advance();
                if (!input.AtEnd)
                {
                    var escape = false;

                    while (!input.AtEnd)
                    {
                        if (input.Current == _limiter)
                        {
                            if (!escape)
                            {
                                var result = builder.ToString();
                                StringBuilderPool.Shared.Return (builder);

                                return Result.Success (result, input.Advance());
                            }
                        }
                        else
                        {
                            if (input.Current == _escapeSymbol)
                            {
                                escape = !escape;
                            }
                            else
                            {
                                escape = false;
                            }
                        }

                        builder.Append (input.Current);
                        input = input.Advance();
                    }
                }
            }

            StringBuilderPool.Shared.Return (builder);

            return Result.Failure<string> (input, "Bad escaped literal", Array.Empty<string>());
        }

        #endregion
    }
}
