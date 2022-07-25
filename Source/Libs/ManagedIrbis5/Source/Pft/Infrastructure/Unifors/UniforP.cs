// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforP.cs -- выдать оригинальное повторение поля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors;

//
// Выдать заданное оригинальное повторение поля – &uf('P')
// Вид функции: P.
// Назначение: Выдать заданное оригинальное повторение поля.
// PV<tag>^<delim>*<offset>.<length>#<occur>
// где:
// <tag> – метка поля;
// <delim> – разделитель подполя;
// <offset> – смещение;
// <length> – длина;
// <occur> – номер повторения.
//
// Примеры:
//
// &unifor('Pv200#2')
// &unifor('Pv910^a#5')
// &unifor('Pv10^b*2.10#2')
//

internal static class UniforP
{
    #region Public methods

    /// <summary>
    /// Get unique field value.
    /// </summary>
    public static void UniqueField
        (
            PftContext context,
            PftNode? node,
            string? expression
        )
    {
        Sure.NotNull (context);

        if (!string.IsNullOrEmpty (expression))
        {
            try
            {
                if (context.Record is { } record)
                {
                    // ibatrak
                    // ИРБИС игнорирует код команды в спецификации,
                    // все работает как v
                    var command = expression[0];
                    if (command != 'v' && command != 'V')
                    {
                        expression = "v" + expression.Substring (1);
                    }

                    var specification = new FieldSpecification();
                    if (specification.ParseUnifor (expression))
                    {
                        if (specification.Tag == IrbisGuid.Tag)
                        {
                            // Поле GUID не выводится
                            return;
                        }

                        var reference = new FieldReference();
                        reference.Apply (specification);

                        var array = reference.GetUniqueValues (record);
                        string? result = null;
                        switch (reference.FieldRepeat.Kind)
                        {
                            case IndexKind.None:
                                result = string.Join (",", array);
                                break;

                            case IndexKind.Literal:
                                result = array.GetOccurrence
                                    (
                                        reference.FieldRepeat.Literal - 1
                                    );
                                break;

                            case IndexKind.LastRepeat:
                                if (array.Length != 0)
                                {
                                    result = array[^1];
                                }

                                break;

                            default:
                                throw new IrbisException
                                    (
                                        "Unexpected repeat: "
                                        + reference.FieldRepeat.Kind
                                    );
                        }

                        if (!string.IsNullOrEmpty (result))
                        {
                            context.WriteAndSetFlag (node, result);
                            context.VMonitor = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Magna.Logger.LogError
                    (
                        exception,
                        nameof (UniforP) + "::" + nameof (UniqueField)
                    );
            }
        }
    }

    #endregion
}
