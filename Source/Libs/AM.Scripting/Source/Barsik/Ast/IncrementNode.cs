// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* IncrementNode.cs -- инкремент переменной
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Инкремент переменной.
    /// </summary>
    sealed class IncrementNode
        : AtomNode
    {
        #region Construction

        public IncrementNode
            (
                string variableName,
                string? prefix,
                string? suffix
            )
        {
            _prefix = prefix;
            _variableName = variableName;
            _suffix = suffix;
        }

        #endregion

        #region Private members

        private readonly string? _prefix;
        private readonly string _variableName;
        private readonly string? _suffix;

        private dynamic Increment
            (
                dynamic value
            )
        {
            if (value is string text)
            {
                var number = new NumberText (text);
                number.Increment();

                return number.ToString();
            }

            return (value + 1);
        }

        private dynamic Decrement
            (
                dynamic value
            )
        {
            if (value is string text)
            {
                var number = new NumberText (text);
                number.Increment();

                return number.ToString();
            }

            return (value - 1);
        }

        #endregion

        #region AtomNode members

        /// <inheritdoc cref="AtomNode.Compute"/>
        public override dynamic? Compute
            (
                Context context
            )
        {
            if (!context.TryGetVariable (_variableName, out var value))
            {
                context.Error.WriteLine ($"Variable {_variableName} not found");
                return null;
            }

            if (value is null)
            {
                return value;
            }

            var result = value;
            switch (_prefix)
            {
                case "++":
                    result = value = Increment (value);
                    break;

                case "--":
                    result = value = Decrement (value);
                    break;
            }

            switch (_suffix)
            {
                case "++":
                    value = Increment (value);
                    break;

                case "--":
                    value = Decrement (value);
                    break;
            }

            context.SetVariable (_variableName, value);

            return result;
        }

        #endregion
    }
}
