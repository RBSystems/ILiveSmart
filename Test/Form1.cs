using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Thread th = new Thread(new ThreadStart(this.Start));
            th.Start();
        }

        public void Start()
        {
            ushort i = 0;
            ushort j = 0;
            this.UIInvoke(() =>
            {
                 i = Convert.ToUInt16(textBox1.Text);
                 j = Convert.ToUInt16(textBox2.Text);

            });

            ushort t =(ushort) (i + j);
            this.UIInvoke(() =>
            {
                this.label1.Text = t.ToString();

            });
        }


    }

    /// <summary>
    /// 扩展控件
    /// </summary>
    public static class ControlExtended
    {
        public delegate void InvokeHandler();
        public static void UIInvoke(this Control control, InvokeHandler handler)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(handler);
            }
            else
            {
                handler();

            }
        }
    }

}
