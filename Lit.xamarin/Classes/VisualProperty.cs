namespace Lit.xamarin
{
    using System.ComponentModel;
    using Xamarin.Forms;

    public abstract class VisualProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected View View { get; private set; }

        public void Bind(View view)
        {
            View = view;
        }

        public abstract object GetData();

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(View, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class VisualProperty<T> : VisualProperty
    {
        public T Value { get { return value; } set { SetValue(value); } }

        private T value;

        public override object GetData()
        {
            return value;
        }

        private void SetValue(T newValue)
        {
            if (value == null && newValue != null || value != null && !value.Equals(newValue))
            {
                value = newValue;

                OnPropertyChanged(null);
            }
        }
    }
}
