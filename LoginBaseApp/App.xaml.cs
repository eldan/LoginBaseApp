using LoginBaseApp.Models;
using LoginBaseApp.Service;
using LoginBaseApp.ViewModels;
using LoginBaseApp.Views;

namespace LoginBaseApp
{
  public partial class App : Application
  {
    public User? CurrentUser { get; set; }
    Page firstpage;
    IApplicationService _db;
    Task loadSampleData;
  
    public App(IServiceProvider serviceProvider)
    {
      InitializeComponent();
      
      _db = serviceProvider.GetService<IApplicationService>();
      loadSampleData = LoadSampleDataAsync();

      //this.firstpage = serviceProvider.GetService<RegisterPage>();
      this.firstpage = serviceProvider.GetService<LoginPage>();
      //this.firstpage = serviceProvider.GetService<UserListPage>();
    }

    private async Task? LoadSampleDataAsync()
    {
      try
      {
       // await _db.LoadSampleDataAsync().ConfigureAwait(false);
      }
      catch (Exception ex)
      {

      }
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
      return new Window(firstpage);
    }
  }
}
