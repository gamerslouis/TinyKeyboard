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
    partial class Form1 : Form
    {
        private CenterControlGUIMessage ccmsg;
        private ModeContainer[] modeContainers = new ModeContainer[GlobalSetting.KeyMaxNumber];

        public Form1(CenterControlGUIMessage ccmsg)
        {
            InitializeComponent();
            this.ccmsg = ccmsg;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (var container in modeContainers)
            {
                container.AppendTo(this);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count >= GlobalSetting.KeyMaxNumber) return;
            Form form = new Form();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = "新增";
            textBox.Text = "";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            if (textBox.Text == "")
            {
                MessageBox.Show("名稱不可留白");
                return;
            }

            if (ccmsg.AddNewProfile(textBox.Text))
            {
                ReloadListBox();
                listBox1.SelectedIndex = ccmsg.GetProfileNames().Length - 1;
            }
            else
            {
                MessageBox.Show("錯誤", "設定檔存取失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("刪除", "是否刪除" + ccmsg.GetProfile(listBox1.SelectedIndex).Name
                , MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (ccmsg.RemoveProfile(ccmsg.GetCureentIndex()))
                {
                    MessageBox.Show("錯誤", "設定檔存取失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            ReloadListBox();
        }

        private void ReloadListBox()
        {
            listBox1.Items.Clear();
            foreach (var item in ccmsg.GetProfileNames())
            {
                listBox1.Items.Add(item);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowProfile(listBox1.SelectedIndex);
        }

        private void ShowProfile(int index)
        {
            var profile = ccmsg.GetProfile(index);
            for (int i = 0; i < GlobalSetting.KeyMaxNumber; i++)
            {
                modeContainers[i].SetInfoText.Text = profile.jSONModes[i].Set;
                modeContainers[i].ModeComboBox.Items.Clear();
                foreach (var mode in ModeFactory.Modes)
                {
                    modeContainers[i].ModeComboBox.Items.Add(mode.OptionInfo);
                    if (mode.Name == profile.jSONModes[i].Name) modeContainers[i].ModeComboBox.SelectedIndex = i;
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Form form = new Form();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = "更改名稱";
            textBox.Text = "";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            if (textBox.Text == "")
            {
                MessageBox.Show("名稱不可留白");
                return;
            }

            if (!ccmsg.ChangeProfileName(listBox1.SelectedIndex, textBox.Text))
            {
                MessageBox.Show("錯誤", "設定檔存取失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var i = listBox1.SelectedIndex;
            ReloadListBox();
            listBox1.SelectedIndex = i;
        }
    }

    public class ModeContainer
    {
        Button SetButton;
        public ComboBox ModeComboBox;
        public TextBox SetInfoText;

        public ModeContainer(int x, int y)
        {
            SetButton.SetBounds(x, y, 25, 25);

            ModeComboBox.SetBounds(x + 27, y, 180, 23);

            SetInfoText.SetBounds(x + 210, y, 200, 25);
        }

        public void AppendTo(Form form)
        {
            Control[] x = { SetButton, ModeComboBox, SetInfoText };
            form.Controls.AddRange(x);
        }
    }
}
