namespace Lit.xamarin
{
    using System.ComponentModel;

    public class VisualProperty<T> : INotifyPropertyChanged
    {
        public T Value { get { return value; } set { SetValue(value); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private T value;

        private void SetValue(T newValue)
        {
            if (!value.Equals(newValue))
            {
                value = newValue;
            }
        }
    }
}
