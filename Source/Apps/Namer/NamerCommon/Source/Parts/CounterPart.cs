// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CounterPart.cs -- счетчик
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
/// Счетчик.
/// </summary>
[PublicAPI]
public sealed class CounterPart
    : NamePart
{
    #region Properties

    /// <inheritdoc cref="Designation"/>
    public override string Designation => "counter";

    /// <inheritdoc cref="NamePart.Title"/>
    public override string Title => "Счетчик";

    /// <summary>
    /// Начальное значение счетчика.
    /// </summary>
    public int InitialValue { get; set; }

    /// <summary>
    /// Значение счетчика.
    /// </summary>
    public int CurrentValue { get; set; }

    /// <summary>
    /// Уровень.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Ширина.
    /// </summary>
    public int Width { get; set; }

    #endregion

    #region NamePart members

    /// <inheritdoc cref="NamePart.Parse"/>
    public override NamePart Parse
        (
            string text
        )
    {
        Sure.NotNull (text);

        var result = new CounterPart();
        var parameters = ParameterUtility.ParseString (text);
        foreach (var parameter in parameters)
        {
            parameter.Verify (true);
            switch (parameter.Name)
            {
                case "start":
                    result.InitialValue = parameter.Value!.ParseInt32 ();
                    break;

                case "width":
                    result.Width = parameter.Value!.ParseInt32();
                    break;

                default:
                    throw new ApplicationException();
            }
        }

        return result;
    }

    /// <inheritdoc cref="NamePart.Render"/>
    public override string Render
        (
            NamingContext context,
            FileInfo fileInfo
        )
    {
        var value = ++CurrentValue;

        if (Width > 0)
        {
            var format = new string ('0', Width);
            return value.ToInvariantString (format);
        }

        return value.ToInvariantString();
    }

    /// <inheritdoc cref="NamePart.Reset"/>
    public override void Reset()
    {
        base.Reset();
        CurrentValue = InitialValue;
    }

    #endregion
}
