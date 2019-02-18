namespace Lit.DataType
{
    using System.Reflection;

    public abstract class ReflectionMemberBase
    {
        /// <summary>
        /// Member information.
        /// </summary>
        public abstract MemberInfo MemberInfo { get; }
    }

    public abstract class ReflectionMemberBase<T> : ReflectionMemberBase where T : class
    {
        /// <summary>
        /// Associated attribute.
        /// </summary>
        public T Attribute { get; private set; }

        public ReflectionMemberBase(T attribute)
        {
            Attribute = attribute;
        }
    }
}
