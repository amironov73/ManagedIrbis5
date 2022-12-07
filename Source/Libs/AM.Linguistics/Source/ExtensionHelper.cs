// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ExtensionHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics;

/// <summary>
///
/// </summary>
public static class ExtensionHelper
{
    /// <summary>
    /// Одушевленность
    /// </summary>
    public static Animacy Animacy (this Gender gender)
    {
        switch (gender)
        {
            case Gender.FA:
            case Gender.MA:
            case Gender.MAFA:
            case Gender.NA:
            case Gender.PA:
                return Linguistics.Animacy.Animate;

            default:
                return Linguistics.Animacy.Inanimate;
        }
    }

    /// <summary>
    /// Род без учета одушевленности (возвращает F, M, N или P)
    /// </summary>
    public static Gender Gen (this Gender gender)
    {
        switch (gender)
        {
            case Gender.F:
            case Gender.FA:
            case Gender.MAFA:
                return Gender.F;

            case Gender.M:
            case Gender.MA:
                return Gender.M;

            case Gender.N:
            case Gender.NA:
                return Gender.N;

            case Gender.P:
            case Gender.PA:
                return Gender.P;

            default:
                return Gender.Undefined;
        }
    }

    /// <summary>
    /// Число без учета рода и одушеленности
    /// </summary>
    public static Number Number (this Gender gender)
    {
        switch (gender)
        {
            case Gender.P:
            case Gender.PA:
                return Linguistics.Number.Plural;

            default:
                return Linguistics.Number.Singular;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="case"></param>
    /// <param name="gender"></param>
    /// <returns></returns>
    public static int IndexWithAnimate (this Case @case, Gender gender)
    {
        var i = 0;
        switch (@case)
        {
            case Case.Nominative:
                i = 0;
                break;

            case Case.Genitive:
                i = 1;
                break;

            case Case.Dative:
                i = 2;
                break;

            case Case.Accusative:
                i = gender.Animacy() == Linguistics.Animacy.Animate ? 4 : 3;
                break;

            case Case.Instrumental:
                i = 5;
                break;

            case Case.Locative:
                i = 6;
                break;

            case Case.Short:
                i = 7;
                break;
        }

        return i;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <param name="sample"></param>
    /// <returns></returns>
    public static string ToUpper (this string s, string sample)
    {
        return char.IsUpper (sample[0])
            ? char.ToUpper (s[0]) + s.Substring (1).ToLower()
            : s.ToLower();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToUpperFirst (this string s)
    {
        return !string.IsNullOrEmpty (s)
            ? char.ToUpper (s[0]) + s.Substring (1)
            : s;
    }
}
