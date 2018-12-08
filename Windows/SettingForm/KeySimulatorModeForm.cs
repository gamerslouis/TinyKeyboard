using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyKeyboard.SettingForm
{
    class KeySimulatorModeForm : ISettingForm
    {
        string ISettingForm.GetSet()
        {
            Int32 value = 0;

            Form form = new Form();
            Button buttonCancel = new Button();

            form.Text = "請按任意按鍵以設定";

            buttonCancel.Text = "取消";
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.SetBounds(2, 2, 98, 28);

            form.Size = new System.Drawing.Size(200, 70);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.Controls.Add(buttonCancel);
            form.CancelButton = buttonCancel;
            form.KeyPreview = true;

            form.KeyDown += (object sender, KeyEventArgs e) =>
            {
                value = (Int32)e.KeyCode;
                form.DialogResult = DialogResult.OK;
            };

            switch (form.ShowDialog())
            {
                case DialogResult.OK: return value.ToString();
                case DialogResult.Cancel: return "cancel";
                default: return "cancel";
            }
        }
    }
}
