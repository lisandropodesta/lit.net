using System.Reflection;

namespace Lit.DataType
{
    public abstract class ReflectionMemberBase
    {
        /// <summary>
        /// Member information.
        /// </summary>
        public abstract MemberInfo MemberInfo { get; }

        public override string ToString()
        {
            return string.Format("[{0}.{1}]", MemberInfo.DeclaringType?.Name, MemberInfo.Name);
        }
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

        public override string ToString()
        {
            return string.Format("[{0}.{1}]", MemberInfo.DeclaringType?.Name, MemberInfo.Name);
        }
    }
}
