using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;

namespace MyWallet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con;
        DataTable dt;

        private float pirMoney = 0, skrMoney = 0, walMoney = 0, totMoney = 0;

        public MainWindow()
        {
            InitializeComponent();

            //Connect your access database
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\myWalletDB.mdb";
            BindGrid();

            refreshValues();
        }

        //Display records in grid
        private void BindGrid()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            //get current month's number
            string cMonth = DateTime.Now.ToString("MM");

            // get current month's records
            cmd.CommandText = "SELECT * FROM Transactions WHERE MONTH(TransDate) = " + cMonth +"; ";

            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvData.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                //lblCount.Visibility = System.Windows.Visibility.Hidden;
                gvData.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                //lblCount.Visibility = System.Windows.Visibility.Visible;
                gvData.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        // run sql command
        internal void runSQLCmd(String cmdQuery)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            cmd.CommandText = cmdQuery;
            cmd.ExecuteNonQuery();

            BindGrid();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // execute some code

            DataRowView dr = gvData.SelectedItem as DataRowView;
            DataRow dr1 = dr.Row;

            // MessageBox.Show(Convert.ToString(dr1.ItemArray[0]));

            grayLayer.Visibility = System.Windows.Visibility.Visible;
            EditWindow editWin = new EditWindow(dr1.ItemArray[0].ToString(), dr1.ItemArray[1].ToString(), dr1.ItemArray[2].ToString(), dr1.ItemArray[3].ToString(), dr1.ItemArray[4].ToString());
            editWin.Owner = this;
            editWin.ShowDialog();

        }

        private void payButton_Click(object sender, RoutedEventArgs e)
        {
            grayLayer.Visibility = System.Windows.Visibility.Visible;
            PayWindow payWin = new PayWindow();
            payWin.Owner = this;
            payWin.ShowDialog();
        }

        private void getButton_Click(object sender, RoutedEventArgs e)
        {
            grayLayer.Visibility = System.Windows.Visibility.Visible;
            GetWindow getWin = new GetWindow();
            getWin.Owner = this;
            getWin.ShowDialog();
        }

        private void refreshValues()
        {
            totMoney = pirMoney + skrMoney + walMoney;

            piraeusMoney.Text = pirMoney.ToString("N2") + " €";
            skrillMoney.Text = skrMoney.ToString("N2") + " €";
            walletMoney.Text = walMoney.ToString("N2") + " €";
            totalMoney.Text = totMoney.ToString("N2") + " €";
        }
    }
}
