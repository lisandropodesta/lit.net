namespace Lit.DataType
{
    /// <summary>
    /// Binding to a class's property with attributes.
    /// </summary>
    public interface IAttrPropertyBinding<TA> : IPropertyBinding
    {
        /// <summary>
        /// Attributes.
        /// </summary>
        TA Attributes { get; }
    }
}
