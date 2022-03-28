using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace ThreadsLessons
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            Thread fc = new Thread(FibonacciCounter);
            fc.IsBackground = true;
            fc.Start();
        }

        private void FibonacciCounter()
        {
            bool active = true;
            StringBuilder sb = new StringBuilder("0, 1");
            int currentNumber = 1;
            int previousNumber = 0;

            while (active)
            {
                int temp = currentNumber;
                currentNumber = currentNumber + previousNumber;
                previousNumber = temp;
                sb.Append($", {currentNumber}");
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                FibonacciTextBox.Text = sb.ToString()));
                
                Thread.Sleep(1000);
            }
        }
    }
}
