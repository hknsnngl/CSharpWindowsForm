using BooksExamples;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace MasterDetail
{
    public partial class Details : Form
    {
        public Details()
        {
            InitializeComponent();
        }
        BooksEntities dbcontext = new BooksEntities();
        private void Details_Load(object sender, EventArgs e)
        {
            dbcontext.Authors
                .OrderBy(author => author.LastName)
                .ThenBy(author => author.FirstName)
                .Load();
            authorBindingSource.DataSource = dbcontext.Authors.Local;
        }

    }
}
