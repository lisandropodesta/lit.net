namespace Lit.DataType
{
    /// <summary>
    /// Property binding.
    /// </summary>
    public interface IPropertyBinding
    {
        object GetValue(object instance);

        void SetValue(object instance, object value);
    }
}
