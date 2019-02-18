using CrystalDecisions.CrystalReports.Engine;
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

namespace Stock.ReportForm
{
    public partial class ProductsReport : Form
    {
        SqlConnection sqlConnection = new SqlConnection();
        ReportDocument crypt = new ReportDocument();

        public ProductsReport()
        {
            InitializeComponent();
        }

        private void ProductsReport_Load(object sender, EventArgs e)
        {
            crypt.Load(@"C:\Users\hakan\Desktop\CSharp-WindowsForm\StockManagement\Stock\Reports\Product.rpt");
            sqlConnection = Connection.GetConnection();
            sqlConnection.Open();
            DataSet dst = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter("Select * From Stock.dbo.Products",sqlConnection);
            //DataTable dt = new DataTable();
            sda.Fill(dst,"Products");//sda.Fill(dt);
            sqlConnection.Close();
            DataTable dt = new DataTable();
            dt.Columns.Add("WaterMarkPath", typeof(string));
            dt.Rows.Add(@"D:\RESIM\201811\118APPLE\IMG_8533.jpg");
            crypt.Database.Tables["Products"].SetDataSource(dst.Tables[0]);
            crypt.Database.Tables["ReportDetails"].SetDataSource(dt);

            //crypt.SetDataSource(dt);
            crystalReportViewer1.ReportSource = crypt;

        }
    }
}
