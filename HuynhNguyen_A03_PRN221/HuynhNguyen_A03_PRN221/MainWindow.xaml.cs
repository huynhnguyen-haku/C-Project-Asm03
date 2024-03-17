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

namespace HuynhNguyen_A03_PRN221
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private CancellationTokenSource cts;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 24; i++)
            {
                cb_Hour.Items.Add(i);
            }

            for (int i = 0; i < 60; i++)
            {
                cb_Minute.Items.Add(i);
            }

            Thread timeThread = new Thread(UpdateTime);
            timeThread.Start();
        }
        private void UpdateTime()
        {
            try
            {
                while (true)
                {
                    DateTime now = DateTime.Now;
                    Dispatcher.Invoke(() =>
                    {
                        lb_Hour.Content = now.Hour.ToString("00");
                        lb_Minute.Content = now.Minute.ToString("00");
                        lb_Second.Content = now.Second.ToString("00");
                    });
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã có lỗi xảy ra: {ex.Message}");
            }
        }

        private void btn_SetAlarm_Click(object sender, RoutedEventArgs e)
        {
            int hour = (int)cb_Hour.SelectedItem;
            int minute = (int)cb_Minute.SelectedItem;

            DateTime now = DateTime.Now;
            DateTime alarmTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

            if (alarmTime < now)
            {
                alarmTime = alarmTime.AddDays(1);
            }

            int countdownTime = (int)(alarmTime - now).TotalSeconds;

            lb_TimeLeft.Content = $"Thời gian còn lại trước khi báo thức: {countdownTime / 3600:00}:{(countdownTime / 60) % 60:00}:{countdownTime % 60:00}";
            lb_TimeLeft.Visibility = Visibility.Visible;

            if (cts != null)
            {
                cts.Cancel();
                cts = null;
            }

            // Tạo và khởi động hoạt động đếm ngược mới
            cts = new CancellationTokenSource();
            var token = cts.Token;
            Task.Run(() => Countdown(countdownTime, token), token);
        }

        private async Task Countdown(int countdownTime, CancellationToken token)
        {
            while (countdownTime > 0)
            {
                int hour = countdownTime / 3600;
                int minute = (countdownTime % 3600) / 60;
                int second = countdownTime % 60;

                Dispatcher.Invoke(() =>
                {
                    lb_TimeLeft.Content = $"Thời gian còn lại trước khi báo thức: {hour:00}:{minute:00}:{second:00}";
                });

                countdownTime--;

                await Task.Delay(1000);

                if (token.IsCancellationRequested)
                {
                    return;
                }
            }

            Dispatcher.Invoke(() =>
            {
                lb_TimeLeft.Visibility = Visibility.Hidden;
                var messageBox = new MessageBox();
                messageBox.Show();
            });
        }
    }
}