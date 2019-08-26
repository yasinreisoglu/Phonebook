using Android.App;
using System;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace App21.Views
{

    public partial class ListApi : ContentPage
    {

        

        public ListApi()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();     
        }

        private void KisileriGoster_Clicked(object sender, EventArgs e)
        {
            //Kişileri göster Buttonuna tıklandığında ListKisi sayfasına yönlendirilir.
            Navigation.PushAsync(new ListKisi());
        }

        private void KisiEkle_Clicked(object sender, EventArgs e)
        {
            //Kişi Ekle Buttonuna tıklandığında ListEkle sayfasına yönlendirilir.
            Navigation.PushAsync(new ListEkle());
        }

        protected override bool OnBackButtonPressed()
        {
                Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Uyarı!", "Çıkış yapmak istediğine emin misin?", "Evet", "Hayır");
                if (result)
                    {
                        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                    }
                });
                return true;
        }

    }
}