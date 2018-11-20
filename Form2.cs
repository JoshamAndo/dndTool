using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = @".\sounds\untitled.wav";
            player.Play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 newMess = new Form2();
            System.Threading.Thread.Sleep(1000);
            newMess.Show();
            this.Close();
        }
    }
}
