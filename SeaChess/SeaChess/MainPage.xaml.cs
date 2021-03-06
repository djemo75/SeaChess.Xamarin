using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SeaChess
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async void OnButtonClicked(object sender, EventArgs e)
        {
            var firstPlayer = firstPlayerName.Text;
            var secondPlayer = secondPlayerName.Text;

            if (String.IsNullOrEmpty(firstPlayer) || String.IsNullOrEmpty(secondPlayer))
            {
                await DisplayAlert("Alert", "Please fill the names of the players!", "OK");
                return;
            }

            await Navigation.PushAsync(new GamePage(firstPlayer, secondPlayer));

        }
    }
}
