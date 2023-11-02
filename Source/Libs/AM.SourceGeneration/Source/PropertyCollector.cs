// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PropertyCollector.cs -- собирает упоминания свойств класса, помеченных указанным атрибутом
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
    /// Собирает упоминания свойств класса, помеченных указанным атрибутом.
    /// </summary>
    internal sealed class PropertyCollector
        : ISyntaxContextReceiver
    {
        #region Properties

        /// <summary>
        /// Собранные упоминания свойства класса.
        /// </summary>
        public List<IPropertySymbol> Collected { get; } = new List<IPropertySymbol>();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="attributeName">Имя искомого атрибута.</param>
        public PropertyCollector (string attributeName)
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
            if (context.Node is PropertyDeclarationSyntax node
                && node.AttributeLists.Count > 0)
            {
                if (context.SemanticModel.GetDeclaredSymbol (node) is IPropertySymbol property
                    && property.GetAttributes().Any (ContainsAttribute))
                {
                    Collected.Add (property);
                }
            }
        }

        #endregion
    }
}
