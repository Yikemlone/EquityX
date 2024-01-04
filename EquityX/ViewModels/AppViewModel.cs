using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace EquityX.ViewModels
{
    public partial class AppViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public static PropertyChangedEventHandler? StaticPropertyChanged;

        private static bool _displayTabs;
        public static bool DisplayTabs
        {
            get => _displayTabs;
            set
            {
                _displayTabs = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(DisplayTabs)));
            }
        }

        public bool InstanceObservedProperty => DisplayTabs;

        public AppViewModel()
        {
            StaticPropertyChanged += HandleStaticPropertyChanged;
        }

        private void HandleStaticPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Debug.Assert(sender is null);
            if (e.PropertyName == nameof(DisplayTabs))
            {
                OnPropertyChanged(nameof(InstanceObservedProperty));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            StaticPropertyChanged -= HandleStaticPropertyChanged;
        }
    }
}
