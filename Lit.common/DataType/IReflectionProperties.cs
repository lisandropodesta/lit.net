namespace Lit.DataType
{
    using System.Collections.Generic;

    public interface IReflectionProperties : IDictionary<string, ReflectionProperty>
    {
    }

    public interface IReflectionProperties<T> : IDictionary<string, ReflectionProperty<T>> where T : class
    {
    }
}
