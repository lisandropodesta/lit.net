using System.Reflection;

namespace Lit.DataType
{
    /// <summary>
    /// Binding to a class's property with attributes.
    /// </summary>
    public class AttrPropertyBinding<TC, TP, TA> : PropertyBinding<TC, TP>, IAttrPropertyBinding<TA> where TC : class
    {
        /// <summary>
        /// Attributes.
        /// </summary>
        public TA Attributes { get; private set; }

        #region Constructor

        public AttrPropertyBinding(PropertyInfo propInfo, TA attr, bool getterRequired, bool setterRequired)
            : base(propInfo, getterRequired, setterRequired)
        {
            this.Attributes = attr;
        }

        #endregion
    }
}
