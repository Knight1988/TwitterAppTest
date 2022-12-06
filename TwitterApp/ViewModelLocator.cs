using Microsoft.Extensions.DependencyInjection;
using TwitterApp.ViewModels;

namespace TwitterApp;

public class ViewModelLocator
{
    public MainViewModel MainViewModel => App.ServiceProvider.GetService<MainViewModel>();
}