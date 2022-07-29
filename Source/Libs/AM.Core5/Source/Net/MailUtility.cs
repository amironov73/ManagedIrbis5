// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* MailUtility.cs -- работа с электронной почтой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

using AM.Text;

#endregion

#nullable enable

namespace AM.Net;

/// <summary>
/// Работа с электронной почтой.
/// </summary>
public static class MailUtility
{
    #region Public methods

    /// <summary>
    /// Очистка e-mail от лишних символов.
    /// Перевод кириллических символов в латиницу.
    /// </summary>
    public static string CleanupEmail
        (
            string email
        )
    {
        Sure.NotNullNorEmpty (email);

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (email.Length);
        foreach (var c in email)
        {
            // Пробелов и служебных символов вообще не должно быть
            if (c <= ' ')
            {
                continue;
            }

            // Превращаем кириллические буквы, похожие на латиницу,
            // в визуально совпадающие латинские
            switch (c)
            {
                // запятая и точка расположены на клавиатуре рядом,
                // да и выглядят на мелком шрифте почти одинаково,
                // поэтому запятую часто вводят вместо точки.
                case ',':
                    builder.Append ('.');
                    break;

                case 'А':
                    builder.Append ('A');
                    break;

                case 'а':
                    builder.Append ('a');
                    break;

                case 'В':
                    builder.Append ('B');
                    break;

                case 'в':
                    builder.Append ('b');
                    break;

                case 'С':
                    builder.Append ('C');
                    break;

                case 'с':
                    builder.Append ('c');
                    break;

                case 'Е':
                    builder.Append ('E');
                    break;

                case 'е':
                    builder.Append ('e');
                    break;

                case 'Н':
                    builder.Append ('H');
                    break;

                case 'п':
                    builder.Append ('n');
                    break;

                case 'К':
                    builder.Append ('K');
                    break;

                case 'к':
                    builder.Append ('k');
                    break;

                case 'М':
                    builder.Append ('M');
                    break;

                case 'м':
                    builder.Append ('m');
                    break;

                case 'О':
                    builder.Append ('O');
                    break;

                case 'о':
                    builder.Append ('o');
                    break;

                case 'Р':
                    builder.Append ('P');
                    break;

                case 'р':
                    builder.Append ('p');
                    break;

                case 'Т':
                    builder.Append ('T');
                    break;

                case 'т':
                    builder.Append ('t');
                    break;

                case 'Х':
                    builder.Append ('x');
                    break;

                case 'х':
                    builder.Append ('x');
                    break;

                case 'У':
                    builder.Append ('Y');
                    break;

                case 'у':
                    builder.Append ('y');
                    break;

                default:
                    if (c < 256)
                    {
                        builder.Append (c);
                    }

                    break;
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Верификация (приблизительная) e-mail.
    /// </summary>
    public static bool VerifyEmail
        (
            string email
        )
    {
        if (string.IsNullOrEmpty (email))
        {
            return false;
        }

        var result = Regex.IsMatch
            (
                email,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase
            );

        return result;
    }

    #endregion
}
