using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace stok_takip
{
    public partial class urunislem : Form
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=stok_takip_db;Uid=root;Pwd='';");
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt;
        int seciliUrunID = 0;
        public urunislem()
        {
            InitializeComponent();
        }

        void VeriGetir()
        {
            dt = new DataTable();
            conn.Open();
            adapter = new MySqlDataAdapter("SELECT *FROM urunler", conn);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }


        private void urunislem_Load(object sender, EventArgs e)
        {

            VeriGetir();
            kategoricmb();
            tedarikcmb();
        }

        private bool BilgiGirisKontrol()
        {
            decimal kont;
            bool kontrol = false;
            if (string.IsNullOrEmpty(txturunadi.Text))
            {
                MessageBox.Show("Ürün Adını Giriniz");
            }
            else if (!Decimal.TryParse(txtfiyat.Text, out kont))
            {
                MessageBox.Show("Ürün Fiyatını Giriniz");
            }
            else
            {
                kontrol = true;
            }
            return kontrol;
        }
        private void kategoricmb()
        {
            MySqlCommand komut = new MySqlCommand();
            komut.CommandText = "SELECT *FROM kategoriler";
            komut.Connection = conn;
            komut.CommandType = CommandType.Text;

            MySqlDataReader dr;
            conn.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                string kategori = $"{dr["kategori_id"]} - {dr["kategori_adi"]}";
                combokategori.Items.Add(kategori);
            }
            conn.Close();
        }
        private void tedarikcmb()
        {
            MySqlCommand komut = new MySqlCommand();
            komut.CommandText = "SELECT *FROM tedarikciler";
            komut.Connection = conn;
            komut.CommandType = CommandType.Text;

            MySqlDataReader dr;
            conn.Open();
            dr = komut.ExecuteReader();
            while (dr.Read())
            {
                string kategori = $"{dr["id"]} - {dr["firma_adi"]}";
                combotedarik.Items.Add(kategori);
            }
            conn.Close();
        }

        private void eklebtn_Click(object sender, EventArgs e)
        {
            string selectedCategory = combokategori.SelectedItem.ToString();
            string categoryId = selectedCategory.Split('-')[0].Trim();

            string selectedtedarik = combokategori.SelectedItem.ToString();
            string tedarikId = selectedCategory.Split('-')[0].Trim();
            try
            {
                if (seciliUrunID == 0)
                {
                    if (BilgiGirisKontrol())
                    {
                        string sorgu = "Insert into urunler (urun_adi,kategori_id,tedarikci_id,stok_adedi,fiyat,aciklama) values (@txturunadi,@kategori_id,@tedarikci_id,@stok_adedi,@fiyat,@aciklama)";
                        cmd = new MySqlCommand(sorgu, conn);
                        cmd.Parameters.AddWithValue("@txturunadi", txturunadi.Text);
                        cmd.Parameters.AddWithValue("@kategori_id", categoryId);
                        cmd.Parameters.AddWithValue("@tedarikci_id", tedarikId);
                        cmd.Parameters.AddWithValue("@stok_adedi", numstok.Text);
                        cmd.Parameters.AddWithValue("@fiyat", txtfiyat.Text);
                        cmd.Parameters.AddWithValue("@aciklama", txtaciklama.Text);
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
            txturunadi.Clear();
            txtaciklama.Clear();
            txtfiyat.Clear();
            combokategori.Items.Clear();
            combotedarik.Items.Clear();
            dataGridView1.ClearSelection();
            seciliUrunID = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    seciliUrunID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["urun_kodu"].Value.ToString());
                    txturunadi.Text = dataGridView1.CurrentRow.Cells["urun_adi"].Value.ToString();
                    txtaciklama.Text = dataGridView1.CurrentRow.Cells["kategori_id"].Value.ToString();
                    txtfiyat.Text = dataGridView1.CurrentRow.Cells["tedarikci_id"].Value.ToString();
                    numstok.Text = dataGridView1.CurrentRow.Cells["stok_adedi"].Value.ToString();
                    combokategori.Text = dataGridView1.CurrentRow.Cells["fiyat"].Value.ToString();
                    combotedarik.Text = dataGridView1.CurrentRow.Cells["aciklama"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void silbtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (seciliUrunID != 0)
                {
                    string sql = "Delete From urunler Where id=" + seciliUrunID;
                    cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    VeriGetir();
                    MessageBox.Show("Kayıt silindi.");
                    formutemizle();
                }
                else
                {
                    MessageBox.Show("Seçilecek Ürünü Seçiniz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "silme İşleminde Hata");
            }
        }

        private void guncellebtn_Click(object sender, EventArgs e)
        {
            string selectedCategory = combokategori.SelectedItem.ToString();
            string categoryId = selectedCategory.Split('-')[0].Trim();

            string selectedtedarik = combokategori.SelectedItem.ToString();
            string tedarikId = selectedCategory.Split('-')[0].Trim();
            try
            {
                if (seciliUrunID != 0)
                {
                    if (BilgiGirisKontrol())
                    {
                        string sql = "UPDATE urunler SET urun_adi=@urun_adi, kategori_id=@kategori_id, tedarikci_id=@tedarikci_id, stok_adedi=@stok_adedi, fiyat=@fiyat, aciklama=@aciklama WHERE id=" + seciliUrunID;
                        cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@urun_adi", txturunadi.Text);
                        cmd.Parameters.AddWithValue("@kategori_id", categoryId);
                        cmd.Parameters.AddWithValue("@tedarikId", tedarikId);
                        cmd.Parameters.AddWithValue("@stok_adedi", numstok.Text);
                        cmd.Parameters.AddWithValue("@fiyat", txtfiyat.Text);
                        cmd.Parameters.AddWithValue("@aciklama", txtaciklama.Text);
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

        private void button1_Click(object sender, EventArgs e)
        {
            frmkategori frmkategori = new frmkategori();
            DialogResult result = frmkategori.ShowDialog();
            if(result == DialogResult.Cancel) {
                VeriGetir();
            }
        }
    }
    }

