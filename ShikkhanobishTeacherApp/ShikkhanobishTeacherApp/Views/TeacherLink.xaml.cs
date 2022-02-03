using ShikkhanobishTeacherApp.View_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShikkhanobishTeacherApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeacherLink : ContentPage
    {
        public TeacherLink()
        {
            InitializeComponent();
            //BindingContext = new TeacherLinkViewModel(chapid);
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}