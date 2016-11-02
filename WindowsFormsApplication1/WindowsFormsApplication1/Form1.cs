using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(Print);
            t.Start();
            t.Join();
        }

        private void Print()
        {
            int y = 0;
            for (int i = 0; i < 100000000; i++)
                for (int j = 0; j < 100000000; j++)
                    y++;
        }
    }
}
