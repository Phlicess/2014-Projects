﻿using System;
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
using System.Drawing;
using System.Printing;
using System.Drawing.Printing;
using Microsoft.Reporting.WinForms;
using System.IO;
using DSoft;
using DSoft.MethodExtension;
using DSoft.RDLCReport;

namespace DSoft.RDLC
{
    /// <summary>
    /// RDLCPrinterDialog
    /// <remarks>
    /// CREDIT : 2013-2014 Derek Tremblay (abbaye), Martin Savard
    /// https://rdlcprinter.codeplex.com/
    /// </remarks>
    /// </summary>
    public partial class RDLCPrinterDialog : Window
    {
        private PrintDocument _printer = new PrintDocument();
        private RDLCPrinter _report = null;
        private PrintQueue _currentPrinter = null;
        private LocalPrintServer _printServer = new LocalPrintServer();
        private List<PrintQueue> _printerList = new List<PrintQueue>();
        private string ImgSource; 

        public RDLCPrinter Report
        {
            get
            {
                return _report;
            }

            set
            {
                _report = value;

                if (_report.isDefaultLandscape == true)                
                    _printer.DefaultPageSettings.Landscape = true;   
            }
        }

        public RDLCPrinterDialog()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshWindow();
        }

        public void RefreshWindow()
        {

            if (this._report != null)
            {
                if (this._report.CopyNumber >= 1)
                {
                    NumberOfCopySpinner.Value = this._report.CopyNumber;
                }

                //update spinner
                FirstPageSpinner.Maximum = this._report.PagesCount;
                LastPageSpinner.Maximum = this._report.PagesCount;

                FirstPageSpinner.Value = 1;
                LastPageSpinner.Value = this._report.PagesCount;

                //Get all printer
                cboImprimanteNom.ItemsSource = _printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections }).Cast<PrintQueue>();
                cboImprimanteNom.DisplayMemberPath = "FullName";
                cboImprimanteNom.SelectedValue = "FullName";

                //Select Default printer
                _currentPrinter = LocalPrintServer.GetDefaultPrintQueue();
                for (int i = 0; i < cboImprimanteNom.Items.Count; i++)
                {
                    PrintQueue testPrint = (PrintQueue)cboImprimanteNom.Items[i];
                    if (testPrint.FullName.ToString() == _currentPrinter.FullName.ToString())
                    {
                        cboImprimanteNom.SelectedIndex = i;
                    }
                }

                //Check if printer is ready
                if (_currentPrinter.IsNotAvailable == false)
                {
                    lblImprimanteStatus.Content = "Ready";
                    ImgSource = @"pack://application:,,,/RDLCPrinter;component/Resources/Button-Blank-Green.ico";

                }
                else
                {
                    lblImprimanteStatus.Content = "Offline";
                    ImgSource = @"pack://application:,,,/RDLCPrinter;component/Resources/Button-Blank-Red.ico";

                }
                ReadyImage.Source = new BitmapImage(new Uri(ImgSource));
            }
        }

        /// <summary>
        /// Select user printer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboImprimanetNom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            lblImprimanteStatus.Content = "";

            _currentPrinter = (PrintQueue)cboImprimanteNom.SelectedItem;

            if (_currentPrinter.IsNotAvailable == false)
            {
                lblImprimanteStatus.Content = "Ready";                
                _printer.PrinterSettings.PrinterName = _currentPrinter.FullName;
                lblEmplacementImprimante.Content = _currentPrinter.QueuePort.Name;
                ImgSource = @"pack://application:,,,/RDLCPrinter;component/Resources/Button-Blank-Green.ico";
            }
            else
            {
                lblImprimanteStatus.Content = "Offline";
                ImgSource = @"pack://application:,,,/RDLCPrinter;component/Resources/Button-Blank-Red.ico";
            }

            ReadyImage.Source = new BitmapImage(new Uri(ImgSource));
        }

        /// <summary>
        /// Launch print
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {            
            PreparePrint();
            Report.PrintDoc = this._printer;

            if (NumberOfCopySpinner.Value.HasValue)
                Report.CopyNumber = NumberOfCopySpinner.Value.Value;
            else
                Report.CopyNumber = 1;

            Report.Print();
            this.Close();
        }

        /// <summary>
        /// Page to page for printing
        /// </summary>
        /// <returns></returns>
        private void PreparePrint()
        {
            if (cmdAllPageButton.IsChecked == false)
            {
                _printer.PrinterSettings.FromPage = FirstPageSpinner.Value.Value;
                _printer.PrinterSettings.ToPage = LastPageSpinner.Value.Value;
            }
        }


        /// <summary>
        /// Close window 
        /// </summary>        
        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Cancel ctrl-v (paste)
        /// </summary>        
        private void OnCancelCommand(object sender, DataObjectEventArgs e)
        {
            e.CancelCommand();
        }

        private void cmdAllPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (cmdAllPageButton.IsChecked == true)
            {
                PageChoiceStackPanel.IsEnabled = false;
                FirstPageSpinner.Value = 1;
                LastPageSpinner.Value = this._report.PagesCount;

            }
            else
                PageChoiceStackPanel.IsEnabled = true;
        }
    }
}
