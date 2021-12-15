// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* NotNode.cs -- логическое отрицание
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Логическое отрицание
    /// </summary>
    sealed class NotNode
        : AtomNode
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public NotNode (AtomNode inner)
        {
            _inner = inner;
        }

        #endregion

        #region Private members

        private readonly AtomNode _inner;

        /// <summary>
        /// Преобразование любого значения в логическое.
        /// </summary>
        public static bool ToBoolean (object? value)
        {
            return value switch
            {
                null => false,
                true => true,
                false => false,
                "true" or "True" => true,
                "false" or "False" => false,
                string text => !string.IsNullOrEmpty (text),
                sbyte sb => sb != 0,
                byte b => b != 0,
                short i16 => i16 != 0,
                int i32 => i32 != 0,
                long i64 => i64 != 0,
                float f32 => f32 != 0.0f,
                double d64 => d64 != 0.0,
                decimal d => d != 0.0m,
                IList list => list.Count != 0,
                IDictionary dictionary => dictionary.Count != 0,
                _ => true
            };
        }


        #endregion

        #region AtomNode members

        /// <inheritdoc cref="AtomNode.Compute"/>
        public override dynamic? Compute (Context context)
        {
            var value = ToBoolean (_inner.Compute (context));

            return !value;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"not ({_inner})";
        }

        #endregion
    }
}
