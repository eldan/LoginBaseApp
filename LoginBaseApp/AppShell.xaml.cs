using LoginBaseApp.ViewModels;
using LoginBaseApp.Views;

namespace LoginBaseApp
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel vm)
        {
            InitializeComponent();
            
            // xamlרישום של דפים באופן תכנותי שלא מופיעים ב 
            Routing.RegisterRoute("UserDetailPage", typeof(UserDetailPage));
  

            BindingContext = vm;
    }
  }
}
