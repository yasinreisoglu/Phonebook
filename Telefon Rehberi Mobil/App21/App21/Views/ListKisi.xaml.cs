using RestSharp;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App21.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListKisi : ContentPage
    {
        CloudData.Conn conn;

        //İşlem yapılacak satırın index'inin tutulması için değişken tanımlanır.
        int idSatir;
        public ListKisi()
        {
            //Kişileri göster Buttonuna tıklandığında ekrana veriler getirilir.

            conn = new CloudData.Conn();
            InitializeComponent();
            var result = conn.userAll();
            listayr.ItemsSource =result;

            //Guncelleme ve listeleme ekranları aynı sayfada olduğundan güncelleme ekranının Visible değeri false yapılır.
            stcGuncelle.IsVisible = false;
        }
        //Her işlemden sonra verilerin yenilenmesi için yenile() metodu.
        public void yenile()
        {
            conn = new CloudData.Conn();
            var result = conn.userAll();
            listayr.ItemsSource = result;
        }
        //Silme Buttonu.
        private void Button_Clicked_Sil(object sender, EventArgs e)
        {
            //Eğer ekranda Herhangi bir satır seçilmediyse ekrana uyarı mesajı verilir.
            if (idSatir == 0)
            {
                DisplayAlert("Uyarı", "Lütfen Bir Satır Seçiniz", "Ok");
            }
            //Silme işlemi.
            else
            {
                var token = new RestClient("http://10.0.2.2:65292/api/token/token?user=deneme&password=110");
                var requesttoken = new RestRequest(Method.POST);
                IRestResponse respo = token.Execute(requesttoken);
                string tokenkey = respo.Content.Replace("\"", "");

                var client = new RestClient("http://10.0.2.2:65292/api/values/?id=" + idSatir + "");
                var request = new RestRequest(Method.DELETE);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Host", "10.0.2.2:65292");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + tokenkey + " ");
                IRestResponse response = client.Execute(request);
                yenile();
                //idSatir değeri sıfıra eşitlenir çünkü başka bir işlem yapıldığında eski idSatir değerini kullanılmaması için.
                idSatir = 0;
            }
        }
        //Listelenen verilerin eventHandler'ı.
        public void Listayr_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Seçilen satırın indexi idSatir değişkenine atanır.
            idSatir = e.SelectedItemIndex;
            idSatir++;            
        }
        //Guncellemek için objeleri açma Buttonu.
        private void Button_Clicked_Guncelle(object sender, EventArgs e)
        {         
            //Satır seçme kontrolü.
            if (idSatir == 0)
            {
                DisplayAlert("Uyarı", "Lütfen Bir Satır Seçiniz", "Ok");
            }
            else
            {
                //Gizlenen objeler gösterilir.
                listayr.IsVisible = false;
                stcSilGuncelle.IsVisible = false;
                stcGuncelle.IsVisible = true;
            }
        }
        //Güncelleme Buttonu.
        private void Button_Clicked_Guncellendi(object sender, EventArgs e)
        {
            //Entry'lerdeki veriler string değişkenlere aktarılır.
            string isim = entisim.Text;
            string soyisim = entsoyisim.Text;
            string telefon = enttelefon.Text;
            string telefon2 = enttelefon2.Text;
            string adres = entadres.Text;
            string eposta = enteposta.Text;
            //Entry'lerin dolu olup olmadığı kontrol edilir.
            if (entisim.Text == null || entsoyisim.Text == null || enttelefon.Text == null || entadres.Text == null || enteposta.Text == null)
            {
                DisplayAlert("Uyarı", "Lütfen bütün kutucukları doldurunuz", "Ok");
            }
            else
            {

                var token = new RestClient("http://10.0.2.2:65292/api/token/token?user=deneme&password=110");
                var requesttoken = new RestRequest(Method.POST);
                IRestResponse respo = token.Execute(requesttoken);
                string tokenkey = respo.Content.Replace("\"", "");

                //Günceleme işlemi yapılır.
                var client = new RestClient("http://10.0.2.2:65292/api/values/1?id=" + idSatir + "&Isim=" + isim + "&Soyisim=" + soyisim + "&Telefon=" + telefon + "&Telefon2=" + telefon2 + "&Adres=" + adres + "&Eposta=" + eposta + "");
                var request = new RestRequest(Method.PUT);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Host", "10.0.2.2:65292");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Authorization", "Bearer " + tokenkey + "");
                IRestResponse response = client.Execute(request);
                //Güncelleme işleminden sonra listelenen veriler için gereken objeler gösterilir. 
                listayr.IsVisible = true;
                stcSilGuncelle.IsVisible = true;
                stcGuncelle.IsVisible = false;
                yenile();
                //Entry'ler temizlenir.
                entisim.Text = string.Empty;
                entsoyisim.Text = string.Empty;
                enttelefon.Text = string.Empty;
                enttelefon2.Text = string.Empty;
                entadres.Text = string.Empty;
                enteposta.Text = string.Empty;
                //Eski idSatir değeri sıfıra eşitlenir.
                idSatir = 0;
            }
        }
    }
}