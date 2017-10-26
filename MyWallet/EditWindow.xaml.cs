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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private int trIdOld;
        private String trTypeOld, trDateOld, trMoneyOld, trDetailsOld;

        public EditWindow(String transId, String transType, String transDate, String transMoney, String transDetails)
        {
            InitializeComponent();

            ScrollViewer viewer = new ScrollViewer();
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            this.trIdOld = 
               Convert.ToInt32(transId);
            this.trTypeOld = transType;
            this.trDateOld = transDate.Substring(0,transDate.IndexOf(" "));
            this.trMoneyOld = transMoney;
            this.trDetailsOld = transDetails;

            transTypeOld.Text = transType;
            transDateOld.Text = transDate.Substring(0, transDate.IndexOf(" "));
            transMoneyOld.Text = transMoney;
            transDetailsOld.Text = transDetails;
        }

        // allow only numbers and comma in transMoney field
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // cancel edit
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            var myObject = this.Owner as MainWindow;
            myObject.grayLayer.Visibility = System.Windows.Visibility.Hidden;

            this.Close();
        }

        // update current transaction
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var myObject = this.Owner as MainWindow;
            String cmdQuery = "UPDATE Transactions SET TransType = '"+ transTypeNew.Text + "', TransDate = '" + transDateNew.Text + "', TransMoney = '" + transMoneyNew.Text + "', TransDetails = '" + transDetailsNew.Text + "' WHERE TransID = "+ trIdOld + ";";
            myObject.runSQLCmd(cmdQuery); // Call your method here.

            myObject.grayLayer.Visibility = System.Windows.Visibility.Hidden;

            this.Close();

            MessageBox.Show("Η συναλλαγή ενημερώθηκε.");
        }

        // delete current transaction
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var myObject = this.Owner as MainWindow;
            String cmdQuery = "DELETE FROM Transactions WHERE TransID = "+ trIdOld +";";
            myObject.runSQLCmd(cmdQuery); // Call your method here.

            myObject.grayLayer.Visibility = System.Windows.Visibility.Hidden;

            this.Close();

            MessageBox.Show("Η συναλλαγή διαγράφηκε.");
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
