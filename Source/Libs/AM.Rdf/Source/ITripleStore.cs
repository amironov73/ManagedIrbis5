using System;

namespace AM.Rdf
{
    /// <summary>
    /// Интерфейс хранилища триплетов.
    /// </summary>
    public interface ITripleStore
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        bool Add(IGraph graph);

        /// <summary>
        ///
        /// </summary>
        /// <param name="graphUri"></param>
        /// <returns></returns>
        bool HasGraph(Uri graphUri);

        /// <summary>
        ///
        /// </summary>
        /// <param name="graphUri"></param>
        IGraph this[Uri graphUri] { get; }
    }
}
