namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Menu category model.
    /// </summary>
    public abstract class MenuCategoryModel : MenuObjectModel
    {
        /// <summary>
        /// Title.
        /// </summary>
        public string Title { get => title; set => SetProp(ref title, value, nameof(Title)); }

        private string title;

        /// <summary>
        /// Constructor.
        /// </summary>
        protected MenuCategoryModel(string title)
        {
            Title = title;
        }
    }
}
