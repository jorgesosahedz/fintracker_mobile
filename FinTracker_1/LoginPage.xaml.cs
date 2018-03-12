using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FinTracker_1
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		}

        async private void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TabbedMainPage());
        }
    }
}
