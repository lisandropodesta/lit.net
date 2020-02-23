namespace Lit.Names
{
    /// <summary>
    /// Naming case.
    /// </summary>
    public enum Case
    {
        /// <summary>
        /// Unspecified.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Camel case (thisIsAnExample).
        /// </summary>
        Camel,

        /// <summary>
        /// Pascal case (ThisIsAnExample).
        /// </summary>
        Pascal,

        /// <summary>
        /// Snake case (this_is_an_example).
        /// </summary>
        Snake,

        /// <summary>
        /// Upper snake case (THIS_IS_AN_EXAMPLE).
        /// </summary>
        UpperSnake,

        /// <summary>
        /// Kebab case (this-is-an-example).
        /// </summary>
        Kebab,

        /// <summary>
        /// Upper kebab case (THIS-IS-AN-EXAMPLE).
        /// </summary>
        UpperKebab
    }
}
