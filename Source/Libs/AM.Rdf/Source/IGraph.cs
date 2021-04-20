namespace AM.Rdf
{
    /// <summary>
    /// Интерфейс графа.
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Добавляет триплет в граф.
        /// </summary>
        bool Assert(Triple triple);

        /// <summary>
        /// Удаляет триплет из графа.
        /// </summary>
        bool Retract(Triple triple);
    }
}
