using System.Collections.Generic;

namespace Lit.DataType
{
    public class ReflectionProperties : Dictionary<string, ReflectionProperty>, IReflectionProperties
    {
    }

    public class ReflectionProperties<T> : Dictionary<string, ReflectionProperty<T>>, IReflectionProperties<T> where T : class
    {
    }
}
