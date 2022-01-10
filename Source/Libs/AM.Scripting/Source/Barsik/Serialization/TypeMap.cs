// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TypeMap.cs -- отображение типов при сериализации Барсик-дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace AM.Scripting.Barsik.Serialization;

/// <summary>
/// Отображение типов при сериализации Барсик-дерева.
/// </summary>
sealed class TypeMap
{
    #region Properties

    /// <summary>
    /// Код типа.
    /// </summary>
    public byte Code;

    /// <summary>
    /// Собственно тип.
    /// </summary>
    public Type? Type;

    /// <summary>
    /// Создание узла.
    /// </summary>
    public Func<AstNode>? Create;

    #endregion

        #region Public members

        public static readonly TypeMap[] Map =
        {
            new() { Code=1, Type=typeof (AssignmentNode) },
            new() { Code=2, Type=typeof (AwaitNode) },
            new() { Code=3, Type=typeof (BinaryNode) },
            new() { Code=4, Type=typeof (CastNode) },
            new() { Code=5, Type=typeof (ConstantNode) },
            new() { Code=6, Type=typeof (DefinitionNode) },
            new() { Code=7, Type=typeof (DictionaryNode) },
            new() { Code=8, Type=typeof (ExpressionNode) },
            new() { Code=9, Type=typeof (ExternalNode) },
            new() { Code=10, Type=typeof (ForEachNode) },
            new() { Code=11, Type=typeof (ForNode) },
            new() { Code=12, Type=typeof (FreeCallNode) },
            new() { Code=13, Type=typeof (IfNode) },
            new() { Code=14, Type=typeof (IndexNode) },
            new() { Code=15, Type=typeof (KeyValueNode) },
            new() { Code=16, Type=typeof (LambdaNode) },
            new() { Code=17, Type=typeof (ListNode) },
            new() { Code=18, Type=typeof (MethodNode) },
            new() { Code=19, Type=typeof (NewNode) },
            new() { Code=20, Type=typeof (ParenthesisNode) },
            new() { Code=21, Type=typeof (PostfixNode) },
            new() { Code=22, Type=typeof (PrefixNode) },
            new() { Code=23, Type=typeof (ProgramNode) },
            new() { Code=24, Type=typeof (PropertyNode) },
            new() { Code=25, Type=typeof (RegexNode) },
            new() { Code=26, Type=typeof (ReturnNode) },
            new() { Code=27, Type=typeof (StatementNode) },
            new() { Code=28, Type=typeof (TernaryNode) },
            new() { Code=29, Type=typeof (ThrowNode) },
            new() { Code=30, Type=typeof (TryNode) },
            new() { Code=31, Type=typeof (UsingNode) },
            new() { Code=32, Type=typeof (VariableNode) },
            new() { Code=33, Type=typeof (WhileNode) },
            new() { Code=33, Type=typeof (WhileNode) },
        };

        public static TypeMap? FindCode
            (
                byte code
            )
        {
            return Map.FirstOrDefault (item => item.Code == code);

            // var lo = 0;
            // var hi = Map.Length - 1;
            //
            // while (lo <= hi)
            // {
            //     var mid = (lo + hi) / 2;
            //     var delta = Map[mid].Code - code;
            //     if (delta == 0)
            //     {
            //         return Map[mid];
            //     }
            //     if (delta < 0)
            //     {
            //         lo = mid + 1;
            //     }
            //     else
            //     {
            //         hi = mid - 1;
            //     }
            // }
            //
            // return null;
        }

        public static TypeMap? FindType
            (
                Type nodeType
            )
        {
            foreach (var entry in Map)
            {
                if (ReferenceEquals(nodeType, entry.Type))
                {
                    return entry;
                }
            }

            return null;
        }

        #endregion
}
