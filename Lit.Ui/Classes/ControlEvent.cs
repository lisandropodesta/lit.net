namespace Lit.Ui
{
    using Lit.DataType;

    public class ControlEvent
    {
        /// <summary>
        /// The object that produced the event.
        /// </summary>
        public object Sender { get; set; }

        /// <summary>
        /// Command name.
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// Command parameter.
        /// </summary>
        public string CommandParameter { get; set; }

        /// <summary>
        /// Property information.
        /// </summary>
        public ReflectionProperty<LitUiAttribute> Property { get; set; }
    }
}
