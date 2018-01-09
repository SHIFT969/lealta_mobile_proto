using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace lealta_mobile
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            loginEntry.Completed += (s, e) => passwordEntry.Focus();
            passwordEntry.Completed += (s, e) => LogonButtonClicked(s, e);
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
            //Contract acc = null;
            Contract acc = new Contract()
            {
                ContractId = "123",
                ContractNumber = "+79131554117",
                Password = "qwe",
                Balance = 500.50M,
                UserName = "Вася Ложкин",
                Rate = "План А",
                Frozen = false,
                Adress = "Гражданская ул., дом 5, кв. 12"
            };
            if (login == acc.ContractId && passwordEntry.Text == acc.Password)
            {
                object curr = new Contract();
                if (App.Current.Properties.TryGetValue("currentContract", out curr))
                    App.Current.Properties["currentContract"] = acc;
                else
                    App.Current.Properties.Add("currentContract", acc);

                var page = new MainPage();
                await Navigation.PushAsync(page, true);
            }

            //try
            //{
            //    if (await App.Database.CheckNumber(num))
            //        acc = await App.Database.GetItemByNumberPas(num, passwordEntry.Text);
            //    else
            //        acc = await App.database.GetItemByIdPas(login, passwordEntry.Text);
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("Ошибка", ex.Message, "ОK");
            //    return;
            //}

            //await DisplayAlert("Добро пожаловать", $"{acc.UserName}, ваш баланс {acc.Balance}, договор {(acc.Frozen ? "" : "не")} заморожен", "ОK");
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
    }
}
