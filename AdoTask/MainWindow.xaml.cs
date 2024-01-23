using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdoTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string sqlstring = "Data Source=LAPTOP-JLBUNNBV\\SQLEXPRESS;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public List<Category> Categories { get; set; }
        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<Book> SearchBooks { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Categories= GetCategories();
            Books = new ObservableCollection<Book>();
            SearchBooks = new ObservableCollection<Book>();
        }


        public static List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection sql = new SqlConnection(sqlstring))
            {
                try
                {
                    sql.Open();

                    string selectQuery = "SELECT * FROM Categories";

                    using (SqlCommand sqlCommand = new SqlCommand(selectQuery, sql))
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category
                            {
                                CategoryId = reader.GetInt32(reader.GetOrdinal("Id")),
                                CategoryName = reader.GetString(reader.GetOrdinal("Name"))
                            };

                            categories.Add(category);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return categories;
        }

        private void changed(object sender, SelectionChangedEventArgs e)
        {
            Books.Clear();
            int categoryid = int.Parse(category.SelectedValue.ToString());

            using (SqlConnection sql = new SqlConnection(sqlstring))
            {
                try
                {
                    sql.Open();

                    string selectQuery = "SELECT * FROM Books WHERE Books.Id_Category=@iid";


                    SqlCommand sqlCommand = new SqlCommand(selectQuery, sql);
                    sqlCommand.Parameters.AddWithValue("@iid", categoryid);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    {
                        while (reader.Read())
                        {
                            Book book = new Book();
                            {
                                book.BookName = reader.GetString(reader.GetOrdinal("Name"));
                              
                            };

                            Books.Add(book);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void txtchange(object sender, TextChangedEventArgs e)
        {
            SearchBooks.Clear();
            string sbook = search.Text;

            using (SqlConnection sql = new SqlConnection(sqlstring))
            {
                try
                {
                    sql.Open();

                    string selectQuery = "SELECT * FROM Books WHERE Books.Name LIKE @iid";
                    SqlCommand sqlCommand = new SqlCommand(selectQuery, sql);
                    sqlCommand.Parameters.AddWithValue("@iid", "%" + sbook + "%");
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    {
                        while (reader.Read())
                        {
                            Book book = new Book();
                            {
                                book.BookName = reader.GetString(reader.GetOrdinal("Name"));

                            };

                            SearchBooks.Add(book);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void leave(object sender, MouseEventArgs e)
        {
            SearchBooks.Clear();
        }
    }
}