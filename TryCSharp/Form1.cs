using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;

//using System.Reactive.Ob 

namespace TryCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {




        }

        private void button1_Click(object sender, EventArgs e)
        {
            Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
                handler => MouseMove += handler,
                handler => MouseMove -= handler)
                .Select(x => x.EventArgs)
                .Subscribe(x => lblMouse.Text = string.Format("{0}, {1}", x.X, x.Y));
        }
    }
}
