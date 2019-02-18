namespace Lit.xamarin
{
    using Lit.DataType;
    using Xamarin.Forms;

    public class ControlEvent
    {
        /// <summary>
        /// The object that produced the event.
        /// </summary>
        public Element Element { get; set; }

        /// <summary>
        /// Property information.
        /// </summary>
        public ReflectionProperty<LitUiAttribute> Property { get; set; }
    }
}
