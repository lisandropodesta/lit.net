namespace Lit.DataType
{
    public enum BindingMode
    {
        /// <summary>
        /// No binding.
        /// </summary>
        None,

        /// <summary>
        /// Single value binding.
        /// </summary>
        Scalar,

        /// <summary>
        /// Class binding.
        /// </summary>
        Class,

        /// <summary>
        /// Class list binding.
        /// </summary>
        ClassList,

        /// <summary>
        /// List binding.
        /// </summary>
        List,

        /// <summary>
        /// Properties by name binding.
        /// </summary>
        Dictionary
    }
}
