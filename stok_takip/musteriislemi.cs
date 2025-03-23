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
    public partial class musteriislemi : Form
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=stok_takip_db;Uid=root;Pwd='';");
        MySqlCommand cmd;
        MySqlDataAdapter adapter;
        DataTable dt;
        int secilimusteriİD = 0;
        public musteriislemi()
        {
            InitializeComponent();
        }

        void VeriGetir()
        {
            dt = new DataTable();
            conn.Open();
            adapter = new MySqlDataAdapter("SELECT *FROM musteriler", conn);
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void musteriislemi_Load(object sender, EventArgs e)
        {
            VeriGetir();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow != null)
                {
                    secilimusteriİD = Convert.ToInt32(dataGridView1.CurrentRow.Cells["id"].Value.ToString());
                    txtfirmaisim.Text = dataGridView1.CurrentRow.Cells["firma_adi"].Value.ToString();
                    txtad.Text = dataGridView1.CurrentRow.Cells["yetkili_adsoyad"].Value.ToString();
                    txtmail.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
                    txtadres.Text = dataGridView1.CurrentRow.Cells["mail"].Value.ToString();
                    txttelefon.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata Oluştu ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool Bilgigiris()
        {
            bool kontrol = false;
            if(txtad.TextLength <= 2)
            {
                MessageBox.Show("Lütfen Müşteri Adını Soyadını giriniz");
            }
            else if (!txttelefon.MaximumSize.IsEmpty)
            {
                MessageBox.Show("Telefon numarasını 10 Haneli Olarak giriniz");
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
                if (secilimusteriİD == 0)
                {
                    if (Bilgigiris())
                    {
                        string sorgu = "Insert into tedarikciler (firma_adi,yetkili_adsoyad,telefon,mail,adres) values (@kategori_adi,@yetkili,@telefon,@mail,@adres)";
                        cmd = new MySqlCommand(sorgu, conn);

                        cmd.Parameters.AddWithValue("@telefon", txttelefon.Text);

                        cmd.Parameters.AddWithValue("@mail", txtmail.Text);
                        cmd.Parameters.AddWithValue("@adres", txtadres.Text);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        VeriGetir();
                        MessageBox.Show("Kayıt Eklendi.");

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

        private void silbtn_Click(object sender, EventArgs e)
        {

        }
    }
}
