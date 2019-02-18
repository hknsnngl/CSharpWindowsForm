using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stock
{
    public partial class StockMain : Form
    {
        //Widows State = Maximized; ile ekranı kaplamasını sağlar.
        public StockMain()
        {
            InitializeComponent();
        }

        //Products.cs Formu açmaya yarar.
        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Products products = new Products();
            products.MdiParent = this;
            products.StartPosition = FormStartPosition.CenterScreen;   
            products.Show();
        }

        // Uygulamayı kapatmaya yarar.
        bool close = true;
        private void StockMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (close)
            {   //DialogResult ile Evet/Hayır sorulu MessageBox kutusu yapmaya yarar.
                 DialogResult dialogResult = MessageBox.Show("Kapatmak istediğine emin misin?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    close = false;
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;

                }
            }
        }

        //Stock.cs Formu açmaya yarar.
        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stock stock = new Stock();
            stock.MdiParent = this;
            stock.StartPosition = FormStartPosition.CenterParent;
            stock.Show();
        }

        //ProductsReport.cs formu açmaya yarar.
        private void productListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm.ProductsReport productsReport = new ReportForm.ProductsReport();
            productsReport.MdiParent = this;
            productsReport.StartPosition = FormStartPosition.CenterParent;
            productsReport.Show();
        }

        //StockReport.cs Formu açmaya yarar.
        private void stockListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm.StockReport stockReport = new ReportForm.StockReport();
            stockReport.MdiParent = this;
            stockReport.StartPosition = FormStartPosition.CenterParent;
            stockReport.Show();
        }

        private void StockMain_Load(object sender, EventArgs e)
        {

        }
    }
}
