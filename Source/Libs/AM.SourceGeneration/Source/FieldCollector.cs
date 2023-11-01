// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

/* FieldCollector.cs -- собирает упоминания поля с указанным атрибутом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

#endregion

namespace AM.SourceGeneration
{

    /// <summary>
    /// Собирает упоминания поля класса с указанным атрибутом.
    /// </summary>
    internal sealed class FieldCollector
        : ISyntaxContextReceiver
    {
        #region Properties

        /// <summary>
        /// Собранные упоминания поля класса.
        /// </summary>
        public List<IFieldSymbol> Collected { get; } = new List<IFieldSymbol>();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="attributeName">Имя искомого атрибута.</param>
        public FieldCollector (string attributeName)
            => _attributeName = attributeName;

        #endregion

        #region Private members

        private readonly string _attributeName;

        private bool ContainsAttribute
            (
                AttributeData attributeData
            )
        {
            var attributeClass = attributeData.AttributeClass;
            if (attributeClass is null)
            {
                return false;
            }

            var attributeName = attributeClass.ToDisplayString();
            return StringComparer.Ordinal.Equals (attributeName, _attributeName);
        }

        #endregion

        #region ISyntaxContextReceiver

        /// <inheritdoc cref="ISyntaxContextReceiver.OnVisitSyntaxNode"/>
        public void OnVisitSyntaxNode
            (
                GeneratorSyntaxContext context
            )
        {
            if (context.Node is FieldDeclarationSyntax node
                && node.AttributeLists.Count > 0)
            {
                foreach (var variable in node.Declaration.Variables)
                {
                    if (context.SemanticModel.GetDeclaredSymbol (variable) is IFieldSymbol symbol
                        && symbol.GetAttributes().Any (ContainsAttribute))
                    {
                        Collected.Add (symbol);
                    }
                }
            }
        }

        #endregion
    }
}
