namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Circular menu category model.
    /// </summary>
    public abstract class CircularMenuCategoryModel : CircularMenuObjectModel
    {
        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get => title; set => SetProp(ref title, value, nameof(Title)); }

        private string title;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CircularMenuCategoryModel(string title)
        {
            Title = title;
        }
    }
}
