using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gurwell__Alex___Game_Of_Life
{
    public partial class Seed : Form
    {
        public Seed()
        {
            InitializeComponent();
        }
        public int seed
        {
            get { return (int)_seed.Value; }
            set { _seed.Value = value; }
        }

        private void Randomize_Click(object sender, EventArgs e)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            _seed.Value = rand.Next();
        }
    }
}
