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
        private TimeSpan countdown = new TimeSpan(0, 0, 0);
        private DateTime countdownEnd;
        private readonly Object lockObject;
        bool active = true;



        public MainWindow()
        {
            InitializeComponent();
            Thread fc = new Thread(FibonacciCounter);
            Thread count = new Thread(ItsTheFinalCountdown);

            lockObject = new Object();

            fc.IsBackground = true;
            fc.Start();

            count.IsBackground = true;
            count.Start();

        }

        private void FibonacciCounter()
        {

            StringBuilder sb = new StringBuilder("0, 1");
            long currentNumber = 1;
            long previousNumber = 0;

            while (active)
            {
                long temp = currentNumber;
                currentNumber = currentNumber + previousNumber;
                previousNumber = temp;
                sb.Append($", {currentNumber}");

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    FibonacciTextBox.Text = sb.ToString()));

                lock (lockObject)
                {
                    countdownEnd = DateTime.Now + TimeSpan.FromMilliseconds(5000);
                }
                Thread.Sleep(5000);
            }
        }

        private void ItsTheFinalCountdown()
        {
            while(active)
            {
                lock (lockObject)
                {
                    countdown = countdownEnd - DateTime.Now;
                }

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    CountdownTextBox.Text = countdown.ToString()));

                Thread.Sleep(100);
            }
        }
    }
}
