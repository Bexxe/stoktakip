using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace stok_takip
{
    public partial class frmkategori : Form
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=stok_takip_db;Uid=root;Pwd='';");
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt;
        int secilliKategoriİd = 0;
        public frmkategori()
        {
            InitializeComponent();
        }

        void VeriGetir()
        {
            dt = new DataTable();
            conn.Open();
            adapter = new MySqlDataAdapter("SELECT *FROM kategoriler", conn);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VeriGetir();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void eklebtn_Click(object sender, EventArgs e)
        {
           try
            {
                if(secilliKategoriİd == 0)
                {
                    if (!string.IsNullOrEmpty(txtkategoriadi.Text))
                    {
                        string sorgu = "Insert into kategoriler (kategori_adi) values (@kategori_adi)";
                        cmd = new MySqlCommand(sorgu, conn);
                        cmd.Parameters.AddWithValue("@kategori_adi", txtkategoriadi.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        VeriGetir();
                        MessageBox.Show("Kayıt Eklendi.");
                        txtkategoriadi.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Kategori adını giriniz. ");
                    }
                }
                else
                {
                    MessageBox.Show("Yeni Kayıt Eklemek İçin 'Yeni Kayıt' Buttonnuna tıklayınız");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Kayıt Hatası");
            }
        }

        private void kayitbtn_Click(object sender, EventArgs e)
        {
            secilliKategoriİd = 0;
            txtkategoriadi.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(dataGridView1.CurrentRow != null)
                {
                    secilliKategoriİd = Convert.ToInt32(dataGridView1.CurrentRow.Cells["kategori_id"].Value.ToString());
                    txtkategoriadi.Text = dataGridView1.CurrentRow.Cells["kategori_adi"].Value.ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void silbtn_Click(object sender, EventArgs e)
        {
            int kategori_id;
            try
            {
                if(secilliKategoriİd != 0)
                {
                    conn.Open();
                    MySqlCommand komut = new MySqlCommand("select *from urunler",conn); 
                    MySqlDataReader read = komut.ExecuteReader();
                    while (read.Read())
                    {
                        kategori_id = int.Parse(read["kategori_id"].ToString());

                        if (kategori_id == secilliKategoriİd)
                        {
                            MessageBox.Show("Bu Kayıt Silinemez. Silmek İstediğiniz Kategoriye Ait Ürünler Mevcut");
                        }
                        else
                        {
                            string sql = "Delete From kategoriler Where kategori_id="+secilliKategoriİd;
                            cmd = new MySqlCommand(sql, conn);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            VeriGetir();
                            MessageBox.Show("Kayıt silindi.");
                            VeriGetir();
                            txtkategoriadi.Clear();
                        }

                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Silinecek Kaydı Seçin");
                }
            }
            catch(Exception ex) {
                MessageBox.Show(ex.Message, "silme işleminde hata");
            }
        }

        private void guncellebtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (secilliKategoriİd != 0)
                {
                    if (!string.IsNullOrEmpty(txtkategoriadi.Text))
                    {
                        string sql = "UPDATE kategoriler SET kategori_adi=@kategori_adi WHERE kategori_id=" + secilliKategoriİd;
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@kategori_adi", txtkategoriadi.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        VeriGetir();
                        MessageBox.Show("Kayıt güncellendi.");
                        txtkategoriadi.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Kategori adını giriniz. ");
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
    }
}
