namespace Lit.DataType
{
    using System.Reflection;

    public class ReflectionMember : ReflectionMemberBase
    {
        public override MemberInfo MemberInfo { get { return memberInfo; } }

        private readonly MemberInfo memberInfo;

        public ReflectionMember(MemberInfo memberInfo)
        {
            this.memberInfo = memberInfo;
        }
    }

    public class ReflectionMember<T> : ReflectionMemberBase<T> where T : class
    {
        public override MemberInfo MemberInfo { get { return memberInfo; } }

        private readonly MemberInfo memberInfo;

        public ReflectionMember(MemberInfo memberInfo, T attribute)
            : base(attribute)
        {
            this.memberInfo = memberInfo;
        }
    }
}
