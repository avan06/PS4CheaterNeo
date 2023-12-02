using System;
using System.Drawing;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    /// <summary>
    /// https://www.csharp-examples.net/inputbox-class/
    /// </summary>
    public class InputBox
    {
        public static void MsgBox(string title, string promptText, string value, int textHeight = 20, int boxWidth = 400, bool handleNewLine = true)
        {
            Form form = CreateForm(title, promptText, value, textHeight, null, boxWidth, false, handleNewLine);
            form.Show();
        }

        public static DialogResult Show(string title, string promptText, ref string value, int textHeight = 20, InputBoxValidation validation = null, int boxWidth = 400, bool showCancelBtn = true, bool handleNewLine = true)
        {
            TextBox textBox = new TextBox();
            Form form = CreateForm(title, promptText, value, textHeight, validation, boxWidth, showCancelBtn, handleNewLine, textBox);
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private static Form CreateForm(string title, string promptText, string value, int textHeight = 20, InputBoxValidation validation = null, int boxWidth = 400, bool showBtn = true, bool handleNewLine = true, TextBox text = null)
        {
            if (handleNewLine) value = value.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);

            Form form = new Form();
            Label label = new Label();
            TextBox textBox = text == null ? new TextBox() : text;
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.BackColor = Color.FromArgb(90, 90, 90);
            form.ForeColor = Color.White;
            form.Text = title;
            label.Text = promptText;
            int labelHeight = 13;
            int labelLines = label.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int labelYOffset = labelHeight * labelLines;
            int textHeightOffset = labelLines > 0 ? 0 : labelHeight * labelLines;

            label.SetBounds(9, 10, boxWidth - 24, labelHeight);
            label.AutoSize = true;
            label.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            textBox.Text = value;
            textBox.SetBounds(12, 13 + labelYOffset, boxWidth - 24, textHeight);
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            textBox.ImeMode = ImeMode.Off;
            textBox.Multiline = true;


            if (showBtn)
            {
                buttonOk.Text = "OK";
                buttonOk.DialogResult = DialogResult.OK;
                buttonOk.SetBounds(boxWidth - 24 - 75 - 75, textHeight + 20 + labelYOffset, 75, 23);
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                form.AcceptButton = buttonOk;

                buttonCancel.Text = "Cancel";
                buttonCancel.DialogResult = DialogResult.Cancel;
                buttonCancel.SetBounds(boxWidth - 24 - 75 + 10, textHeight + 20 + labelYOffset, 75, 23);
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                form.CancelButton = buttonCancel;
            }

            form.ClientSize = new Size(boxWidth, textHeight + 55 + labelYOffset);
            form.Controls.AddRange(new Control[] { label, textBox });
            if (showBtn) form.Controls.AddRange(new Control[] { buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(boxWidth, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            if (validation != null)
            {
                form.FormClosing += delegate (object sender, FormClosingEventArgs e) {
                    if (form.DialogResult == DialogResult.OK)
                    {
                        string errorText = validation(textBox.Text);
                        if (e.Cancel = (errorText != ""))
                        {
                            MessageBox.Show(form, errorText, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBox.Focus();
                        }
                    }
                };
            }

            return form;
        }
    }

    public delegate string InputBoxValidation(string errorMessage);
}
