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

namespace Stock
{
    public partial class Products : Form
    {
        SqlConnection sqlConnection;
        //String baglanti = @"Data Source=DESKTOP-2TS36OR\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True";
        public Products()
        {
            InitializeComponent();
            //Ya da Products_Load da tanımlanırsa aynı sonucu verir.
            LoadData(); 
        }

        //Form açılınca gözükecek olaylar yapılır.
        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;     //Form acılınca 0.index gözükmesini sağlar.
        }

        //Kaydet & Güncelle butonu
        private void button2_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                sqlConnection = Connection.GetConnection();
                sqlConnection.Open();

                Boolean status = false;
                if (comboBox1.SelectedIndex == 0)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                var sqlQuery = "";
                if (IfProductsExists(sqlConnection, textBox1.Text))
                {
                    sqlQuery = "Update Stock.dbo.Products Set ProductsName='" + textBox2.Text + "',ProductsStatus='" + status + "' Where ProductsCode='" + textBox1.Text + "'";
                }
                else
                {
                    sqlQuery = "Insert Into Stock.dbo.Products Values('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";
                }

                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();

                LoadData();
                KayıtTemizleme();
            }
        }

        // Eğer Id değeri o tabloda varsa sorgusu
        private bool IfProductsExists(SqlConnection sqlConnection, string productsCode)
        {
            SqlDataAdapter sql = new SqlDataAdapter("Select ProductsCode From Stock.dbo.Products Where ProductsCode='"+productsCode+"'", sqlConnection);
            DataTable dt = new DataTable();
            sql.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }


        // Veri tabanından verilerin getirildiği fonksiyon.
        public void LoadData()
        {
            sqlConnection = Connection.GetConnection();
            SqlDataAdapter sql = new SqlDataAdapter("Select * From Stock.dbo.Products ", sqlConnection);
            DataTable dt = new DataTable();
            sql.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductsCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductsName"].ToString();
                if ((bool)item["ProductsStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Aktif";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Pasif";
                }

            }
        }

        //DataGridView ('den veri çekme) kayıtlardan herhangi birini çift tıklayınca verileri textbox,combobox'a getirme. 
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button2.Text = "Güncelle";
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString()=="Aktif")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1 ;
            }
            
        }
        
        //Silme Butonu
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Silmek istediğine emin misin?", "Mesaj", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (Validation())
                {
                    sqlConnection = Connection.GetConnection();
                    sqlConnection.Open();

                    var sqlQuery = "";
                    if (IfProductsExists(sqlConnection, textBox1.Text))
                    {
                        sqlQuery = "Delete From Stock.dbo.Products  Where ProductsCode='" + textBox1.Text + "'";
                        SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                        sqlCommand.ExecuteNonQuery();

                        sqlConnection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Kayıt bulunamadı.");
                    }
                    LoadData();
                    KayıtTemizleme();
                }
            }
        }

        // TextBox ve Combobox daki verili silme.
        private void KayıtTemizleme()
        {
            //Güncellemeden sonra tb ve cb temizleme sonra ekle butonu getirme.
            textBox1.Clear();
            textBox2.Clear();
            comboBox1.SelectedIndex = -1;
            button2.Text = "Ekle";
            textBox1.Focus();
        }

        // Temizle Butonu
        private void button3_Click(object sender, EventArgs e)
        {
            KayıtTemizleme();
        }


        // Doğrulama boş girilmemesi için kullanılır.
        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                //errorProvider toolbox dan çekilmiş hata işareti çıkartır.
                errorProvider1.Clear(); 
                errorProvider1.SetError(textBox1,"Gerekli Products Code");
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox2, "Gerekli Products Name");
            }
            else if(comboBox1.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(comboBox1, "Gerekli Products Status");
            }
            else
            {
                result = true;
            }
            ////Boş girildiğinde ekleme veya silme olmasın
            //bool result = false;
            //if(!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text) && comboBox1.SelectedIndex > -1)
            //{
            //    result = true;
            //}
            return result;
        }
    }
}
