using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyKeyboard
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

        public class ModeContainer
        {
            Button SetButton;
            ComboBox ModeComboBox;
            TextBox SetInfoText;

            public ModeContainer(int x, int y)
            {
                SetButton.SetBounds(x, y, 25, 25);

                ModeComboBox.SetBounds(x + 27, y, 180, 23);

                SetInfoText.SetBounds(x + 210, y, 200, 25);
            }

            public void AppendTo(ref Form form)
            {
                Control[] x = { SetButton, ModeComboBox, SetInfoText };
                form.Controls.AddRange(x);
            }
        }
    }
}
