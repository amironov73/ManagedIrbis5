using System;

namespace AM.Rdf
{
    /// <summary>
    /// Интерфейс хранилища триплетов.
    /// </summary>
    public interface ITripleStore
    {
        bool Add(IGraph graph);

        bool HasGraph(Uri graphUri);

        IGraph this[Uri graphUri] { get; }
    }
}
