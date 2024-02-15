using Newtonsoft.Json;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarouselLayoutUpdateIssue;


public class MainPageViewModel : ViewModel
{
	public MainPageViewModel()
	{
        RotateNextCommand = new Command(RotateNext);
        RotateBackCommand = new Command(RotateBack);
        FlushCacheAndReload = new Command(() =>
        {
            var cache = new DirectoryInfo(FileSystem.CacheDirectory);
            var cacheFiles = cache.GetFiles("*.png", SearchOption.AllDirectories);
            foreach (var file in cacheFiles)
                file.Delete();

            _index = 0;
            SetOnIndex();
        });
    }


    public ICommand RotateBackCommand { get; }
    public ICommand RotateNextCommand { get; }
    public ICommand FlushCacheAndReload { get; }


    List<ItemViewModel> _installationModels = new();
    public List<ItemViewModel> InstallationModels
    {
        get => _installationModels;
        private set
        {
            _installationModels = value;
            OnPropertyChanged();
        }
    }


    ItemViewModel _selectedItem = null!;
    public ItemViewModel SelectedItem
    {
        get => _selectedItem;
        private set
        {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    private int _index = 0;

    public void OnAppearing()
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
            if (string.IsNullOrWhiteSpace(installationModelJson))
                return;

            InstallationModels = JsonConvert.DeserializeObject<List<ItemViewModel>>(installationModelJson)!;
            Console.WriteLine(InstallationModels.Count);
        }
    }


    private void RotateNext()
    {
        if(_index < InstallationModels.Count - 1)
        {
            _index++;
            SetOnIndex();
        }
    }

    private void RotateBack()
    {
        if (_index > 0)
        {
            _index--;
            SetOnIndex();
        }
    }

    void SetOnIndex()
    {
        if (SelectedItem != null)
            SelectedItem.IsVisible = false;

        SelectedItem = InstallationModels[_index];
        SelectedItem.IsVisible = true;
    }
}

public class ItemViewModel : ViewModel
{
    bool _isVisible;
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            _isVisible = value;
            OnPropertyChanged();
        }
    }

    string _imageUri = null!;
    public string ImageURL
    {
        get => _imageUri;
        set
        {
            _imageUri = value;
            OnPropertyChanged();
        }
    }
}


public abstract class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}