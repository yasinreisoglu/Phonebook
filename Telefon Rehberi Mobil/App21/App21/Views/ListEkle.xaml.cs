using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RestSharp;

namespace App21.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListEkle : ContentPage
    {
        public ListEkle()
        {
            InitializeComponent();
        }
        
        //Kisi Ekleme Buttonu.
        private async void Ekle_Clicked(object sender, EventArgs e)
        {
            //Entry'lerin dolu olup olmadığı kontrol edilir.
            if (entisim.Text == null || entsoyisim.Text == null || enttelefon.Text == null || entadres.Text == null || enteposta.Text == null)
            {
                await DisplayAlert("Uyarı", "Lütfen bütün kutucukları doldurunuz", "Ok");
                enttelefon2.Text = "2. Telefon yok";
            }
            else
            {
                //Entry'ler stringe dönüştürülür.
                string txtisim = entisim.Text;
                string txtsoyisim = entsoyisim.Text;
                string txttelefon = enttelefon.Text;
                string txttelefon2 = enttelefon2.Text;
                string txtadres = entadres.Text;
                string txteposta = enteposta.Text;
                var token = new RestClient("http://10.0.2.2:65292/api/token/token?user=deneme&password=110");
                var requesttoken = new RestRequest(Method.POST);
                IRestResponse respo = token.Execute(requesttoken);
                string tokenkey = respo.Content.Replace("\"", "");
                //Restclient ile Post işlemi gerçekleştirilir
                var client = new RestClient("http://10.0.2.2:65292/api/values?Isim=" + txtisim + "&Soyisim=" + txtsoyisim + "&Telefon=" + txttelefon + "&Telefon2=" + txttelefon2 + "&Adres=" + txtadres + "&Eposta=" + txteposta + "");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Host", "10.0.2.2:65292");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer "+ tokenkey + "");
                IRestResponse response = client.Execute(request);
                
            } 
        }
    }
}
