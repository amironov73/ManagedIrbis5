// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* PropertyNode.cs -- обращение к свойству объекта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Обращение к свойству объекта.
    /// </summary>
    sealed class PropertyNode : AtomNode
    {
        #region Construction

        public PropertyNode (string objectName, string propertyName)
        {
            _objectName = objectName;
            _propertyName = propertyName;
        }

        #endregion

        #region Private members

        private readonly string _objectName;
        private readonly string _propertyName;

        private dynamic? ComputeStatic (Context context)
        {
            var type = context.FindType (_objectName);
            if (type is null)
            {
                context.Error.WriteLine ($"Variable or type {_objectName} not found");

                return null;
            }

            var property = type.GetProperty (_propertyName);
            if (property is not null)
            {
                return property.GetValue (null);
            }

            var field = type.GetField (_propertyName);
            if (field is not null)
            {
                return field.GetValue (null);
            }

            return null;
        }

        #endregion

        #region AtomNode members

        public override dynamic? Compute (Context context)
        {
            if (!context.TryGetVariable (_objectName, out var obj))
            {
                return ComputeStatic (context);
            }

            if (obj is null)
            {
                return null;
            }

            var type = ((object) obj).GetType();
            var property = type.GetProperty (_propertyName);
            if (property is not null)
            {
                return property.GetValue (obj);
            }

            var field = type.GetField (_propertyName);
            if (field is not null)
            {
                return field.GetValue (obj);
            }

            return null;
        }

        #endregion
    }
}
