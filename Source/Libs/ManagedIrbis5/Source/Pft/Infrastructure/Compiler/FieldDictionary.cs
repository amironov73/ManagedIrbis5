// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldDictionary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class FieldDictionary
    {
        #region Properties

        public Dictionary<string, FieldInfo> Forward { get; } = new();

        public Dictionary<int, FieldInfo> Backward { get; } = new();

        public int LastId { get; set; }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public void Add
            (
                FieldInfo info
            )
        {
            Forward.Add(info.Text, info);
            Backward.Add(info.Id, info);
        }

        public FieldInfo Create
            (
                PftField field
            )
        {
            var result = new FieldInfo(field, ++LastId);
            Add(result);

            return result;
        }

        public FieldInfo? Get
            (
                PftField field
            )
        {
            var text = field.ToSpecification().ToString();
            Forward.TryGetValue(text, out FieldInfo? result);

            return result;
        }

        public FieldInfo Get
            (
                int id
            )
        {
            if (!Backward.TryGetValue(id, out FieldInfo? result))
            {
                throw new PftCompilerException();
            }

            return result;
        }

        public FieldInfo GetOrCreate
            (
                PftField field
            )
        {
            return Get(field) ?? Create(field);
        }

        #endregion

    }
}
