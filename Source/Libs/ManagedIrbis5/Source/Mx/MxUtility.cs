// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MxUtility.cs -- полезные методы для интерпретатора MX
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Mx;

/// <summary>
/// Полезные методы для интерпретатора MX.
/// </summary>
public static class MxUtility
{
    #region Public methods

    /// <summary>
    /// Разбор файловой спецификации.
    /// </summary>
    public static FileSpecification? ParseFileSpecification
        (
            MxExecutive executive,
            MxArgument[] arguments
        )

    {
        // TODO сделать поддержку множественной спецификации

        Sure.NotNull (executive);
        Sure.NotNull (arguments);

        var fileName = "*.*";
        if (arguments.Length != 0)
        {
            fileName = arguments[0].Text;
        }

        if (string.IsNullOrEmpty (fileName))
        {
            fileName = "*.*";
        }

        if (!FileSpecification.TryParse (fileName, out _))
        {
            fileName = "2." + executive.Provider.Database + "." + fileName;
        }

        var result = FileSpecification.Parse (fileName);

        return result;
    }

    #endregion
}
