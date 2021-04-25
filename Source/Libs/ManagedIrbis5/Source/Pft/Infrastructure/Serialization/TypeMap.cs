// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TypeMap.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;

using AM;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    sealed class TypeMap
    {
        #region Properties

        public byte Code;

        public Type? Type;

        public Func<PftNode>? Create;

        #endregion

        #region Construction

        static TypeMap()
        {
            foreach (var entry in Map)
            {
                var constructor = entry.Type!.GetConstructor(Type.EmptyTypes);
                if (ReferenceEquals(constructor, null))
                {
                    Magna.Error("Can't find constructor for " + entry.Type);
                    throw new IrbisException();
                }

                entry.Create = Expression.Lambda<Func<PftNode>>
                    (
                        Expression.New(constructor)
                    )
                    .Compile();
            }
        }

        #endregion

        #region Public members

        /// <summary>
        /// Важно, чтобы массив был упорядоченным!
        /// </summary>
        public static readonly TypeMap[] Map =
        {
            new() { Code=1, Type=typeof(PftA) },
            new() { Code=2, Type=typeof(PftAbs) },
            new() { Code=3, Type=typeof(PftAll) },
            new() { Code=4, Type=typeof(PftAny) },
            new() { Code=5, Type=typeof(PftAssignment) },
            new() { Code=6, Type=typeof(PftBang) },
            new() { Code=7, Type=typeof(PftBlank) },
            //new TypeMap { Code=8, Type=typeof(PftBoolean) },
            new() { Code=9, Type=typeof(PftBreak) },
            new() { Code=10, Type=typeof(PftC) },
            new() { Code=11, Type=typeof(PftCeil) },
            new() { Code=12, Type=typeof(PftCodeBlock) },
            new() { Code=13, Type=typeof(PftComma) },
            new() { Code=14, Type=typeof(PftComment) },
            new() { Code=15, Type=typeof(PftComparison) },
            //new TypeMap { Code=16, Type=typeof(PftCondition) },
            new() { Code=17, Type=typeof(PftConditionalLiteral) },
            new() { Code=18, Type=typeof(PftConditionalStatement) },
            new() { Code=19, Type=typeof(PftConditionAndOr) },
            new() { Code=20, Type=typeof(PftConditionNot) },
            new() { Code=21, Type=typeof(PftConditionParenthesis) },
            new() { Code=22, Type=typeof(PftD) },
            new() { Code=23, Type=typeof(PftEat) },
            new() { Code=24, Type=typeof(PftEmpty) },
            new() { Code=25, Type=typeof(PftF) },
            new() { Code=26, Type=typeof(PftFmt) },
            new() { Code=27, Type=typeof(PftFalse) },
            new() { Code=28, Type=typeof(PftField) },
            new() { Code=29, Type=typeof(PftFieldAssignment) },
            new() { Code=30, Type=typeof(PftFirst) },
            new() { Code=31, Type=typeof(PftFloor) },
            new() { Code=32, Type=typeof(PftFor) },
            new() { Code=33, Type=typeof(PftForEach) },
            new() { Code=34, Type=typeof(PftFrac) },
            new() { Code=35, Type=typeof(PftFrom) },
            new() { Code=36, Type=typeof(PftFunctionCall) },
            new() { Code=37, Type=typeof(PftG) },
            new() { Code=38, Type=typeof(PftGroup) },
            new() { Code=39, Type=typeof(PftHash) },
            new() { Code=40, Type=typeof(PftHave) },
            new() { Code=41, Type=typeof(PftInclude) },
            new() { Code=42, Type=typeof(PftL) },
            new() { Code=43, Type=typeof(PftLast) },
            new() { Code=44, Type=typeof(PftLocal) },
            new() { Code=45, Type=typeof(PftMfn) },
            new() { Code=46, Type=typeof(PftMinus) },
            new() { Code=47, Type=typeof(PftMode) },
            new() { Code=48, Type=typeof(PftN) },
            new() { Code=49, Type=typeof(PftNested) },
            new() { Code=50, Type=typeof(PftNl) },
            new() { Code=51, Type=typeof(PftNode) },
            //new TypeMap { Code=52, Type=typeof(PftNumeric) },
            new() { Code=53, Type=typeof(PftNumericExpression) },
            new() { Code=54, Type=typeof(PftNumericLiteral) },
            new() { Code=55, Type=typeof(PftOrphan) },
            new() { Code=56, Type=typeof(PftP) },
            new() { Code=57, Type=typeof(PftParallelFor) },
            new() { Code=58, Type=typeof(PftParallelForEach) },
            new() { Code=59, Type=typeof(PftParallelGroup) },
            new() { Code=60, Type=typeof(PftParallelWith) },
            new() { Code=61, Type=typeof(PftPercent) },
            new() { Code=62, Type=typeof(PftPow) },
            new() { Code=63, Type=typeof(PftProcedureDefinition) },
            new() { Code=64, Type=typeof(PftProgram) },
            new() { Code=65, Type=typeof(PftRef) },
            new() { Code=66, Type=typeof(PftRepeatableLiteral) },
            new() { Code=67, Type=typeof(PftRound) },
            new() { Code=68, Type=typeof(PftRsum) },
            new() { Code=69, Type=typeof(PftS) },
            new() { Code=70, Type=typeof(PftSemicolon) },
            new() { Code=71, Type=typeof(PftSign) },
            new() { Code=72, Type=typeof(PftSlash) },
            new() { Code=73, Type=typeof(PftTrue) },
            new() { Code=74, Type=typeof(PftTrunc) },
            new() { Code=75, Type=typeof(PftUnconditionalLiteral) },
            new() { Code=76, Type=typeof(PftUnifor) },
            new() { Code=77, Type=typeof(PftV) },
            new() { Code=78, Type=typeof(PftVal) },
            new() { Code=79, Type=typeof(PftVariableReference) },
            new() { Code=80, Type=typeof(PftVerbatim) },
            new() { Code=81, Type=typeof(PftWhile) },
            new() { Code=82, Type=typeof(PftWith) },
            new() { Code=83, Type=typeof(PftX) }
        };

        public static TypeMap? FindCode
            (
                byte code
            )
        {
            int lo = 0, hi = Map.Length - 1;

            while (lo <= hi)
            {
                var mid = (lo + hi) / 2;
                var delta = Map[mid].Code - code;
                if (delta == 0)
                {
                    return Map[mid];
                }
                if (delta < 0)
                {
                    lo = mid + 1;
                }
                else
                {
                    hi = mid - 1;
                }
            }

            //for (int i = 0; i < Map.Length; i++)
            //{
            //    if (code == Map[i].Code)
            //    {
            //        return Map[i];
            //    }
            //}

            return null;
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
}
