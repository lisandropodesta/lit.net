namespace Lit.DataType
{
    using System.Collections.Generic;

    public class ReflectionProperties : Dictionary<string, ReflectionProperty>, IReflectionProperties
    {
    }

    public class ReflectionProperties<T> : Dictionary<string, ReflectionProperty<T>>, IReflectionProperties<T> where T : class
    {
    }
}
