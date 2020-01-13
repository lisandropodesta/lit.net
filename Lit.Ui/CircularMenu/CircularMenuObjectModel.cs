using Lit.Ui.Classes;

namespace Lit.Ui.CircularMenu
{
    /// <summary>
    /// Menu object model.
    /// </summary>
    public abstract class CircularMenuObjectModel : BaseModel
    {
        /// <summary>
        /// Displayed flag.
        /// </summary>
        public bool IsDisplayed => isVisible && !isHidden;

        /// <summary>
        /// Visible flag.
        /// </summary>
        public bool IsVisible { get => isVisible; set => SetProp(ref isVisible, value, nameof(IsVisible), !isHidden); }

        private bool isVisible;

        /// <summary>
        /// Hidden flag.
        /// </summary>
        public bool IsHidden { get => isHidden; set => SetProp(ref isHidden, value, nameof(IsHidden), isVisible); }

        private bool isHidden;

        /// <summary>
        /// Starting radius.
        /// </summary>
        public double FromRadius { get => fromRadius; set => SetProp(ref fromRadius, value, nameof(FromRadius), true); }

        private double fromRadius;

        /// <summary>
        /// Ending radius.
        /// </summary>
        public double ToRadius { get => toRadius; set => SetProp(ref toRadius, value, nameof(ToRadius), true); }

        private double toRadius;

        /// <summary>
        /// Relative angle.
        /// </summary>
        public double RelAngle { get => relAngle; set => SetProp(ref relAngle, value, nameof(RelAngle), true); }

        private double relAngle;

        /// <summary>
        /// Rotation angle.
        /// </summary>
        public virtual double Rotation { get => rotation; set => SetProp(ref rotation, value, nameof(Rotation), true); }

        private double rotation;

        /// <summary>
        /// Item size.
        /// </summary>
        public double Size { get => size; set => SetProp(ref size, value, nameof(Size), true); }

        private double size;

        /// <summary>
        /// Starting angle (bigger angle).
        /// </summary>
        public double FromAngle
        {
            get => rotation + relAngle + size / 2;
            set
            {
                var delta = (value - FromAngle) / 2;
                size += delta / 2;
                SetProp(ref relAngle, relAngle + delta / 2, nameof(RelAngle), true);
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
                var delta = (value - ToAngle) / 2;
                size -= delta / 2;
                SetProp(ref relAngle, relAngle + delta / 2, nameof(RelAngle), true);
            }
        }
    }
}
