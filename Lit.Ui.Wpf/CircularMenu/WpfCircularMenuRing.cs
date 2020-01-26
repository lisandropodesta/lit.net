using Lit.Ui.CircularMenu;
using Lit.Ui.Wpf.Shapes;

namespace Lit.Ui.Wpf.CircularMenu
{
    /// <summary>
    /// WPF circular menu ring.
    /// </summary>
    public class WpfCircularMenuRing : CircularMenuRing<WpfCircularMenuItem>
    {
        public WpfCircularMenu WpfMenu => wpfMenu;

        private readonly WpfCircularMenu wpfMenu;

        private readonly WpfRingSector ringSector = new WpfRingSector();

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfCircularMenuRing(WpfCircularMenu menu) : base(menu)
        {
            wpfMenu = menu;
        }

        /// <summary>
        /// Process layout change event.
        /// </summary>
        protected override void OnPropertyChanged(Change change, string name)
        {
            base.OnPropertyChanged(change, name);

            if (change >= Change.Visibility)
            {
                if (change >= Change.Visibility)
                {
                    ringSector.Show(this, wpfMenu.Canvas, wpfMenu.Position);
                }

                Items.ForEach(i => i.BeginUpdate());

                Items.ForEach(item =>
                {
                    (item as WpfCircularMenuItem).WpfRing = this;
                    item.IsShowing = IsShowing;
                    item.FromRadius = FromRadius;
                    item.ToRadius = ToRadius;
                });

                Items.ForEach(i => i.EndUpdate());
            }
        }

        /// <summary>
        /// Creates a category.
        /// </summary>
        protected override CircularMenuCategory CreateCategory(string title)
        {
            return new WpfCircularMenuCategory(this, title);
        }
    }
}
