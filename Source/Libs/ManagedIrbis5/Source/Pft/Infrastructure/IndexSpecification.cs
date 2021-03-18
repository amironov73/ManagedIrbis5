﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* IndexSpecification.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;
using AM.IO;
using AM.Text;



using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;


#endregion

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
    /// Index specification (for fields).
    /// </summary>
    public struct IndexSpecification
        : ICloneable
    {
        #region Properties

        /// <summary>
        /// Index kind.
        /// </summary>
        public IndexKind Kind { get; set; }

        /// <summary>
        /// Index specified by literal.
        /// </summary>
        public int Literal { get; set; }

        /// <summary>
        /// Index specified by expression.
        /// </summary>
        [CanBeNull]
        public string Expression { get; set; }

        /// <summary>
        /// Compiled <see cref="Expression" />.
        /// </summary>
        [CanBeNull]
        public PftNumeric Program { get; set; }

        #endregion

        #region Private members

        private PftNumeric CompileProgram()
        {
            if (!ReferenceEquals(Program, null))
            {
                return Program;
            }

            string expression = Expression
                .ThrowIfNull("Expression");

            PftLexer lexer = new PftLexer();
            PftTokenList tokens = lexer.Tokenize(expression);
            PftParser parser = new PftParser(tokens);
            Program = parser.ParseArithmetic();

            return Program;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare two specifications.
        /// </summary>
        public static bool Compare
            (
                IndexSpecification left,
                IndexSpecification right
            )
        {
            // TODO more intelligent compare

            bool result = left.Kind == right.Kind
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
            int result = 0;

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

                    PftNumeric program = CompileProgram();
                    context.Evaluate(program);
                    result = (int)program.Value - 1;

                    break;
            }

            return result;
        }

        /// <summary>
        /// Deserialize the specification.
        /// </summary>
        public void Deserialize
            (
                BinaryReader reader
            )
        {
            Code.NotNull(reader, "reader");

            Kind = (IndexKind)reader.ReadPackedInt32();
            Literal = reader.ReadPackedInt32();
            Expression = reader.ReadNullableString();
            Program = null;
        }

        /// <summary>
        /// Get "all repeats" value.
        /// </summary>
        public static IndexSpecification GetAll()
        {
            return new IndexSpecification
            {
                Kind = IndexKind.AllRepeats
            };
        }

        /// <summary>
        /// Get literal value.
        /// </summary>
        public static IndexSpecification GetLiteral
            (
                int i
            )
        {
            return new IndexSpecification
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
            PftNodeInfo result = new PftNodeInfo
            {
                Name = "Index"
            };
            PftNodeInfo kind = new PftNodeInfo
            {
                Name = "Kind",
                Value = Kind.ToString()
            };
            result.Children.Add(kind);
            if (Kind == IndexKind.Literal)
            {
                PftNodeInfo literal = new PftNodeInfo
                {
                    Name = "Literal",
                    Value = Literal.ToInvariantString()
                };
                result.Children.Add(literal);
            }
            if (Kind == IndexKind.Expression)
            {
                PftNodeInfo expression = new PftNodeInfo
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
            Code.NotNull(writer, "writer");

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

            StringBuilder result = StringBuilderCache.Acquire();
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

            return StringBuilderCache.GetStringAndRelease(result);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implicit conversion.
        /// </summary>
        public static implicit operator IndexSpecification
            (
                int i
            )
        {
            return GetLiteral(i);
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            IndexSpecification result = (IndexSpecification)MemberwiseClone();

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
                return string.Format
                    (
                        "{0}: {1}",
                        Kind,
                        Literal
                    );
            }

            if (Kind != IndexKind.Expression)
            {
                return Kind.ToString();
            }

            return string.Format
                (
                    "{0}: {1}",
                    Kind,
                    Expression
                );
        }

        #endregion
    }
}

