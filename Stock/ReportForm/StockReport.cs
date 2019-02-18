using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Stock.ReportForm
{
    public partial class StockReport : Form
    {
        SqlConnection sqlConnection = new SqlConnection();
        ReportDocument crypt = new ReportDocument();

        public StockReport()
        {
            InitializeComponent();
        }

        private void StockReport_Load(object sender, EventArgs e)
        {
            
        }

        DataSet dst = new DataSet();
        private void button1_Click(object sender, EventArgs e)
        {
            crypt.Load(@"C:\Users\hakan\Desktop\CSharp-WindowsForm\StockManagement\Stock\Reports\Stock.rpt");
            sqlConnection = Connection.GetConnection();
            sqlConnection.Open();
           
            SqlDataAdapter sda = new SqlDataAdapter("Select  * From Stock.dbo.Stock Where Cast(TransDate as Date) Between '" + dateTimePicker1.Value.ToString("MM/dd/yyyy") + "' And '" + dateTimePicker2.Value.ToString("MM/dd/yyyy") + "'", sqlConnection);
            sda.Fill(dst, "Stock");
            crypt.SetDataSource(dst);
            crypt.SetParameterValue("@FromDate", dateTimePicker1.Value.ToString("dd/MM/yyyy"));
            crypt.SetParameterValue("@ToDate", dateTimePicker2.Value.ToString("dd/MM/yyyy"));
            crystalReportViewer1.ReportSource = crypt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExportOptions exportOptions;
            DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Pdf Files|*.pdf";
            //sdf.Filter = "Excel|*.xls";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                diskFileDestinationOptions.DiskFileName = sfd.FileName;
            }
            exportOptions = crypt.ExportOptions;
            {
                exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //exportOptions.ExportFormatType = ExportFormatType.Excel;
                exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
                exportOptions.ExportFormatOptions = new PdfFormatOptions();
                //exportOptions.ExportFormatOptions = new ExcelFormatOptions();
            }
            crypt.Export();
        }
    }
}
