using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        public enum Change
        {
            /// <summary>
            /// Nothing changed.
            /// </summary>
            None,

            /// <summary>
            /// Change without affecting visual aspect.
            /// </summary>
            Data,

            /// <summary>
            /// Change affecting visual aspect.
            /// </summary>
            Aspect,

            /// <summary>
            /// Change affecting layout/shape.
            /// </summary>
            Layout,

            /// <summary>
            /// Change affecting visibility.
            /// </summary>
            Visibility
        }

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly object changedLock = new object();

        private Change changedState;

        private ushort changedCounter;

        /// <summary>
        /// Begins changing an object, avoid sending notifications until changes are finished.
        /// </summary>
        public void BeginChange()
        {
            lock (changedLock)
            {
                if (changedCounter != 0xFFFF)
                {
                    changedCounter++;
                }
            }
        }

        /// <summary>
        /// End changing an object, if there are pending notifications they are delivered.
        /// </summary>
        public Change EndChange()
        {
            Change changed;

            lock (changedLock)
            {
                if (changedCounter == 0 || --changedCounter != 0 || changedState == Change.None)
                {
                    return changedState;
                }

                changed = changedState;
                changedState = Change.None;
            }

            NotifyPropertyChanged(changed, null);
            return changed;
        }

        /// <summary>
        /// Check if the model is being changed.
        /// </summary>
        protected Change GetChanging()
        {
            lock (changedLock)
            {
                return changedState;
            }
        }

        /// <summary>
        /// Generic property assignment.
        /// </summary>
        protected bool SetProp<T>(ref T prop, T value, Change change, [CallerMemberName] string name = null)
        {
            if (prop == null && value != null || prop != null && !prop.Equals(value))
            {
                prop = value;
                ReportPropertyChanged(change, name);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reports a property has changed.
        /// </summary>
        protected void ReportPropertyChanged(Change change, string name)
        {
            lock (changedLock)
            {
                if (changedCounter > 0)
                {
                    if (changedState < change)
                    {
                        changedState = change;
                    }

                    return;
                }
            }

            NotifyPropertyChanged(change, name);
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
        /// Dispose allocated resources but the element still can be activated.
        /// </summary>
        public void Dispose()
        {
            Release();
        }

        /// <summary>
        /// Release allocated resources.
        /// </summary>
        protected virtual void Release()
        {
        }
    }
}
