using System.ComponentModel;

namespace Lit.Ui.Classes
{
    /// <summary>
    /// Base data model.
    /// </summary>
    public class BaseModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Generic property assignment.
        /// </summary>
        protected bool SetProp<T>(ref T prop, T value, string name = null, bool layoutChanged = false)
        {
            if (prop == null && value != null || prop != null && !prop.Equals(value))
            {
                prop = value;
                OnPropertyChanged(name, layoutChanged);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Property changed event managment.
        /// </summary>
        protected virtual void OnPropertyChanged(string name, bool layoutChanged)
        {
            if (layoutChanged)
            {
                OnLayoutChanged();
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// A property affecting layout has changed.
        /// </summary>
        protected virtual void OnLayoutChanged()
        {
        }
    }
}
