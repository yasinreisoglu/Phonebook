using MySql.Data.MySqlClient;
using System.Collections.Generic;
using WebApi.Model;

namespace WebApi.Connector
{
    public class Conn
    {
        //connection stringi oluşturulur.
        private string connection;
        public Conn()
        {
            //connection stringine veritabanına bağlantı için gereken bilgiler atanır.
            connection = "server=localhost;userid=root;password=;database=deneme";
        }
        //GET işlemi 
        public List<User> getList()
        {
            //getNumaralar adından yeni bir liste oluşturulur.
            List<User> getNumaralar = new List<User>();
            MySqlConnection connMysql = new MySqlConnection(connection);
            MySqlCommand komut = connMysql.CreateCommand();
            //komut değişkenine veritabanı komutu atanır.
            komut.CommandText = "GET_CONNECT";
            //komut tipi belirlenir.
            komut.CommandType = System.Data.CommandType.StoredProcedure;
            komut.Connection = connMysql;
            connMysql.Open();
            MySqlDataReader veriOku = komut.ExecuteReader();
            while (veriOku.Read())
            {
                getNumaralar.Add(new User
                    {
                    Isim = veriOku.GetString(veriOku.GetOrdinal("Isim")),
                    Soyisim = veriOku.GetString(veriOku.GetOrdinal("Soyisim")),
                    Telefon = veriOku.GetString(veriOku.GetOrdinal("Telefon")),
                    Telefon2 = veriOku.GetString(veriOku.GetOrdinal("Telefon2")),
                    Adres = veriOku.GetString(veriOku.GetOrdinal("Adres")),
                    Eposta = veriOku.GetString(veriOku.GetOrdinal("Eposta")),
                    Id = veriOku.GetInt32(veriOku.GetOrdinal("id"))
                    });
            }
                    
            connMysql.Close();
            return getNumaralar;
        }
        //Id Get
        public List<User> getById(int i)
        {
            List<User> idList = new List<User>();
            MySqlConnection connMysql = new MySqlConnection(connection);

            MySqlCommand cmd = connMysql.CreateCommand();
            cmd.CommandText = "SELECT * FROM `phones` WHERE id = '"+i+"' ";
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = connMysql;
            connMysql.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                idList.Add(new User
                {
                Isim = reader.GetString(reader.GetOrdinal("Isim")),
                Soyisim = reader.GetString(reader.GetOrdinal("Soyisim")),
                Telefon = reader.GetString(reader.GetOrdinal("Telefon")),
                Telefon2 = reader.GetString(reader.GetOrdinal("Telefon2")),
                Adres = reader.GetString(reader.GetOrdinal("Adres")),
                Eposta = reader.GetString(reader.GetOrdinal("Eposta")),
                Id = reader.GetInt32(reader.GetOrdinal("id"))
                });
            }
            connMysql.Close();
            return idList;
        }
        //DELETE işlemi
        public void  deleteById(int id)
        {
            //Yeni komut oluşturulur.
            MySqlConnection connMysql = new MySqlConnection(connection);
            MySqlCommand komut = connMysql.CreateCommand();    

            //Id ile silme işlemi yapılır.
            komut.CommandText = "DELETE_ID_ROW";
            komut.Parameters.Add(new MySqlParameter("USER_ID", id));
            komut.CommandType = System.Data.CommandType.StoredProcedure;

            //Silme işlemi yapıldıktan sonra id sirasi düzenli olmasi için id sutunu drop edilir
            MySqlCommand komutIdDrop = new MySqlCommand();
            komutIdDrop.CommandText = "DROP_ID";
            komutIdDrop.CommandType = System.Data.CommandType.StoredProcedure;

            //id sutunu drop edildikten sonra yeni bir id sutunu eklenir
            MySqlCommand komutIdAdd = new MySqlCommand();
            komutIdAdd.CommandText = "ADD_ID";
            komutIdAdd.CommandType = System.Data.CommandType.StoredProcedure;
            komut.Connection = connMysql;
            komutIdAdd.Connection = connMysql;
            komutIdDrop.Connection = connMysql;
            connMysql.Open();
            komut.ExecuteNonQuery();
            komutIdDrop.ExecuteNonQuery();
            komutIdAdd.ExecuteNonQuery();
            connMysql.Close();
            
        }
        //POST işlemi
        public void  postList (string Isim, string Soyisim, string Telefon, string Telefon2, string Adres, string Eposta)
        {
            //Yeni komut oluşturulur.
            MySqlConnection connMysql = new MySqlConnection(connection);
            MySqlCommand komut = new MySqlCommand();
            //Komuta veritabanı komutu, postList fonksiyonu ile gelen parametrelerle eklenir.
            komut.CommandText = "GET_PHONES";
            komut.CommandType = System.Data.CommandType.StoredProcedure;
            komut.Parameters.Add(new MySqlParameter("dbisim", Isim));
            komut.Parameters.Add(new MySqlParameter("dbsoyisim", Soyisim));
            komut.Parameters.Add(new MySqlParameter("dbtelefon", Telefon));
            komut.Parameters.Add(new MySqlParameter("dbtelefon2", Telefon2));
            komut.Parameters.Add(new MySqlParameter("dbadres", Adres));
            komut.Parameters.Add(new MySqlParameter("dbeposta", Eposta));
            komut.Connection = connMysql;
            connMysql.Open();
            komut.ExecuteNonQuery();
            connMysql.Close();
        }
        //PUT işlemi
        public void putList(int id, string Isim, string Soyisim, string Telefon, string Telefon2, string Adres, string Eposta)
        {
            //Yeni komut oluşturulur.
            MySqlConnection connMysql = new MySqlConnection(connection);
            MySqlCommand komut = new MySqlCommand();
            //Komuta veritabanı komutu, putlist fonksiyonu ile birlikte gelen parametrelerle eklenir
            komut.CommandText = "UPDATE_ROW";
            komut.Parameters.Add(new MySqlParameter("dbisim", Isim));
            komut.Parameters.Add(new MySqlParameter("dbsoyisim", Soyisim));
            komut.Parameters.Add(new MySqlParameter("dbtelefon", Telefon));
            komut.Parameters.Add(new MySqlParameter("dbtelefon2", Telefon2));
            komut.Parameters.Add(new MySqlParameter("dbeposta", Eposta));
            komut.Parameters.Add(new MySqlParameter("dbadres", Adres));
            komut.Parameters.Add(new MySqlParameter("idDeger", id.ToString()));
            komut.CommandType = System.Data.CommandType.StoredProcedure;
            komut.Connection = connMysql;
            connMysql.Open();
            komut.ExecuteNonQuery();
            connMysql.Close();
        }
    }
}

