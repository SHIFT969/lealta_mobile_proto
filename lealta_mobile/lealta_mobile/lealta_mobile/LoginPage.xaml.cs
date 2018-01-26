using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace lealta_mobile
{
    public partial class LoginPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient();

        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            loginEntry.Completed += (s, e) => passwordEntry.Focus();
            passwordEntry.Completed += (s, e) => LogonButtonClicked(s, e);

            //var metrics = Resources.DisplayMetrics;
            //var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);

            //cabinetLabel.FontSize = 
        }

        public async void LogonButtonClicked(object sender, System.EventArgs e)
        {
            var pass = true;
            if (string.IsNullOrWhiteSpace(loginEntry.Text))
            {
                loginFrame.OutlineColor = Color.Red;
                loginFrame.BackgroundColor = Color.FromHex("FFBABA");
                loginNote.TextColor = Color.Red;
                loginNote.Text = "Заполните это поле";
                pass = false;
            }

            if (string.IsNullOrWhiteSpace(passwordEntry.Text))
            {
                pasFrame.OutlineColor = Color.Red;
                pasFrame.BackgroundColor = Color.FromHex("FFBABA");
                pasNote.IsVisible = true;
                pass = false;
            }
            if (!pass) return;

            var login = loginEntry.Text;
            string num = Regex.Replace(login, @"\ |\(|\)|-", "");

            if (await Login())
            {
                var page = new MainPage();
                passwordEntry.Text = "";
                await Navigation.PushAsync(page, true);
            }
        }

        private async Task<bool> Login()
        {
            try
            {
                var login = Regex.Replace(loginEntry.Text, @"[^\d]", "");
                var response = await TryLogin(login); // попытка по логину
                var responseString = await response.Content.ReadAsStringAsync();
                var respObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

                bool passed = true;
                if (respObj.ContainsKey("error"))
                {
                    passed = false;
                    login = login.Remove(0, 1);
                    response = await TryLogin(login); // попытка по телефону, убирается первая цифра
                    responseString = await response.Content.ReadAsStringAsync();
                    respObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);

                    if (respObj.ContainsKey("error"))
                    {
                        passed = false;
                        await DisplayAlert("Ошибка", respObj["error_description"], "OK");
                    }
                    else passed = true;
                }

                if (passed)
                {
                    if (respObj.ContainsKey("access_token"))
                    {
                        object curr = "";
                        var t = new Token();
                        t.AccessToken = respObj["access_token"];
                        t.AssinedAt = DateTime.Now;
                        if (long.TryParse(respObj["expires_in"], out t.ExpiresIn))
                        {
                            t.ExpiresAt = t.AssinedAt.AddSeconds(t.ExpiresIn);
                            if (App.Current.Properties.TryGetValue("token", out curr))
                                App.Current.Properties["token"] = t;
                            else
                                App.Current.Properties.Add("token", t);
                            return true;
                        }
                        else
                        {
                            await DisplayAlert("Ошибка", "Не удалось прочитать ответ сервера", "ОK");
                            return false;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Ошибка", "Ответ сервера не содержит данных", "ОK");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                await (DisplayAlert("Ошибка", e.ToString(), "OK"));
                return false;
            }
        }

        private async Task<HttpResponseMessage> TryLogin(string login)
        {
            var values = new Dictionary<string, string>
            {
                { "username", login },
                { "password", passwordEntry.Text },
                { "grant_type", "password" }
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("http://172.26.26.30/token", content);
            return response;
        }

        private void LoginEntry_Focused(object sender, FocusEventArgs e)
        {
            loginFrame.OutlineColor = Color.Default;
            loginFrame.BackgroundColor = Color.White;
            loginNote.TextColor = Color.FromHex("7C7374");
            loginNote.Text = "Например: 240191, 9601236060";
        }

        private void PasswordEntry_Focused(object sender, FocusEventArgs e)
        {
            pasFrame.OutlineColor = Color.Default;
            pasFrame.BackgroundColor = Color.White;
            pasNote.IsVisible = false;
        }

        private async void RestorePasswordByNumber_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Пароль восстановлен", "СМС-сообщение с паролем отправлено на ваш номер телефона.", "ОК");
            restorePas1.IsVisible = true;
            restorePas2.IsVisible = false;
        }

        private void RestorePasswordButton_Clicked(object sender, EventArgs e)
        {
            restorePas1.IsVisible = false;
            restorePas2.IsVisible = true;
        }

        private void loginEntryIOS_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}
