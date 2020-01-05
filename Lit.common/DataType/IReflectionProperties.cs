using System.Collections.Generic;

namespace Lit.DataType
{
    public interface IReflectionProperties : IDictionary<string, ReflectionProperty>
    {
    }

    public interface IReflectionProperties<T> : IDictionary<string, ReflectionProperty<T>> where T : class
    {
    }
}
