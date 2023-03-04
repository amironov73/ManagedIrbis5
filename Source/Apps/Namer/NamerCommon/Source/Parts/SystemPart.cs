// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SystemPart.cs -- часть имени, принадлежащая файловой системе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM;
using AM.Parameters;
using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Часть имени, принадлежащая файловой системе,
/// например, расширение.
/// </summary>
[PublicAPI]
public abstract class SystemPart
    : NamePart
{
    #region Properties

    /// <summary>
    /// Флаг: попытка хитро определить заглавие.
    /// </summary>
    public bool Smart { get; set; }
    
    /// <summary>
    /// Флаг: капитализация.
    /// </summary>
    public bool Capitalize { get; set; }

    /// <summary>
    /// Флаг: преобразование к нижнему регистру.
    /// </summary>
    public bool ToLower { get; set; }

    /// <summary>
    /// Флаг: преобразование к верхнему регистру.
    /// </summary>
    public bool ToUpper { get; set; }

    /// <summary>
    /// Начальная позиция.
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    /// Длина.
    /// </summary>
    public int Length { get; set; }

    #endregion

    #region Private members

    private static string[] _dontCapitalize =
    {
        "a", "about", "and", "as", "at", "but", "by", "for",
        "from", "in", "into", "neither", "nor", "of", "on",
        "or", "since", "than", "that", "the", "to", "until",
        "while", "with", "within"
    };

    private static bool MustBeLowercase
        (
            TextNavigator navigator,
            int position,
            string word
        )
    {
        if (position == 0)
        {
            return false;
        }

        foreach (var one in _dontCapitalize)
        {
            if (string.Compare (word, one, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
        }

        return false;
    }
    
    private static string CapitalizeWord
        (
            TextNavigator navigator,
            int position,
            string word
        )
    {
        if (MustBeLowercase (navigator, position, word))
        {
            return word.ToLowerInvariant();
        }

        var builder = new StringBuilder();
        builder.Append (char.ToUpperInvariant (word[0]));
        for (var i = 1; i < word.Length; i++)
        {
            builder.Append (char.ToLowerInvariant (word[i]));
        }
        
        return builder.ToString();
    }

    private static char FixChar (char chr) => chr switch
    {
        <= ' ' => '_',
        > 'z' => '_',
        _ => chr
    };

    private static string Smartify
        (
            string text
        )
    {
        Sure.NotNull (text);

        var index = text.LastIndexOf ('-');
        if (index < 0)
        {
            return CapitalizeText (text);
        }

        var candidate = text.Substring (index + 1);
        if (string.IsNullOrWhiteSpace (candidate))
        {
            return CapitalizeText (text);
        }

        return CapitalizeText (candidate.Trim());
    }
    
    private static string CapitalizeText
        (
            string text
        )
    {
        Sure.NotNull (text);
        
        var builder = new StringBuilder();
        var navigator = new TextNavigator (text);

        while (!navigator.IsEOF)
        {
            while (!navigator.IsEOF && !navigator.IsLetter())
            {
                builder.Append (FixChar (navigator.ReadChar()));
            }

            var position = navigator.Position;
            var word = navigator.ReadWord();
            if (!word.IsEmpty)
            {
                builder.Append (CapitalizeWord (navigator, position, word.ToString()));
            }
        }

        return builder.ToString();
    }

    #endregion
    
    #region Protected members

    /// <summary>
    /// Разбор опции для элемента имени.
    /// </summary>
    protected bool Parse
        (
            SystemPart part,
            string text
        )
    {
        var parameters = ParameterUtility.ParseString (text);
        foreach (var parameter in parameters)
        {
            parameter.Verify (true);
            switch (parameter.Name)
            {
                case "capitalize":
                    part.Capitalize = true;
                    break;

                case "lower":
                    part.ToLower = true;
                    break;

                case "length":
                    part.Length = parameter.Value!.ParseInt32();
                    break;

                case "smart":
                    part.Smart = true;
                    break;
                
                case "start":
                    part.Start = parameter.Value!.ParseInt32 ();
                    break;

                case "upper":
                    part.ToUpper = true;
                    break;

                default:
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Рендер элемента имени.
    /// </summary>
    protected string Render
        (
            string value
        )
    {
        var start = Start;
        if (start < 0)
        {
            start = value.Length + start;
        }

        var length = Length;
        if (length < 0)
        {
            length = value.Length + length;
        }

        if (Start != 0)
        {
            value = Length != 0
                ? value.Substring (start, length)
                : value.Substring (start);
        }
        else
        {
            if (Length != 0)
            {
                value = value.Substring (0, length);
            }
        }

        if (Smart)
        {
            value = Smartify (value);
        }

        if (Capitalize)
        {
            value = CapitalizeText (value);
        }

        if (ToUpper)
        {
            value = value.ToUpperInvariant();
        }

        if (ToLower)
        {
            value = value.ToLowerInvariant();
        }

        return value;
    }

    #endregion
}
