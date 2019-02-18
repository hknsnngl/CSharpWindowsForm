using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Stock
{
    public partial class Stock : Form
    {
        SqlConnection sqlConnection;
        public Stock()
        {
            InitializeComponent();
            //LoadData();
        }

        // Form açılınla olan olaylar.
        private void Stock_Load(object sender, EventArgs e)
        {
            this.ActiveControl = dateTimePicker1;
            comboBox1.SelectedIndex = 0;
            LoadData();
            Search();
        }

        //Enter la toolbox arası geçiş KeyDownlar(event)
        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
               textBox1.Focus(); 
               
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(dgView.Rows.Count > 0)
                {
                    textBox1.Text = dgView.SelectedRows[0].Cells[0].Value.ToString();
                    textBox2.Text = dgView.SelectedRows[0].Cells[1].Value.ToString();
                    this.dgView.Visible = false;
                    textBox3.Focus();
                }
                else
                {
                    this.dgView.Visible = false;
                }
                // if (textBox1.Text.Length > 0)
                //{

                //    /* 2 textBox1.Text = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();
                //    textBox2.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();*/
                //    textBox3.Focus();//2

                //    /*1/Code yazınca pname direk gelmesi textboxa                    
                //    sqlConnection = Connection.GetConnection();
                //    sqlConnection.Open();
                //    SqlDataAdapter sql = new SqlDataAdapter("Select ProductsName From Stock.dbo.Stock Where ProductsCode='" + textBox1.Text + "'", sqlConnection);
                //    DataTable dt = new DataTable();
                //    sql.Fill(dt);
                //    if (dt.Rows.Count > 0)
                //    {
                //        textBox2.Text = dt.Rows[0][0].ToString();
                //        textBox3.Focus();
                //    }
                //    else
                //    {
                //        textBox2.Text = "";
                //    }*/
                //}               
                ////1else
                ////{
                ////    textBox1.Focus();
                ////}

            }
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox2.Text.Length > 0)
                {
                    textBox3.Focus();
                }
                else
                {
                    textBox2.Focus();
                }
            }
        }
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textBox3.Text.Length > 0)
                {
                    comboBox1.Focus();
                }
                else
                {
                    textBox3.Focus();
                }
            }
        }
        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (comboBox1.SelectedIndex != -1)
                {
                    button1.Focus();
                }
                else
                {
                    comboBox1.Focus();
                }
            }
        }

        //textboxda  çift tıklama ile (Event) veri çekme
        bool change = true;
        private void proCode_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (change)
            {
                change = false;
                textBox1.Text = dgView.SelectedRows[0].Cells[0].Value.ToString();
                textBox2.Text = dgView.SelectedRows[0].Cells[1].Value.ToString();
                this.dgView.Visible = false;
                textBox3.Focus();
                change = true;
            }
        }
  
        //KeyPress (event) Textboxa hangi değerlerin girileceği işlemi?
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) & (Keys)e.KeyChar != Keys.Back & e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        //Temizleme Fonk.
        private void KayıtTemizleme()
        {
            dateTimePicker1.Value = DateTime.Now;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            button1.Text = "Ekle";
            dateTimePicker1.Focus();
        }
        //Temizleme Butonu
        private void button3_Click(object sender, EventArgs e)
        {
            KayıtTemizleme();
        }

        // Doğrulama işlemleri boş girilmişmi gibi.
        private bool Validation()
        {
            bool result = false;
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox1, "Gerekli Products Code");
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox2, "Gerekli Products Name");
            }
            else if (string.IsNullOrEmpty(textBox3.Text))
            {
                errorProvider1.Clear();
                errorProvider1.SetError(textBox2, "Gerekli Products Quantity");
            }
            else if (comboBox1.SelectedIndex == -1)
            {
                errorProvider1.Clear();
                errorProvider1.SetError(comboBox1, "Gerekli Products Status");
            }
            else
            {
                errorProvider1.Clear();
                result = true;
            }
            return result;
        }

        //Id var mı?
        private bool IfProductsExists(SqlConnection sqlConnection, string productsCode)
        {
            SqlDataAdapter sql = new SqlDataAdapter("Select ProductsCode From Stock.dbo.Stock Where ProductsCode='" + productsCode + "'", sqlConnection);
            DataTable dt = new DataTable();
            sql.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        //Ekleme & Güncellme
        private void button1_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                sqlConnection = Connection.GetConnection();
                sqlConnection.Open();

                bool status = false;
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
                    sqlQuery = "Update Stock.dbo.Stock Set ProductsName='" + textBox2.Text + "',Quantity='" + textBox3.Text + "',ProductsStatus='"+status+"' Where ProductsCode='" + textBox1.Text + "'";
                }
                else
                {//MM/dd/yyyy database gün ay kaydeder.
                    sqlQuery = "Insert Into Stock.dbo.Stock Values('" + textBox1.Text + "','" + textBox2.Text + "','" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "','"+textBox3.Text+"','" + status + "')";
                }

                SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();
                MessageBox.Show("İşlem Başarılı");
                LoadData();
                KayıtTemizleme();
            }
        }


        //VT'dan verileri getirme
        public void LoadData()
        {
            sqlConnection = Connection.GetConnection();
            SqlDataAdapter sql = new SqlDataAdapter("Select * From Stock.dbo.Stock ", sqlConnection);
            DataTable dt = new DataTable();
            sql.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells["dgSno"].Value = n+1;
                dataGridView1.Rows[n].Cells["dgProCode"].Value = item["ProductsCode"].ToString();
                dataGridView1.Rows[n].Cells["dgProName"].Value = item["ProductsName"].ToString();
                dataGridView1.Rows[n].Cells["dgQuantity"].Value = float.Parse(item["Quantity"].ToString());
                dataGridView1.Rows[n].Cells["dgDate"].Value = Convert.ToDateTime(item["TransDate"].ToString()).ToString("dd/MM/yyyy");
                if ((bool)item["ProductsStatus"])
                {
                    dataGridView1.Rows[n].Cells["dgStatus"].Value = "Aktif";
                }
                else
                {
                    dataGridView1.Rows[n].Cells["dgStatus"].Value = "Pasif";
                }
            }
            if (dataGridView1.Rows.Count > 0)
            {
                label6.Text = dataGridView1.Rows.Count.ToString();
                float totQty = 0;
                for (int i = 0; i < dataGridView1.Rows.Count-1; ++i)
                {
                    totQty += float.Parse(dataGridView1.Rows[i].Cells["dgQuantity"].Value.ToString());
                    label7.Text = totQty.ToString();
                }
            }
            else
            {
                label6.Text = "0";
                label7.Text = "0";
            }
        }

        //Çift tıklama Bilgileri Çekme
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button1.Text = "Güncelle";
            textBox1.Text = dataGridView1.SelectedRows[0].Cells["dgProCode"].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells["dgProName"].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells["dgQuantity"].Value.ToString();
            dateTimePicker1.Text = DateTime.Parse(dataGridView1.SelectedRows[0].Cells["dgDate"].Value.ToString()).ToString("dd/MM/yyyy");
            if (dataGridView1.SelectedRows[0].Cells["dgStatus"].Value.ToString() == "Aktif")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }

        }

        //Silme
        private void button2_Click(object sender, EventArgs e)
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
                        sqlQuery = "Delete From Stock.dbo.Stock  Where ProductsCode='" + textBox1.Text + "'";
                        SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                        sqlCommand.ExecuteNonQuery();

                        sqlConnection.Close();
                        MessageBox.Show("Basarılı");
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

        //textboxa değer girince varsa verileri açılı kapanır dg ile gösterme
        private void textBox1_TextChanged(object sender, EventArgs e)
        {  /*//Code a göre dg2 otomatık veri taşıma.
            sqlConnection = Connection.GetConnection();
            sqlConnection.Open();
            SqlDataAdapter sql = new SqlDataAdapter("Select ProductsCode,ProductsName From Stock.dbo.Stock Where ProductsCode Like '%" + textBox1.Text + "%'", sqlConnection);
            DataTable dt = new DataTable();
            sql.Fill(dt);
            dataGridView2.DataSource = dt;*/
            if (textBox1.Text.Length > 0)
            {
                this.dgView.Visible = true;
                dgView.BringToFront();
                Search(150, 105, 430, 200, "Pro Code, Pro Name", "100,0");
                this.dgView.MouseDoubleClick += new MouseEventHandler(this.proCode_MouseDoubleClick);
                sqlConnection = Connection.GetConnection();
                sqlConnection.Open();
                SqlDataAdapter sql = new SqlDataAdapter("Select Top(10) ProductsCode,ProductsName From Stock.dbo.Stock Where ProductsCode Like '%" + textBox1.Text + "%'", sqlConnection);
                DataTable dt = new DataTable();
                sql.Fill(dt);
                dgView.Rows.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    int n = dgView.Rows.Add();
                    dgView.Rows[n].Cells[0].Value = row["ProductsCode"].ToString();
                    dgView.Rows[n].Cells[1].Value = row["ProductsName"].ToString();

                }
            }
            else
            {
                dgView.Visible = false;
            }
        }


        //Açılır Kapanır Dg tanımı
        private DataGridView dgView;
        private DataGridViewTextBoxColumn dgViewCol1;
        private DataGridViewTextBoxColumn dgViewCol2;       
        void Search()
        {
            dgView = new DataGridView();
            dgViewCol1 = new DataGridViewTextBoxColumn();
            dgViewCol2 = new DataGridViewTextBoxColumn();

            this.dgView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView.Columns.AddRange(new DataGridViewColumn[]
            {
                this.dgViewCol1,
                this.dgViewCol2
            }); 
            this.dgView.Name = "dgview";
            dgView.Visible = false;
            this.dgViewCol1.Visible = false;
            this.dgViewCol2.Visible = false;
            this.dgView.AllowUserToAddRows = false;
            this.dgView.RowHeadersVisible = false;
            this.dgView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.Controls.Add(dgView);
            this.dgView.ReadOnly = true;
            dgView.BringToFront();
        }
        void Search(int LX, int LY, int DW, int DH, string colName, String colSize )
        {
            this.dgView.Location = new Point(LX, LY);
            this.dgView.Size = new Size(DW, DH);

            string[] clSize = colSize.Split(',');
            for(int i=0; i< clSize.Length; i++)
            {
                if(int.Parse(clSize[i]) != 0)
                {
                    dgView.Columns[i].Width = int.Parse(clSize[i]);
                }
                else
                {
                    dgView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                }
            }

            string[] clName = colName.Split(',');
            for(int i=0; i<clName.Length; i++)
            {
                this.dgView.Columns[i].HeaderText = clName[i];
                this.dgView.Columns[i].Visible = true;
            }


        }

    }
}

