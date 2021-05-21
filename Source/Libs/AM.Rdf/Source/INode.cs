namespace AM.Rdf
{
    /// <summary>
    /// Интерфейс узла
    /// </summary>
    public interface INode
    {
        /// <summary>
        ///
        /// </summary>
        NodeType NodeType { get; }

        /// <summary>
        ///
        /// </summary>
        IGraph Graph { get; }
    }
}
