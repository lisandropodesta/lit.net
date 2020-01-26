using System;
using System.ComponentModel;

namespace Lit.Ui
{
    /// <summary>
    /// Base data model.
    /// </summary>
    public class BaseModel : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Property change affectation.
        /// </summary>
        protected enum Change
        {
            /// <summary>
            /// Change without affecting visual aspect.
            /// </summary>
            Data,

            /// <summary>
            /// Change affecting visibility.
            /// </summary>
            Visibility,

            /// <summary>
            /// Change affecting visual aspect.
            /// </summary>
            Aspect,

            /// <summary>
            /// Change affecting layout/shape.
            /// </summary>
            Layout
        }

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private int updatingCount;

        private Change? updatingChange;

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
                updatingCount++;
            }
        }

        /// <summary>
        /// Holds notifications during an update.
        /// </summary>
        public void EndUpdate()
        {
            Change? flags = null;

            lock (this)
            {
                if (updatingCount > 0)
                {
                    updatingCount--;
                }

                if (updatingCount == 0)
                {
                    flags = updatingChange;
                    updatingChange = null;
                }
            }

            if (flags.HasValue)
            {
                NotifyPropertyChanged(flags.Value, null);
            }
        }

        /// <summary>
        /// Generic property assignment.
        /// </summary>
        protected bool SetProp<T>(ref T prop, T value, Change change, string name = null)
        {
            if (prop == null && value != null || prop != null && !prop.Equals(value))
            {
                prop = value;
                InformPropertyChanged(change, name);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the model is being updated.
        /// </summary>
        protected bool IsUpdating()
        {
            lock (this)
            {
                return updatingCount > 0;
            }
        }

        /// <summary>
        /// Informs that a property has changed.
        /// </summary>
        protected void InformPropertyChanged(Change change, string name)
        {
            bool mustNotify;

            lock (this)
            {
                if (updatingCount > 0)
                {
                    if (!updatingChange.HasValue || updatingChange.Value < change)
                    {
                        updatingChange = change;
                    }

                    mustNotify = false;
                }
                else
                {
                    mustNotify = true;
                }
            }

            if (mustNotify)
            {
                NotifyPropertyChanged(change, name);
            }
        }

        /// <summary>
        /// Notify property changed.
        /// </summary>
        private void NotifyPropertyChanged(Change change, string name)
        {
            OnPropertyChanged(change, name);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Property changed event managment.
        /// </summary>
        protected virtual void OnPropertyChanged(Change change, string name)
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
