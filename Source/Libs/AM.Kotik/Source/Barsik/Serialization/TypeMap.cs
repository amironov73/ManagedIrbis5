// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* TypeMap.cs -- отображение типов при сериализации Барсик-дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Reflection;

using AM.Kotik.Ast;
using AM.Kotik.Barsik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Serialization;

/// <summary>
/// Отображение типов при сериализации Барсик-дерева.
/// </summary>
internal sealed class TypeMap
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

    #endregion

    #region Public members

    /// <summary>
    /// Известные нам типы узлов.
    /// </summary>
    private static readonly TypeMap[] KnownTypes =
    {
        new() { Code=1,  Type=typeof (AwaitNode) },
        new() { Code=2,  Type=typeof (BinaryNode) },
        new() { Code=3,  Type=typeof (BlockNode) },
        new() { Code=4,  Type=typeof (BreakNode) },
        new() { Code=5,  Type=typeof (CallNode) },
        new() { Code=6,  Type=typeof (CastNode) },
        new() { Code=7,  Type=typeof (ConstantNode) },
        new() { Code=8,  Type=typeof (ContinueNode) },
        new() { Code=9,  Type=typeof (DictionaryNode) },
        new() { Code=10, Type=typeof (ExpressionNode) },
        new() { Code=11, Type=typeof (ExternalNode) },
        new() { Code=12, Type=typeof (ForEachNode) },
        new() { Code=13, Type=typeof (FormatNode) },
        new() { Code=14, Type=typeof (ForNode) },
        new() { Code=15, Type=typeof (FunctionDefinitionNode) },
        new() { Code=16, Type=typeof (GotoNode) },
        new() { Code=17, Type=typeof (IfNode) },
        new() { Code=18, Type=typeof (IncrementNode) },
        new() { Code=19, Type=typeof (IndexNode) },
        new() { Code=20, Type=typeof (KeyValueNode) },
        new() { Code=21, Type=typeof (LabelNode) },
        new() { Code=22, Type=typeof (LambdaNode) },
        new() { Code=23, Type=typeof (LinqNode) },
        new() { Code=24, Type=typeof (LinqNode.OrderClause) },
        new() { Code=25, Type=typeof (ListNode) },
        new() { Code=26, Type=typeof (MethodNode) },
        new() { Code=27, Type=typeof (MinusNode) },
        new() { Code=28, Type=typeof (NamedArgumentNode) },
        new() { Code=29, Type=typeof (NewNode) },
        new() { Code=30, Type=typeof (PostfixBangNode) },
        new() { Code=31, Type=typeof (PostfixNode) },
        new() { Code=32, Type=typeof (PrefixBangNode) },
        new() { Code=33, Type=typeof (PrefixNode) },
        new() { Code=34, Type=typeof (ProgramNode) },
        new() { Code=35, Type=typeof (PropertyNode) },
        new() { Code=36, Type=typeof (ReturnNode) },
        new() { Code=37, Type=typeof (SemicolonNode) },
        new() { Code=38, Type=typeof (SimpleStatement) },
        new() { Code=39, Type=typeof (SwitchNode) },
        new() { Code=40, Type=typeof (TernaryNode) },
        new() { Code=41, Type=typeof (ThrowNode) },
        new() { Code=42, Type=typeof (TildaNode) },
        new() { Code=43, Type=typeof (TryNode) },
        new() { Code=44, Type=typeof (TryNode.CatchBlock) },
        new() { Code=45, Type=typeof (UsingNode) },
        new() { Code=46, Type=typeof (VariableNode) },
        new() { Code=47, Type=typeof (WhileNode) },
        new() { Code=48, Type=typeof (WithAssignmentNode) },
        new() { Code=49, Type=typeof (WithNode) },
    };

    /// <summary>
    /// Поиск типа узла по его коду.
    /// </summary>
    public static TypeMap? FindTypeCode
        (
            byte code
        )
    {
        return KnownTypes.FirstOrDefault (item => item.Code == code);
    }

    /// <summary>
    /// Поиск типа узла в таблице.
    /// </summary>
    public static TypeMap? FindType
        (
            Type nodeType
        )
    {
        foreach (var entry in KnownTypes)
        {
            if (ReferenceEquals (nodeType, entry.Type))
            {
                return entry;
            }
        }

        return null;
    }

    /// <summary>
    /// Верификация таблицы типов.
    /// </summary>
    public static void VerifyTypeMap()
    {
        var duplicateCode = KnownTypes
            .GroupBy (x => x.Code)
            .FirstOrDefault(g => g.Count() != 1);
        if (duplicateCode is not null)
        {
            throw new BarsikException ($"Duplicate code {duplicateCode.Key}");
        }

        var duplicateType = KnownTypes
            .GroupBy (x => x.Type)
            .FirstOrDefault (g => g.Count() != 1);
        if (duplicateType is not null)
        {
            throw new BarsikException ($"Duplicate type {duplicateType.Key}");
        }

        foreach (var knownType in KnownTypes)
        {
            var type = knownType.Type!;
            if (type.IsAbstract)
            {
                throw new BarsikException ($"Abstract type {type}");
            }

            if (!type.IsAssignableTo (typeof (AstNode)))
            {
                throw new BarsikException ($"Outsider type {type}");
            }
        }

        var astTypes = Assembly.GetCallingAssembly()
            .GetTypes()
            .Where (t => !t.IsAbstract && t.IsAssignableTo (typeof (AstNode)))
            .ToArray();

        foreach (var nodeType in astTypes)
        {
            if (FindType (nodeType) is null)
            {
                throw new BarsikException ($"Missing type {nodeType}");
            }
        }
    }

    #endregion
}
