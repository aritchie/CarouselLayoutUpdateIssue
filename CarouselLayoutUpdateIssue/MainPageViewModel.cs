using Microsoft.Maui.Controls;
using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarouselLayoutUpdateIssue
{
	public class MainPageViewModel : INotifyPropertyChanged
    {
        private List<ExampleModel> _installationModels = new List<ExampleModel>();
		public List<ExampleModel> InstallationModels
        {
            get => _installationModels;
            set
            {
                _installationModels = value;
                OnPropertyChanged(nameof(InstallationModels));
            }
        }


        private ExampleModel _selectedItem = new ExampleModel();
        public ExampleModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private int _index = 0;

		public MainPageViewModel()
		{
            _ = LoadJsonDataAsync();

		}

        private async Task LoadJsonDataAsync()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
            Stream? stream = assembly.GetManifestResourceStream("CarouselLayoutUpdateIssue.Resources.ExampleModels.json");
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                var installationModelJson = await reader.ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(installationModelJson)) return;
                InstallationModels = JsonConvert.DeserializeObject<List<ExampleModel>>(installationModelJson);
                Console.WriteLine(InstallationModels.Count);
            }
        }

        private Command? rotateNextCommand;


        public ICommand RotateNextCommand => rotateNextCommand ??= new Command(RotateNext);

        private void RotateNext()
        {
            if(_index < InstallationModels.Count - 1)
            {
                _index++;
                SelectedItem = InstallationModels[_index];
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") =>
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private Command rotateBackCommand;
        public ICommand RotateBackCommand => rotateBackCommand ??= new Command(RotateBack);

        private void RotateBack()
        {
            if (_index > 0)
            {
                _index--;
                SelectedItem = InstallationModels[_index];
            }
        }
    }
}

