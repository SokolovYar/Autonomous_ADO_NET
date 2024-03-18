using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
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

namespace WpfApp1
{
    public partial class MainWindow : Window
    {

        private SqlConnection conn = null;
        SqlDataAdapter da = null;
        DataSet set = null;
        SqlCommandBuilder cmd = null;
        string cs = "";

        public MainWindow()
        {
            InitializeComponent();
            conn = new SqlConnection();
            cs = @" Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = TeaShop; Integrated Security = SSPI;";
            conn.ConnectionString = cs;
        }

        private void SendSQLQuery (string sql)
        {
            try
            {
                SqlConnection conn = new SqlConnection(cs);
                set = new DataSet();
                da = new SqlDataAdapter(sql, conn);
                dataGrid.ItemsSource = null;
                cmd = new SqlCommandBuilder(da);
                da.Fill(set, "tea_shop");
                DataView Source = new DataView(set.Tables[0]);
                dataGrid.Items.Refresh();
                dataGrid.ItemsSource = Source;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Menu_3_1_Click(object sender, RoutedEventArgs e)
        {
            // Отображение всей информации о чаях
            SendSQLQuery("SELECT * FROM Tea");
        }

        private void Menu_3_2_Click(object sender, RoutedEventArgs e)
        {
            // Отображение всех зелёных чаёв
            SendSQLQuery("SELECT * FROM Tea WHERE idType = 1");
        }

        private void Menu_3_3_Click(object sender, RoutedEventArgs e)
        {
            // Отображение всех чаёв, кроме зелёных и чёрных
            SendSQLQuery("SELECT* FROM Tea WHERE idType != 1 AND idType != 2");
        }


        private void Menu_4_1_Click(object sender, RoutedEventArgs e)
        {
            //Отображение самого дешёвого чая
            SendSQLQuery("SELECT  TOP 1 * FROM Tea ORDER BY costValue ASC");
        }

        private void Menu_4_2_Click(object sender, RoutedEventArgs e)
        {
            //Показать все чаи с минимальной стоимостью
            SendSQLQuery("SELECT * FROM Tea WHERE costValue = (SELECT MIN(costValue) FROM Tea)");
        }

        private void Menu_4_3_Click(object sender, RoutedEventArgs e)
        {
            //Показать все чаи со стоимостью выше средней
            SendSQLQuery("SELECT * FROM Tea WHERE costValue > (SELECT AVG(costValue) FROM Tea)");
        }

        private void Menu_4_4_Click(object sender, RoutedEventArgs e)
        {
            //Показать количество единиц каждого вида чая
            SendSQLQuery("SELECT typeName, COUNT(*) as quantity FROM Tea JOIN TypeTea ON TypeTea.id = Tea.idType GROUP BY idType, typeName");
        }

        private void Menu_4_5_Click(object sender, RoutedEventArgs e)
        {
            //Показать ТОП-3 стран по количеству чая
            SendSQLQuery("SELECT TOP 3 TeaCountry.country, COUNT(*) as quantity FROM Tea JOIN TeaCountry ON TeaCountry.id = Tea.idCountry GROUP BY TeaCountry.country ORDER BY quantity");
        }

        private void Menu_4_6_Click(object sender, RoutedEventArgs e)
        {
            //Показать ТОП-3 зелёных чаёв по весу
            SendSQLQuery("SELECT TOP 3 TypeTea.typeName, Tea.weight_ FROM Tea\r\nJOIN TypeTea ON TypeTea.id = Tea.idType\r\nORDER BY Tea.weight_ DESC");
        }


        private void Save_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                da.Update(set, "tea_shop");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}