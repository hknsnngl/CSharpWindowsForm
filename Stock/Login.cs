using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Stock
{
    public partial class Login : Form
    {
        SqlConnection con;
        //String baglanti = "Data Source=DESKTOP-2TS36OR\\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True"; //?\\onemli!!
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";     //textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            con = Connection.GetConnection();  
            SqlDataAdapter sql = new SqlDataAdapter("Select * From Stock.dbo.Login Where Username = '"+textBox1.Text+"' and Password ='"+textBox2.Text+"'", con);
            DataTable dt = new DataTable();
            sql.Fill(dt);
            if(dt.Rows.Count==1)
            {
                this.Hide();
                StockMain stockMain = new StockMain();
                stockMain.Show();
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı giris ya da şifre","Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1_Click(sender, e);
            }
          
        }
    }
}
