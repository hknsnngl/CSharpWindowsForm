using BooksExamples;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace JoinQueries
{
    public partial class JoiningTableData : Form
    {
        public JoiningTableData()
        {
            InitializeComponent();
        }

        private void JoiningTableData_Load(object sender, EventArgs e)
        {
            var dbcontext = new BooksEntities();
            var authorsAndISBNs =
                from author in dbcontext.Authors
                from book in author.Titles
                orderby author.LastName, author.FirstName
                select new { author.FirstName, author.LastName, book.ISBN };
            outputTextBox.AppendText("Authors and ISBNs:");
            foreach (var element in authorsAndISBNs)
            {
                outputTextBox.AppendText($"\r\n\t{element.FirstName,-10} " +
                    $"{element.LastName,-10} {element.ISBN,-10}");
            }


            var authorsAndTitles =
                from book in dbcontext.Titles
                from author in book.Authors
                orderby author.LastName, author.FirstName, book.Title1
                select new { author.FirstName, author.LastName, book.Title1 };
            outputTextBox.AppendText("\r\n\r\nAuthors and titles:");
            foreach (var element in authorsAndTitles)
            {
                outputTextBox.AppendText($"\r\n\t{element.FirstName,-10} " +
                         $"{element.LastName,-10} {element.Title1}");

            }

            var titlesByAuthor =
                from author in dbcontext.Authors
                orderby author.LastName, author.FirstName
                select new{Name = author.FirstName + " " + author.LastName,Titles =
                from book in author.Titles
                orderby book.Title1
                select book.Title1
};
            outputTextBox.AppendText("\r\n\r\nTitles grouped by author:");
            foreach (var author in titlesByAuthor)
            {
                outputTextBox.AppendText($"\r\n\t{author.Name}:");
                foreach (var title in author.Titles)
                {
                    outputTextBox.AppendText($"\r\n\t\t{title}");
                }
            }


        }
    }
}
