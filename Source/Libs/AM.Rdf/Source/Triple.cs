namespace AM.Rdf
{
    /// <summary>
    /// Триплет.
    /// </summary>
    public sealed class Triple
    {
        #region Properties

        public INode Subject { get; }

        public INode Predicate { get; }

        public INode Object { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Triple
            (
                INode subject,
                INode predicate,
                INode obj
            )
        {
            Subject = subject;
            Predicate = predicate;
            Object = obj;
        }

        #endregion
    }
}
