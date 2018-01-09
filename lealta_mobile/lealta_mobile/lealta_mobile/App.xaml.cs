using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace lealta_mobile
{
    public partial class App : Application
    {
        public const string DATABASE_NAME = "contracts.db";
        public static ContractRepo database;
        public static ContractRepo Database
        {
            get
            {
                if (database == null)
                {
                    database = new ContractRepo(DATABASE_NAME);
                    Database.SaveItem(new Contract() {
                        ContractId = "12345",
                        ContractNumber = "+79131554117",
                        Password = "qwerty",
                        Balance = 500.50M,
                        UserName = "Вася Ложкин",
                        Frozen = false});
                    Database.SaveItem(new Contract() {
                        ContractId = "777",
                        ContractNumber = "+71234554117",
                        Password = "1234",
                        Balance = 777.77M,
                        UserName = "Катя Квашина",
                        Frozen = false
                    });
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
