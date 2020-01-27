using System.Collections.Generic;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Menu object model.
    /// </summary>
    public abstract class CircularMenuObjectModel : BaseModel
    {
        /// <summary>
        /// Showing flag.
        /// </summary>
        public bool IsShowing => show && isVisible && !isHidden;

        /// <summary>
        /// Show control.
        /// </summary>
        public bool Show { get => show; set => SetProp(ref show, value, isVisible && !isHidden ? Change.Visibility : Change.Data, nameof(Show)); }

        private bool show;

        /// <summary>
        /// Visible flag.
        /// </summary>
        public bool IsVisible { get => isVisible; set => SetProp(ref isVisible, value, show && !isHidden ? Change.Visibility : Change.Data, nameof(IsVisible)); }

        private bool isVisible;

        /// <summary>
        /// Hidden flag.
        /// </summary>
        public bool IsHidden { get => isHidden; set => SetProp(ref isHidden, value, show && isVisible ? Change.Visibility : Change.Data, nameof(IsHidden)); }

        private bool isHidden;

        /// <summary>
        /// Starting radius.
        /// </summary>
        public double FromRadius { get => fromRadius; set => SetProp(ref fromRadius, value, Change.Layout, nameof(FromRadius)); }

        private double fromRadius;

        /// <summary>
        /// Ending radius.
        /// </summary>
        public double ToRadius { get => toRadius; set => SetProp(ref toRadius, value, Change.Layout, nameof(ToRadius)); }

        private double toRadius;

        /// <summary>
        /// Relative angle.
        /// </summary>
        public double RelAngle { get => relAngle; set => SetProp(ref relAngle, value, Change.Layout, nameof(RelAngle)); }

        private double relAngle;

        /// <summary>
        /// Rotation angle.
        /// </summary>
        public virtual double Rotation { get => rotation; set => SetProp(ref rotation, value, Change.Layout, nameof(Rotation)); }

        private double rotation;

        /// <summary>
        /// Item size.
        /// </summary>
        public double Size { get => size; set => SetProp(ref size, value, Change.Layout, nameof(Size)); }

        private double size;

        /// <summary>
        /// Starting angle (bigger angle).
        /// </summary>
        public double FromAngle
        {
            get => rotation + relAngle + size / 2;
            set
            {
                var delta = value - FromAngle;
                size += delta;
                SetProp(ref relAngle, relAngle + delta / 2, Change.Layout, nameof(RelAngle));
            }
        }

        /// <summary>
        /// Ending angle (lower angle).
        /// </summary>
        public double ToAngle
        {
            get => rotation + relAngle - size / 2;
            set
            {
                var delta = value - ToAngle;
                size -= delta;
                SetProp(ref relAngle, relAngle + delta / 2, Change.Layout, nameof(RelAngle));
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CircularMenuObjectModel()
        {
            isVisible = true;
            isHidden = false;
        }

        /// <summary>
        /// Closes a list of objects.
        /// </summary>
        protected static void Release<T>(IList<T> list) where T : CircularMenuObjectModel
        {
            if (list != null)
            {
                foreach (var i in list)
                {
                    i.Release();
                }

                list.Clear();
            }
        }
    }
}
