namespace AM.Rdf
{
    /// <summary>
    /// Интерфейс узла
    /// </summary>
    public interface INode
    {
        NodeType NodeType { get; }

        IGraph Graph { get; }
    }
}
