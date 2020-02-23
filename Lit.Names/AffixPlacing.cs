namespace Lit.Names
{
    /// <summary>
    /// Affix (suffix/prefix) placing.
    /// </summary>
    public enum AffixPlacing
    {
        /// <summary>
        /// Affix not changed.
        /// </summary>
        DoNotChange,

        /// <summary>
        /// Affix not placed.
        /// </summary>
        DoNotPlace,

        /// <summary>
        /// Affix at the begining.
        /// </summary>
        Prefix,

        /// <summary>
        /// Affix at the end.
        /// </summary>
        Sufix,

        /// <summary>
        /// Generates only the affix.
        /// </summary>
        Whole
    }
}
