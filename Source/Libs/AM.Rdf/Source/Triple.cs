namespace AM.Rdf
{
    /// <summary>
    /// Триплет.
    /// </summary>
    public sealed class Triple
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public INode Subject { get; }

        /// <summary>
        ///
        /// </summary>
        public INode Predicate { get; }

        /// <summary>
        ///
        /// </summary>
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
