﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refractored.XamForms.PullToRefresh;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

namespace lealta_mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient();

        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            _refreshCommand = new Command(UpdateData);
            InitializeComponent();
            PTRLayout.RefreshCommand = RefreshCommand;

            var profileSwitchRecognizer = new TapGestureRecognizer();
            profileSwitchRecognizer.Tapped += (s, e) => OpenProfileTab(s, e);
            profileSwitchGroup.GestureRecognizers.Add(profileSwitchRecognizer);

            var financesSwitchRecognizer = new TapGestureRecognizer();
            financesSwitchRecognizer.Tapped += (s, e) => OpenFinancesTab(s, e);
            financesSwitchGroup.GestureRecognizers.Add(financesSwitchRecognizer);

            UpdateData();
        }

        Command _refreshCommand;
        public Command RefreshCommand
        {
            get
            {
                return _refreshCommand;
            }
        }

        private async void UpdateData()
        {
            try
            {
                PTRLayout.IsRefreshing = true;
                object t = new Token();
                if (App.Current.Properties.TryGetValue("token", out t))
                {
                    Token token = t as Token;
                    if (!await Expired(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
                        var response = await client.GetAsync("http://172.26.26.30/api/data/clientinfo");

                        if (response.IsSuccessStatusCode)
                        {
                            var responseString = await response.Content.ReadAsStringAsync();
                            var respObj = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(responseString);
                            if (respObj.ContainsKey("data"))
                            {
                                var data = respObj["data"];
                                contactData.Text = data["address"];
                                balance.Text = decimal.Parse(data["balance"], CultureInfo.InvariantCulture).ToString("0.00");
                                //contractNumber.Text = contract.ContractNumber.ToString();
                                contractRate.Text = data["tarif"];
                            }
                            else await Exit();
                        }
                        else await Exit();
                    }
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Ошибка", e.ToString(), "OK");
            }
            finally
            {
                PTRLayout.IsRefreshing = false;
            }
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
            await Exit();
        }

        private async Task<bool> Expired(Token t)
        {
            if (t.ExpiresAt <= DateTime.Now)
            {
                await Navigation.PopAsync();
                return true;
            }
            else return false;
        }

        private async Task Exit()
        {
            if (App.Current.Properties.ContainsKey("token"))
                App.Current.Properties.Remove("token");
            await Navigation.PopAsync();
        }
    }
}