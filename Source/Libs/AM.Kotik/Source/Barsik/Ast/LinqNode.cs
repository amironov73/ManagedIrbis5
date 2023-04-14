// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LinqNode.cs -- жалкое подобие LINQ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/*

  from score in scores //required
  let len = score.Length // optional
  where score > 80 // optional
  orderby score // optional
  select score; //must end with select or group
  
  или

  from person in people //required
  group person by person.Age; // required

  несколько источников

  from a in firstSequence
  from b in secondSequence
  select a + b;

  соединение

  from category in categories
  join product in products on category.Id equals product.CategoryId
  select new { .Category = category.Id; .Product = product.Name }

 */

/// <summary>
/// Жалкое подобие LINQ.
/// </summary>
internal sealed class LinqNode
    : AtomNode
{
    #region NestedClasses

    internal sealed class LetClause
        : AstNode
    {
        #region Properties

        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string VariableName { get; }
        
        /// <summary>
        /// Выражение, присваиваемое переменной.
        /// </summary>
        public AtomNode Expression { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LetClause
            (
                string variableName, 
                AtomNode expression
            )
        {
            Sure.NotNullNorEmpty (variableName);
            Sure.NotNull (expression);
            
            VariableName = variableName;
            Expression = expression;
        }

        #endregion

        #region AstNode members

        /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
        internal override void DumpHierarchyItem 
            (
                string? name, 
                int level, 
                TextWriter writer
            )
        {
            base.DumpHierarchyItem (name, level, writer);
            
            DumpHierarchyItem ("Variable", level + 1, writer, VariableName);
            Expression.DumpHierarchyItem ("Expression", level + 1, writer);
        }

        #endregion
    }
    
    internal sealed class FromClause
        : AstNode
    {
        #region Proprerties

        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string VariableName { get; }
        
        /// <summary>
        /// Последовательность.
        /// </summary>
        public AtomNode Sequence { get; }
        
        /// <summary>
        /// Клауза let.
        /// </summary>
        public LetClause? LetClause { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FromClause
            (
                string variableName,
                AtomNode sequence,
                LetClause letClause
            )
        {
            Sure.NotNullNorEmpty (variableName);
            Sure.NotNull (sequence);
            
            VariableName = variableName;
            Sequence = sequence;
            LetClause = letClause;
        }

        #endregion

        #region AstNode members

        /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
        internal override void DumpHierarchyItem 
            (
                string? name, 
                int level, 
                TextWriter writer
            )
        {
            base.DumpHierarchyItem (name, level, writer);
            
            DumpHierarchyItem ("Variable", level + 1, writer, VariableName);
            Sequence.DumpHierarchyItem ("Sequence", level + 1, writer);
            LetClause?.DumpHierarchyItem ("Let", level + 1, writer);
        }

        #endregion
    }

    internal sealed class JoinClause
        : AstNode
    {
        #region Properties

        /// <summary>
        /// Имя переменной.
        /// </summary>
        public string VariableName { get; }
        
        /// <summary>
        /// Последовательность.
        /// </summary>
        public AtomNode Sequence { get; }
        
        /// <summary>
        /// Предложение on.
        /// </summary>
        public AtomNode On { get; }
        
        /// <summary>
        /// Предложение equals.
        /// </summary>
        public AtomNode EqualsTo { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public JoinClause
            (
                string variableName,
                AtomNode sequence,
                AtomNode on,
                AtomNode equals
            )
        {
            VariableName = variableName;
            Sequence = sequence;
            On = on;
            EqualsTo = equals;
        }

        #endregion

        #region AstNode members

        /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
        internal override void DumpHierarchyItem 
            (
                string? name, 
                int level, 
                TextWriter writer
            )
        {
            base.DumpHierarchyItem (name, level, writer);
            
            DumpHierarchyItem ("Variable", level + 1, writer, VariableName);
            Sequence.DumpHierarchyItem ("Sequence", level + 1, writer);
            On.DumpHierarchyItem ("On", level + 1, writer);
            EqualsTo.DumpHierarchyItem ("Equals", level + 1, writer);
        }

        #endregion
    }

    private sealed class Grouped
        : IGrouping<object?, object?>
    {
        #region IGrouping members
    
        public object? Key { get; }
        
        public List<object?> Items { get; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object?> GetEnumerator() => Items.GetEnumerator();

        #endregion

        #region Construction

        public Grouped
            (
                object? key, 
                List<object?> items
            )
        {
            Key = key;
            Items = items;
        }

        #endregion
    }

    private sealed class GroupedList
        : List<Grouped>
    {
        #region Public methods

        public void Add 
            (
                object? key,
                object? value
            )
        {
            var found = false;
            foreach (var one in this)
            {
                if (OmnipotentComparer.Default.Compare (one.Key, key) == 0)
                {
                    one.Items.Add (value);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var item = new Grouped (key, new List<object?> { value });
                Add (item);
            }
        }

        #endregion
    }

    internal sealed class OrderClause
        : AstNode
    {
        #region Properties

        public AtomNode Expression { get; }

        public bool Descending { get; }

        #endregion

        #region Construction

        public OrderClause
            (
                AtomNode expression,
                bool descending
            )
        {
            Expression = expression;
            Descending = descending;
        }

        #endregion

        #region AstNode members

        /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
        internal override void DumpHierarchyItem 
            (
                string? name, 
                int level, 
                TextWriter writer
            )
        {
            base.DumpHierarchyItem (name, level, writer);
            
            Expression.DumpHierarchyItem ("Expression", level + 1, writer);
            DumpHierarchyItem ("Descending", level + 1, writer, Descending.ToInvariantString());
        }

        #endregion
    }

    internal sealed class GroupClause
        : AstNode
    {
        #region Properties

        public AtomNode Expression { get; }
        
        public AtomNode By { get; }

        #endregion

        #region Construction

        public GroupClause
            (
                AtomNode expression, 
                AtomNode by
            )
        {
            Expression = expression;
            By = by;
        }

        #endregion
        
        #region AstNode members

        /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
        internal override void DumpHierarchyItem 
            (
                string? name, 
                int level, 
                TextWriter writer
            )
        {
            base.DumpHierarchyItem (name, level, writer);
            
            Expression.DumpHierarchyItem ("Expression", level + 1, writer);
            By.DumpHierarchyItem ("By", level + 1, writer);
        }

        #endregion
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LinqNode
        (
            IList<FromClause> fromClauses,
            IList<JoinClause> joinClauses,
            AtomNode? whereClause,
            OrderClause? orderClause,
            AtomNode? selectClause,
            GroupClause? groupClause
        )
    {
        Sure.NotNull (fromClauses);

        if (fromClauses.Count == 0)
        {
            throw new SyntaxException ("from clause must present");
        }
        
        if (selectClause is null && groupClause is null)
        {
            throw new SyntaxException ("Must be select or group clause");
        }

        if (selectClause is not null && groupClause is not null)
        {
            throw new SyntaxException ("Can't handle select and group simultaneously");
        }

        _fromClauses = fromClauses;
        _joinClauses = joinClauses;
        _whereClause = whereClause;
        _orderClause = orderClause;
        _selectClause = selectClause;
        _groupClause = groupClause;
    }

    #endregion

    #region Private members

    private readonly IList<FromClause> _fromClauses;
    private readonly IList<JoinClause> _joinClauses;
    private readonly AtomNode? _whereClause;
    private readonly OrderClause? _orderClause;
    private readonly AtomNode? _selectClause;
    private readonly GroupClause? _groupClause;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        context = context.CreateChildContext();

        var fromRank = _fromClauses.Count;
        var joinRank = _joinClauses.Count;
        var rank = fromRank + joinRank;
        var variables = context.Variables;
        var sequences = new List<IReadOnlyList<dynamic?>>();
        foreach (var clause in _fromClauses)
        {
            for (var i = 0; i < fromRank; i++)
            {
                var variableName = _fromClauses[i].VariableName;

                // переменная не должна упоминаться в родительских контекстах
                Sure.AssertState (!context.Parent!.TryGetVariable (variableName, out _));

                // это должно быть lvalue
                Sure.AssertState (context.EnsureVariableCanBeAssigned (variableName));

                // имя переменной не должно повторяться в текущем контексте
                var count = _fromClauses.Count (x => x.VariableName == variableName);
                Sure.AssertState (count == 1);

                variables[variableName] = null;
            }

            var computed = (IEnumerable<dynamic?>) clause.Sequence.Compute (context)!;
            IReadOnlyList<dynamic?> sequence;
            if (computed is IReadOnlyList<dynamic?> readOnlyList)
            {
                sequence = readOnlyList;
            }
            else
            {
                sequence = computed.ToList();
            }

            if (clause.LetClause is { } letClause)
            {
                var variableName = letClause.VariableName;
                variables[variableName] = letClause.Expression.Compute (context);
            }
            
            sequences.Add (sequence);
        }
        
        foreach (var clause in _joinClauses)
        {
            for (var i = 0; i < joinRank; i++)
            {
                var variableName = _joinClauses[i].VariableName;

                // переменная не должна упоминаться в родительских контекстах
                Sure.AssertState (!context.Parent!.TryGetVariable (variableName, out _));

                // это должно быть lvalue
                Sure.AssertState (context.EnsureVariableCanBeAssigned (variableName));

                // имя переменной не должно повторяться в текущем контексте
                var count = _fromClauses.Count (x => x.VariableName == variableName);
                Sure.AssertState (count == 1);

                variables[variableName] = null;
            }

            var computed = (IEnumerable<dynamic?>) clause.Sequence.Compute (context)!;
            IReadOnlyList<dynamic?> sequence;
            if (computed is IReadOnlyList<dynamic?> readOnlyList)
            {
                sequence = readOnlyList;
            }
            else
            {
                sequence = computed.ToList();
            }

            sequences.Add (sequence);
        }

        // TODO сделать join

        // in и where (если есть)
        var temporary = new List<IReadOnlyList<dynamic?>>();
        var permuted = Utility.Permute (sequences);
        foreach (var row in permuted)
        {
            for (var i = 0; i < rank; i++)
            {
                variables[_fromClauses[i].VariableName] = row[i];
            }

            var success = _whereClause is null
                || KotikUtility.ToBoolean (_whereClause.Compute (context));
            if (success)
            {
                var copy = new List<dynamic?>();
                copy.AddRange (row);
                temporary.Add (copy);
            }
        }

        // orderby
        if (_orderClause is not null)
        {
            var list = new List<KeyValuePair<dynamic, dynamic>>();
            foreach (var row in temporary)
            {
                for (var i = 0; i < rank; i++)
                {
                    variables[_fromClauses[i].VariableName] = row[i];
                }
                
                var key = _orderClause.Expression.Compute (context);
                var pair = new KeyValuePair<dynamic, dynamic> (key, row);
                list.Add (pair);
            }
        
            if (_orderClause.Descending)
            {
                list.Sort ((left, right) =>
                    OmnipotentComparer.Default.Compare (right.Key, left.Key));
            }
            else
            {
                list.Sort ((left, right) =>
                    OmnipotentComparer.Default.Compare (left.Key, right.Key));
            }
        
            temporary.Clear();
            foreach (var pair in list)
            {
                temporary.Add (pair.Value);
            }
        }

        if (_selectClause is not null)
        {
            var result = new BarsikList();
            foreach (var row in temporary)
            {
                for (var i = 0; i < rank; i++)
                {
                    variables[_fromClauses[i].VariableName] = row[i];
                }

                var selected = _selectClause.Compute (context);
                result.Add (selected);
            }

            return result;
        }

        if (_groupClause is not null)
        {
            var result = new GroupedList();
            foreach (var row in temporary)
            {
                for (var i = 0; i < rank; i++)
                {
                    variables[_fromClauses[i].VariableName] = row[i];
                }

                var key = _groupClause.By.Compute (context);
                var value = _groupClause.Expression.Compute (context);
                result.Add (key, value);
            }

            return result;
        }

        return null;
    }

    #endregion
}
