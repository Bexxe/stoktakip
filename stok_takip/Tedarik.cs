using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace stok_takip
{
    public partial class Tedarik : Form
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=stok_takip_db;Uid=root;Pwd='';");
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt;
        int seciliTedarikciID = 0;
        int tedarikci_id;
        public Tedarik()
        {
            InitializeComponent();
        }
        void VeriGetir()
        {
            dt = new DataTable();
            conn.Open();
            adapter = new MySqlDataAdapter("SELECT *FROM tedarikciler", conn);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void Tedarik_Load(object sender, EventArgs e)
        {
            VeriGetir();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    seciliTedarikciID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value.ToString());
                    txtfirmadi.Text = dataGridView1.CurrentRow.Cells["firma_adi"].Value.ToString();
                    txtyetkili.Text = dataGridView1.CurrentRow.Cells["yetkili_adsoyad"].Value.ToString();
                    txttelefon.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
                    txtmail.Text = dataGridView1.CurrentRow.Cells["mail"].Value.ToString();
                    txtadres.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
                }
                MessageBox.Show(seciliTedarikciID.ToString());
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Hata Oluştu ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool BilgiGirisKontrol()
        {
            bool kontrol = false;
            if(txtfirmadi.TextLength < 2)
            {
                MessageBox.Show("Firma adını giriniz.");
            }
            else if (txtyetkili.TextLength < 2)
            {
                MessageBox.Show("Yetkili adını giriniz.");
            }
            else if(!txttelefon.MaximumSize.IsEmpty)
            {
                MessageBox.Show("Telefon numarasını 10 Haneli Olarak giriniz");
            }
            else if (txtmail.TextLength < 5)
            {
                MessageBox.Show("Mail Adresinizi Giriniz");
            }
            else if (txtadres.TextLength <= 2)
            {
                MessageBox.Show("Firma Adresini Giriniz");
            }
            else
            {
                kontrol = true;
            }
            return kontrol;
        }

        private void eklebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliTedarikciID==0)
                {
                    if (BilgiGirisKontrol())
                    {
                        string sorgu = "Insert into tedarikciler (firma_adi,yetkili_adsoyad,telefon,mail,adres) values (@kategori_adi,@yetkili,@telefon,@mail,@adres)";
                        cmd = new MySqlCommand(sorgu, conn);
                        cmd.Parameters.AddWithValue("@kategori_adi", txtfirmadi.Text);
                        cmd.Parameters.AddWithValue("@telefon", txttelefon.Text);
                        cmd.Parameters.AddWithValue("@yetkili", txtyetkili.Text);
                        cmd.Parameters.AddWithValue("@mail", txtmail.Text);
                        cmd.Parameters.AddWithValue("@adres", txtadres.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        VeriGetir();
                        MessageBox.Show("Kayıt Eklendi.");
                        formutemizle();
                    }
                }
                else
                {
                    MessageBox.Show("Yeni Tedarikçi Eklemek İçin 'Yeni Kayıt' Buttonuna Tıklayınız");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "kayıt hatası");
            }
        }
        private void formutemizle()
        {
            txtfirmadi.Clear();
            txtyetkili.Clear();
            txtmail.Clear();
            txtadres.Clear();
            txttelefon.Clear();
            dataGridView1.ClearSelection();
            seciliTedarikciID = 0;
        }

        private void silbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliTedarikciID != 0)
                {
                    conn.Open();

                    // Check if there are related records in the 'urunler' table
                    MySqlCommand checkCommand = new MySqlCommand("SELECT * FROM urunler WHERE tedarikci_id = @tedarikciID", conn);
                    checkCommand.Parameters.AddWithValue("@tedarikciID", seciliTedarikciID);

                    using (MySqlDataReader reader = checkCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MessageBox.Show("Bu Kayıt Silinemez. Silmek İstediğiniz Kategoriye Ait Ürünler Mevcut");
                        }
                        else
                        {
                            // No related records found, proceed with deletion
                            string deleteSql = "DELETE FROM tedarikciler WHERE id = @tedarikciID";
                            using (MySqlCommand deleteCommand = new MySqlCommand(deleteSql, conn))
                            {
                                deleteCommand.Parameters.AddWithValue("@tedarikciID", seciliTedarikciID);
                                deleteCommand.ExecuteNonQuery();
                            }

                            VeriGetir();
                            MessageBox.Show("Kayıt silindi.");
                            formutemizle();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Silinecek Kaydı Seçin");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "silme işleminde hata");
            }
            finally
            {
                conn.Close();
            }
        }

        private void guncellebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliTedarikciID != 0)
                {
                    if (BilgiGirisKontrol())
                    {
                        string sql = "UPDATE tedarikciler SET firma_adi=@firma_adi, yetkili_adsoyad=@yetkili_adsoyad, telefon=@telefon, mail=@mail, adres=@adres WHERE id=" + seciliTedarikciID;
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@firma_adi", txtfirmadi.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        VeriGetir();
                        MessageBox.Show("Kayıt güncellendi.");
                        formutemizle();
                    }
                }
                else
                {
                    MessageBox.Show("Güncellenecek Kaydı Seçiniz");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Güncelleme Hatası");
            }
        }

        private void kayitbtn_Click(object sender, EventArgs e)
        {
            formutemizle();
        }
    }
}
