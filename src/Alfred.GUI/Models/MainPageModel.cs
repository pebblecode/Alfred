using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Alfred.GUI.Models
{
    public class MainPageModel : INotifyPropertyChanged
    {
        private string _source;

        public string Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    OnPropertyChanged();
                }
                
            }
        }

        public MainPageModel(string defaultImage)
        {
            _source = defaultImage;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
