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

namespace HuynhNguyen_A03_PRN221
{
    public partial class MessageBox : Window
    {
        private System.Media.SoundPlayer player;

        public MessageBox()
        {
            InitializeComponent();
            player = new System.Media.SoundPlayer(@"D:\Alarm.wav");
            player.Play();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            this.Close();
        }
    }
}
