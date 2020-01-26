namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu category.
    /// </summary>
    public abstract class CircularMenuCategory : CircularMenuObjectModel
    {
        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get => title; set => SetProp(ref title, value, Change.Aspect, nameof(Title)); }

        private string title;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CircularMenuCategory(string title)
        {
            Title = title;
        }
    }
}
