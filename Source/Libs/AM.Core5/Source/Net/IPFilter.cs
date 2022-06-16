// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IPFilter.cs -- умеет фильтровать IP-адреса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Net;
using System.Text;

#endregion

#nullable enable

namespace AM.Net;

/// <summary>
/// Умеет фильтровать IP-адреса.
/// </summary>
public sealed class IPFilter
{
    #region Properties

    /// <summary>
    /// Диапазоны допустимых IP-адресов.
    /// </summary>
    public List<IPRange> Ranges { get; } = new ();

    /// <summary>
    /// Пропускать любые адреса без проверки?
    /// </summary>
    public bool AllowAnyAddress { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текстовой спецификации.
    /// </summary>
    public static IPFilter Parse
        (
            string specification
        )
    {
        if (string.IsNullOrEmpty (specification)
            || specification == "*")
        {
            return new IPFilter() { AllowAnyAddress = true };
        }

        var parts = specification.Split (';', ',');
        var result = new IPFilter();
        foreach (var part in parts)
        {
            if (!string.IsNullOrEmpty (part))
            {
                result.Ranges.Add (IPRange.Parse (part));
            }
        }

        return result;
    }

    /// <summary>
    /// Проверка, является ли указанный адрес допустимым.
    /// </summary>
    public bool IsAllowed
        (
            IPAddress address
        )
    {
        Sure.NotNull (address);

        if (AllowAnyAddress)
        {
            return true;
        }

        foreach (var range in Ranges)
        {
            if (range.Contains (address))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Проверка, является ли указанный адрес допустимым.
    /// </summary>
    public bool IsAllowed
        (
            EndPoint endPoint
        )
    {
        return endPoint is  IPEndPoint ip && IsAllowed (ip.Address);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        if (AllowAnyAddress)
        {
            return "*";
        }

        var result = new StringBuilder();
        var first = true;
        foreach (var range in Ranges)
        {
            if (!first)
            {
                result.Append (';');
            }

            result.Append (range);
            first = false;
        }

        return result.ToString();
    }

    #endregion
}
