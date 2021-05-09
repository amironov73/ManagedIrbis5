// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IndexSpecification.cs -- спецификация индекса для поля/подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    //
    // Индекс, задающий повторение поля или переменной.
    // Возможные значения
    //
    // не указан
    // число (литерал) указывает непосредственно
    // * последнее повторение
    // + новое повторение (будет создано)
    // - все повторения (склеиваются в одно значение)
    // . текущее повторение (см. контекст исполнения)
    // выражение (будет вычислено)
    //

    /// <summary>
    /// Спецификация индекса для поля/подполя.
    /// </summary>
    public struct IndexSpecification
        : ICloneable
    {
        #region Properties

        /// <summary>
        /// Вид индекса (литерал, все повторения и т. д.).
        /// </summary>
        public IndexKind Kind { get; set; }

        /// <summary>
        /// Литеральное значение индекса (только если вид = литерал).
        /// </summary>
        public int Literal { get; set; }

        /// <summary>
        /// Выражение, задающее значение индекса (только).
        /// </summary>
        public string? Expression { get; set; }

        /// <summary>
        /// Compiled <see cref="Expression" />.
        /// </summary>
        public PftNumeric? Program { get; set; }

        #endregion

        #region Private members

        private PftNumeric? CompileProgram()
        {
            if (!ReferenceEquals(Program, null))
            {
                return Program;
            }

            var expression = Expression
                .ThrowIfNull("Expression");

            var lexer = new PftLexer();
            var tokens = lexer.Tokenize(expression);
            var parser = new PftParser(tokens);
            Program = parser.ParseArithmetic();

            return Program;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Сравнение двух спецификаций.
        /// </summary>
        public static bool Compare
            (
                IndexSpecification left,
                IndexSpecification right
            )
        {
            // TODO more intelligent compare

            var result = left.Kind == right.Kind
                         && left.Literal == right.Literal
                         && PftSerializationUtility.CompareStrings
                         (
                             left.Expression,
                             right.Expression
                         );

            return result;
        }

        /// <summary>
        /// Compute value of the index.
        /// </summary>
        public int ComputeValue<T>
            (
                PftContext context,
                T[] array
            )
        {
            var result = 0;

            switch (Kind)
            {
                case IndexKind.None:
                    result = 0;
                    break;

                case IndexKind.Literal:
                    result = Literal < 0
                        ? array.Length + Literal
                        : Literal - 1;
                    break;

                // *
                case IndexKind.LastRepeat:
                    result = array.Length - 1;
                    break;

                // +
                case IndexKind.NewRepeat:
                    result = array.Length;
                    break;

                // .
                case IndexKind.CurrentRepeat:
                    result = context.Index;
                    break;

                // -
                case IndexKind.AllRepeats:
                    result = 0;
                    break;

                case IndexKind.Expression:

                    var program = CompileProgram()
                        .ThrowIfNull(nameof(CompileProgram));
                    context.Evaluate(program);
                    result = (int)program.Value - 1;

                    break;
            } // switch

            return result;

        } // method ComputeValue

        /// <summary>
        /// Deserialize the specification.
        /// </summary>
        public void Deserialize
            (
                BinaryReader reader
            )
        {
            Kind = (IndexKind)reader.ReadPackedInt32();
            Literal = reader.ReadPackedInt32();
            Expression = reader.ReadNullableString();
            Program = null;
        }

        /// <summary>
        /// Get "all repeats" value.
        /// </summary>
        public static IndexSpecification GetAll() =>
            new () { Kind = IndexKind.AllRepeats };

        /// <summary>
        /// Get literal value.
        /// </summary>
        public static IndexSpecification GetLiteral
            (
                int i
            )
        {
            return new ()
            {
                Kind = IndexKind.Literal,
                Literal = i,
                Expression = i.ToInvariantString()
            };
        }

        /// <summary>
        /// Get node info for debugger visualization.
        /// </summary>
        public PftNodeInfo GetNodeInfo()
        {
            var result = new PftNodeInfo
            {
                Name = "Index"
            };
            var kind = new PftNodeInfo
            {
                Name = "Kind",
                Value = Kind.ToString()
            };
            result.Children.Add(kind);
            if (Kind == IndexKind.Literal)
            {
                var literal = new PftNodeInfo
                {
                    Name = "Literal",
                    Value = Literal.ToInvariantString()
                };
                result.Children.Add(literal);
            }
            if (Kind == IndexKind.Expression)
            {
                var expression = new PftNodeInfo
                {
                    Name = "Expression",
                    Value = Expression
                };
                result.Children.Add(expression);
            }

            return result;
        }

        /// <summary>
        /// Serialize the specification.
        /// </summary>
        public void Serialize
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32((int)Kind)
                .WritePackedInt32(Literal)
                .WriteNullable(Expression);
        }

        /// <summary>
        ///
        /// </summary>
        public string ToText()
        {
            if (Kind == IndexKind.None)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            result.Append('[');
            switch (Kind)
            {
                case IndexKind.AllRepeats:
                    result.Append('-');
                    break;

                case IndexKind.CurrentRepeat:
                    result.Append('.');
                    break;

                case IndexKind.Expression:
                    result.Append(Expression);
                    break;

                case IndexKind.LastRepeat:
                    result.Append('*');
                    break;

                case IndexKind.Literal:
                    result.Append(Literal.ToInvariantString());
                    break;

                case IndexKind.NewRepeat:
                    result.Append('+');
                    break;
            }
            result.Append(']');

            return result.ToString();
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implicit conversion.
        /// </summary>
        public static implicit operator IndexSpecification ( int i ) => GetLiteral(i);

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            // TODO сделать ручное копирование?
            var result = (IndexSpecification)MemberwiseClone();

            // Reset the program
            result.Program = null;

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            if (Kind == IndexKind.Literal)
            {
                return $"{Kind}: {Literal}";
            }

            if (Kind != IndexKind.Expression)
            {
                return Kind.ToString();
            }

            return $"{Kind}: {Expression}";
        }

        #endregion

    } // class IndexSpecification

} // ManagedIrbis.Pft.Infrastructure

