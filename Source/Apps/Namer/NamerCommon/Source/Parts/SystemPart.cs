// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SystemPart.cs -- часть имени, принадлежащая файловой системе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Parameters;

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
    /// Флаг: преобразование к нижнему регистру.
    /// </summary>
    public bool ToLower { get; set; }

    /// <summary>
    /// Начальная позиция.
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    /// Длина.
    /// </summary>
    public int Length { get; set; }

    #endregion

    #region Protected members

    /// <summary>
    /// 
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
                case "lower":
                    part.ToLower = true;
                    break;
                
                case "start":
                    part.Start = parameter.Value!.ParseInt32 ();
                    break;

                case "length":
                    part.Length = parameter.Value!.ParseInt32();
                    break;

                default:
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    protected string Render
        (
            string value
        )
    {
        var start = Start;
        if (start < 0)
        {
            start = value.Length - start;
        }

        var length = Length;
        if (length < 0)
        {
            length = value.Length - length;
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

        if (ToLower)
        {
            value = value.ToLowerInvariant();
        }

        return value;
    }

    #endregion
}
