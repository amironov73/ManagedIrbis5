// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StandardCommentHandler.cs -- стандартный обработчик комментариев
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Стандартный обработчик комментариев для C/C++-подобных языков.
/// Строчный комментарий начинается с `//`,
/// блочный комментарий начинается с `/*`, заканчивается `*/`.
/// </summary>
public sealed class StandardCommentHandler
    : CommentHandler
{
    #region CommentHandler members

    /// <inheritdoc cref="CommentHandler.ParseComments"/>
    public override Token? ParseComments()
    {
        // мы всегда возвращаем null, т. о. комментарии отбрасываются
        if (PeekChar() == '/')
        {
            var nextChar = _navigator.LookAhead();

            // комментарий до конца строки
            if (nextChar == '/')
            {
                // съедаем всю текущую строку до конца
                _navigator.ReadLine();
                return null;
            }

            // многострочный комментарий
            if (nextChar == '*')
            {
                // проматываем всё до конца
                var position = _navigator.Position;
                _navigator.ReadTo ("*/");
                if (_navigator.Position == position)
                {
                    throw new SyntaxException (_navigator);
                }

                return null;
            }
        }

        return null;
    }

    #endregion
}
