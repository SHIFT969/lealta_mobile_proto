using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace lealta_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            var profileSwitchRecognizer = new TapGestureRecognizer();
            profileSwitchRecognizer.Tapped += (s, e) => OpenProfileTab(s, e);
            profileSwitchGroup.GestureRecognizers.Add(profileSwitchRecognizer);

            var financesSwitchRecognizer = new TapGestureRecognizer();
            financesSwitchRecognizer.Tapped += (s, e) => OpenFinancesTab(s, e);
            financesSwitchGroup.GestureRecognizers.Add(financesSwitchRecognizer);

            object curr = new Contract();
            if (App.Current.Properties.TryGetValue("currentContract", out curr))
            {
                var contract = curr as Contract;
                contactData.Text = contract.Adress;
                balance.Text = contract.Balance.ToString("0.00");
                contractNumber.Text = contract.ContractNumber.ToString();
                contractRate.Text = contract.Rate;
            }

            var a = new TapGestureRecognizer();

            //ChandePassControl
        }

        public void OpenProfileTab(object sender, System.EventArgs e)
        {
            if (profileTab.IsVisible)
            {
                profileTab.IsVisible = false;
                profileSwitch.Source = "arrow_in_circle_point_to_right24.png";
                profileSwitchLabel.TextColor = Color.FromHex("#F45719");
            }
            else
            {
                profileTab.IsVisible = true;
                profileSwitch.Source = "arrow_in_circle_point_up_black24.png";
                profileSwitchLabel.TextColor = Color.Black;
            }
        }

        public void OpenFinancesTab(object sender, System.EventArgs e)
        {
            if (financesTab.IsVisible)
            {
                financesTab.IsVisible = false;
                financesSwitch.Source = "arrow_in_circle_point_to_right24.png";
                financesSwitchLabel.TextColor = Color.FromHex("#F45719");
            }
            else
            {
                financesTab.IsVisible = true;
                financesSwitch.Source = "arrow_in_circle_point_up_black24.png";
                financesSwitchLabel.TextColor = Color.Black;
            }
        }

        private async void ExitButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}