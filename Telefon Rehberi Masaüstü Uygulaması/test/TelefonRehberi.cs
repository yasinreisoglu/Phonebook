using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace test
{
   
    public partial class TelefonRehberi : Form
    {
        public TelefonRehberi()
        {
            InitializeComponent();
        }
        //Mysql bağlantısı için gereken değişken oluşturulur.
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=deneme;Uid=root;Pwd=''");    
        private void TelefonRehberi_Load(object sender, EventArgs e)
        {
            //Program açıldığında ekrana verilerin gelmesi için Yenile fonksiyonu çağırılır.
            Yenile();
        }
        private void Yenile()
        {
            try {
                MySqlDataAdapter veriDoldur = new MySqlDataAdapter("GET_CONNECT", connection);
                connection.Open();
                DataSet veriEkle = new DataSet();
                veriDoldur.Fill(veriEkle, "phones");
                dgvListe.DataSource = veriEkle.Tables["phones"];
                connection.Close();
            }
            catch
            {
                MessageBox.Show("Bağlantı kurulamadı", "Hata");
                this.Close();
                
            }
            
        }
        //Kişi kaydetme Buttonu.
        private void btnSave_Click(object sender, EventArgs e)
        {
            //Textbox değerleri string olarak alınır.
            string strKisi = txtAd.Text;
            string strSoyisim = txtSoyad.Text;
            string strTelefon = txtNo.Text;
            string strTelefon2 = txtNo2.Text;
            string strEposta = txtEposta.Text;
            string strAdres = txtAdres.Text;
            //Textboxların dolu olup olmadığı kontrol edilir.
            if (strKisi == "" || strSoyisim == "" || strTelefon == "" || strEposta == "" || strAdres == "")
            {
                MessageBox.Show("Lütfen gerekli olan alanları doldurunuz.", "Hata");
            }
            else
            {
                //Kaydet komutu oluşturulur.
                MySqlCommand komut = new MySqlCommand("GET_PHONES", connection);
                connection.Open();
                //Parametreler eklenir.
                komut.Parameters.Add(new MySqlParameter("dbisim", strKisi));
                komut.Parameters.Add(new MySqlParameter("dbsoyisim", strSoyisim));
                komut.Parameters.Add(new MySqlParameter("dbtelefon", strTelefon));
                komut.Parameters.Add(new MySqlParameter("dbtelefon2", strTelefon2));
                komut.Parameters.Add(new MySqlParameter("dbadres", strAdres));
                komut.Parameters.Add(new MySqlParameter("dbeposta", strEposta));
                //Komut stored procedure olduğundan komut tipi ayarlanır.
                komut.CommandType = CommandType.StoredProcedure;
                //Komut çalıştırılır.
                komut.ExecuteNonQuery();
                connection.Close();
                Yenile();
                Temizle();
            }            
        }
        
        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Silme işlemi gerçekleşmesi için kullanıcıdan onay alınır.
            DialogResult silOnay = new DialogResult();
            silOnay = MessageBox.Show("Silmek istediğinize emin misiniz?","Silme İşlemi", MessageBoxButtons.YesNo);

            if (silOnay == DialogResult.Yes)
            {
                //Seçilen satırın id değeri idDeger değişkenine atanır.
                int idDeger = Convert.ToInt32(dgvListe.CurrentRow.Cells["id"].Value);
                connection.Open();
                //Silme işlemi komut değişkenine atanır.
                MySqlCommand komut = new MySqlCommand("DELETE_ID_ROW", connection);
                //idDeger parametre olarak komuta eklenir.
                komut.Parameters.Add(new MySqlParameter("USER_ID", idDeger.ToString()));
                //Komutun tipi stored procedure olarak ayarlanır.
                komut.CommandType = CommandType.StoredProcedure;
                komut.ExecuteNonQuery();
                //Silme işleminden sonra id sırası karışacağından dolayı id sutünu kaldırılır.
                komut.CommandText = "DROP_ID";
                komut.ExecuteNonQuery();
                //Kaldırılan id sutünu yeniden eklenir.
                komut.CommandText = "ADD_ID";
                komut.ExecuteNonQuery();
                connection.Close();
                Yenile();
            }

        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Textboxdaki değerler string değişkenlere atanır.
            string strKisi = txtAd.Text;
            string strSoyad = txtSoyad.Text;
            string strTelefon = txtNo.Text;
            string strTelefon2 = txtNo2.Text;
            string strEposta = txtEposta.Text;
            string strAdres = txtAdres.Text;
            //Textboxların dolu olup olmadığı kontrol edilir.
            if (strKisi == "" || strSoyad == "" || strTelefon == "" || strEposta == "" || strAdres == "")
            {
                MessageBox.Show("Lütfen gerekli olan alanları doldurunuz.", "Hata");
            }
            else
            {
                try
                {
                    int idDeger = Convert.ToInt32(dgvListe.CurrentRow.Cells["id"].Value);
                    MySqlCommand komut = new MySqlCommand("UPDATE_ROW", connection);
                    komut.Parameters.Add(new MySqlParameter("dbisim", strKisi));
                    komut.Parameters.Add(new MySqlParameter("dbsoyisim", strSoyad));
                    komut.Parameters.Add(new MySqlParameter("dbtelefon", strTelefon));
                    komut.Parameters.Add(new MySqlParameter("dbtelefon2", strTelefon2));
                    komut.Parameters.Add(new MySqlParameter("dbeposta", strEposta));
                    komut.Parameters.Add(new MySqlParameter("dbadres",strAdres));
                    komut.Parameters.Add(new MySqlParameter("idDeger", idDeger.ToString()));
                    komut.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    komut.ExecuteNonQuery();
                    connection.Close();
                    Yenile();
                    Temizle();
                }
                catch (Exception)
                {
                    MessageBox.Show("Lütfen bir satır seçiniz", "Hata");
                }
            }
        }

       

        private void Temizle()
        {
            txtAd.Clear();
            txtEposta.Clear();
            txtAdres.Clear();
            txtNo.Clear();
            txtNo2.Clear();
            txtSoyad.Clear();
        }

        private void dgvListe_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                txtAd.Text = dgvListe.CurrentRow.Cells["Isim"].Value.ToString();
                txtSoyad.Text = dgvListe.CurrentRow.Cells["Soyisim"].Value.ToString();
                txtNo.Text = dgvListe.CurrentRow.Cells["Telefon"].Value.ToString();
                txtNo2.Text = dgvListe.CurrentRow.Cells["Telefon2"].Value.ToString();
                txtEposta.Text = dgvListe.CurrentRow.Cells["Eposta"].Value.ToString();
                txtAdres.Text = dgvListe.CurrentRow.Cells["Adres"].Value.ToString();
            }
            catch
            {
                MessageBox.Show("Veri yok lütfen veri ekleyiniz.", "Hata");
            }
        }

        private void kisiNo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber((char)e.KeyChar) || e.KeyChar == (char)8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


        private void kisiAdi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter((char)e.KeyChar) || e.KeyChar == (char)8 || e.KeyChar == ' ')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void isimArama_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter((char)e.KeyChar) || e.KeyChar == (char)8 || e.KeyChar == ' ')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void isimArama_TextChanged(object sender, EventArgs e)
        {
            string strArama = txtArama.Text;
            connection.Open();
            DataSet veriEkle = new DataSet();
            MySqlDataAdapter veriDoldur = new MySqlDataAdapter("SELECT * FROM phones WHERE isim LIKE '%" + strArama + "%'", connection);
            veriDoldur.Fill(veriEkle, "phones");
            connection.Close();
            dgvListe.DataSource = veriEkle.Tables["phones"];


            if (dgvListe.Rows.Count == 0)
            {
                connection.Open();
                MySqlDataAdapter veriDoldurSoyisim = new MySqlDataAdapter("SELECT * FROM phones WHERE Soyisim LIKE '%" + strArama + "%'", connection);
                veriDoldurSoyisim.Fill(veriEkle, "phones");
                connection.Close();
                dgvListe.DataSource = veriEkle.Tables["phones"];
            }
        }

        private void TxtNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber((char)e.KeyChar) || e.KeyChar == (char)8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TxtSoyad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter((char)e.KeyChar) || e.KeyChar == (char)8 || e.KeyChar == ' ')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
