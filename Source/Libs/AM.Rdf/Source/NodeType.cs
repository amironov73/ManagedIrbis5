namespace AM.Rdf
{
    /// <summary>
    /// Типы узлов.
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// Пустой.
        /// </summary>
        Blank = 0,

        /// <summary>
        /// URI.
        /// </summary>
        Uri = 1,

        /// <summary>
        /// Литерал.
        /// </summary>
        Literal = 2,

        /// <summary>
        /// Графовый литерал.
        /// </summary>
        GraphLiteral = 3,

        /// <summary>
        /// Переменная.
        /// </summary>
        Variable = 4
    }
}
