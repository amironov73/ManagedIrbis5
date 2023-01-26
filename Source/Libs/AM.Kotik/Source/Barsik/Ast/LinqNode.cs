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

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/*

  from score in scores //required
  where score > 80 // optional
  orderby score // optional
  select score; //must end with select or group

 */

/// <summary>
/// Жалкое подобие LINQ.
/// </summary>
internal sealed class LinqNode
    : AtomNode
{
    #region NestedClasses

    public record OrderClause (AtomNode Expression, bool Descending);

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LinqNode
        (
            string variableName,
            AtomNode sequence,
            AtomNode? whereClause,
            OrderClause? orderClause,
            AtomNode selectClause
        )
    {
        Sure.NotNullNorEmpty (variableName);
        Sure.NotNull (sequence);
        Sure.NotNull (selectClause);

        _variableName = variableName;
        _sequence = sequence;
        _whereClause = whereClause;
        _orderClause = orderClause;
        _selectClause = selectClause;
    }

    #endregion

    #region Private members

    private readonly string _variableName;
    private readonly AtomNode _sequence;
    private readonly AtomNode? _whereClause;
    private readonly OrderClause? _orderClause;
    private readonly AtomNode _selectClause;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        var sequence = _sequence.Compute (context);
        if (sequence is not IEnumerable)
        {
            sequence = new [] { sequence };
        }

        context = context.CreateChildContext();
        var variables = context.Variables;
        variables[_variableName] = null;

        // in и where (если есть)
        var temporary = new BarsikList();
        foreach (var item in sequence)
        {
            variables[_variableName] = item;
            var success = _whereClause is null
                || _whereClause.Compute (context);
            if (success)
            {
                temporary.Add (item);
            }
        }

        // orderby
        if (_orderClause is not null)
        {
            var list = new List<KeyValuePair<dynamic, dynamic>>();
            foreach (var one in temporary)
            {
                variables[_variableName] = one;
                var key = _orderClause.Expression.Compute (context);
                var pair = new KeyValuePair<dynamic, dynamic> (key, one);
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

        var result = new BarsikList();
        foreach (var one in temporary)
        {
            variables[_variableName] = one;
            var selected = _selectClause.Compute (context);
            result.Add (selected);
        }

        return result;
    }

    #endregion
}
