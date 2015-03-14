using System.ComponentModel;

namespace AliveCheckerService.Classes
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INPC implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void RaisePropertyChange(string propertyName)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
