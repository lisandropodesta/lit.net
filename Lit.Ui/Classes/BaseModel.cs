using System;
using System.ComponentModel;

namespace Lit.Ui.Classes
{
    /// <summary>
    /// Base data model.
    /// </summary>
    public class BaseModel : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private int updateCount;

        private bool flagChanged;

        private bool flagLayoutChanged;

        /// <summary>
        /// Dispose allocated resources but the element still can be activated.
        /// </summary>
        public void Dispose()
        {
            Release();
        }

        /// <summary>
        /// Holds notifications during an update.
        /// </summary>
        public void BeginUpdate()
        {
            lock (this)
            {
                updateCount++;
            }
        }

        /// <summary>
        /// Holds notifications during an update.
        /// </summary>
        public void EndUpdate()
        {
            var changed = false;
            var layoutChanged = false;

            lock (this)
            {
                if (updateCount > 0)
                {
                    updateCount--;
                }

                if (updateCount == 0)
                {
                    changed = flagChanged;
                    layoutChanged = flagLayoutChanged;
                    flagChanged = false;
                    flagLayoutChanged = false;
                }
            }

            if (changed)
            {
                NotifyPropertyChanged(null, layoutChanged);
            }
        }

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
            bool mustNotify;

            lock (this)
            {
                if (updateCount > 0)
                {
                    flagChanged = true;
                    flagLayoutChanged = flagLayoutChanged || layoutChanged;
                    mustNotify = false;
                }
                else
                {
                    mustNotify = true;
                }
            }

            if (mustNotify)
            {
                NotifyPropertyChanged(name, layoutChanged);
            }
        }

        /// <summary>
        /// Notify property changed.
        /// </summary>
        private void NotifyPropertyChanged(string name, bool layoutChanged)
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

        /// <summary>
        /// Release allocated resources.
        /// </summary>
        protected virtual void Release()
        {
        }
    }
}
