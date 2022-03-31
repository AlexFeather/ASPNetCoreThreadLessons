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

    public partial class MainWindow : Window
    {
        private bool fcStopper = true;

        private DateTime cdUntil;

        private object cdLock = new object();

        private MultithreadList<string> list;

        public MainWindow()
        {
            InitializeComponent();

            Thread fc = new Thread(FibonacciCounter);
            fc.IsBackground = true;
            fc.Start();

            Thread cd = new Thread(ItsTheFinalCountown);
            cd.IsBackground = true;
            cd.Start();

            list = new MultithreadList<string>();

            for(int i = 0; i < 10; i++)
            {
                Thread mtlt = new Thread(MultithreadListTester);
                mtlt.IsBackground = true;
                mtlt.Start();
            }

        }

        private void FibonacciCounter()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0, 1, ");

            long currentNum = 1;
            long previousNum = 0;
            long temp;

            while(fcStopper)
            {
                temp = currentNum + previousNum;
                previousNum = currentNum;
                currentNum = temp;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    FibonacciTextBox.Text = sb.Append($"{currentNum}, ").ToString()));
                lock (cdLock)
                {
                    cdUntil = DateTime.Now + TimeSpan.FromMilliseconds(5000);
                }
                Thread.Sleep(5000);
            }
        }

        private void ItsTheFinalCountown() //not really
        {
            TimeSpan countdown;
            while (fcStopper)
            {
                lock (cdLock)
                {
                    countdown = cdUntil - DateTime.Now;
                }
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    CountdownTextBox.Text = countdown.ToString()));
                Thread.Sleep(250);
            }

        }

        private void MultithreadListTester()
        {
            string item = "adfgh";
            while(true)
            {
                list.MultithreadAdd(item);
                Thread.Sleep(1000);
                list.MiltithreadRemove(item);
                Thread.Sleep(1000);
            }
        }
    }
}
