namespace Lit.DataType
{
    using System.Reflection;

    public class ReflectionProperty : ReflectionMemberBase
    {
        /// <summary>
        /// Property information.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        public override MemberInfo MemberInfo { get { return PropertyInfo; } }

        public ReflectionProperty(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }

    public class ReflectionProperty<T> : ReflectionMemberBase<T> where T : class
    {
        /// <summary>
        /// Property information.
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        public override MemberInfo MemberInfo { get { return PropertyInfo; } }

        public ReflectionProperty(PropertyInfo propertyInfo, T attribute)
            : base(attribute)
        {
            PropertyInfo = propertyInfo;
        }
    }
}
