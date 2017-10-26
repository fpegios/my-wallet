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
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;

namespace MyWallet
{
    /// <summary>
    /// Interaction logic for PayWindow.xaml
    /// </summary>
    public partial class PayWindow : Window
    {
        OleDbConnection con;
        DataTable dt;

        private int trId;
        private String trDate;
        
        public PayWindow()
        {
            InitializeComponent();

            //Connect your access database
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\myWalletDB.mdb";

            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Transactions";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            trId = dt.Rows.Count + 1;
            transId.Text = "#" + trId;
        }

        // get the transDate as a string
        private void transDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get DatePicker reference.
            var picker = sender as DatePicker;

            // ... Get nullable DateTime from SelectedDate.
            DateTime? date = picker.SelectedDate;
            if (date == null)
            {
                // ... A null object.
                trDate = null;
            }
            else
            {
                // ... No need to display the time.
                trDate = date.Value.ToShortDateString();
            }
        }

        // allow only numbers and comma in transMoney field
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var myObject = this.Owner as MainWindow;
            myObject.grayLayer.Visibility = System.Windows.Visibility.Hidden;

            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            // check if the fields are empty
            if (trDate == null || transMoney == null || transDetails == null)
            {
                MessageBox.Show("Πρέπει να συμπληρώσετε όλα τα πλαίσια για να πραγματοποιηθεί η καταχώρηση της πληρωμής!");
            }
            // Check if comma (,) exists more than one time
            else if (Regex.Matches(transMoney.Text, ",").Count > 1)
            {
                MessageBox.Show("Λανθασμένη τιμή στο πλαίσιο 'Ποσό'!\nΔοκιμάστε πάλι.");
            }
            // create comand query and call MainWindow function to insert the records
            else
            {
                var myObject = this.Owner as MainWindow;
                String cmdQuery = "insert into Transactions(TransID, TransType, TransDate, TransMoney, TransDetails) Values(" + trId + ",'Πληρωμή','" + trDate + "','" + transMoney.Text + "','" + transDetails.Text + "');";
                myObject.runSQLCmd(cmdQuery); // Call your method here.

                myObject.grayLayer.Visibility = System.Windows.Visibility.Hidden;

                this.Close();
                MessageBox.Show("Η συναλλαγή καταχωρήθηκε.");
            }
            
        }

        
        // when close button (X) clicked, show the gray layer
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var myObject = this.Owner as MainWindow;
            myObject.grayLayer.Visibility = System.Windows.Visibility.Hidden;
            base.OnClosing(e);
        }
    }
}
