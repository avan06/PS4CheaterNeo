using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    /// <summary>
    /// https://www.csharp-examples.net/inputbox-class/
    /// </summary>
    public class InputBox
    {
        public static DialogResult Show(string title, string promptText, ref string value, int textHeight = 20, InputBoxValidation validation = null, int boxWidth = 400)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.BackColor = Color.FromArgb(90, 90, 90);
            form.ForeColor = Color.White;
            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, boxWidth - 24, 13);
            textBox.SetBounds(12, 36, boxWidth - 24, textHeight);
            buttonOk.SetBounds(boxWidth - 24 - 75 - 75, textHeight + 52, 75, 23);
            buttonCancel.SetBounds(boxWidth - 24 - 75 + 10, textHeight + 52, 75, 23);

            label.AutoSize = true;
            label.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            textBox.ImeMode = ImeMode.Off;
            textBox.Multiline = true;

            form.ClientSize = new Size(boxWidth, textHeight + 90);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(boxWidth, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.Sizable;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
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
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
    }
    public delegate string InputBoxValidation(string errorMessage);

    public static class MsgBox
    {
        public static Form form = null;
        public static Label label = null;
        public static Form Show(Form parent, string promptText, bool autoClose=true)
        {
            if (form != null && !form.IsDisposed)
            {
                label.Text = promptText;
                form.Refresh();
                return form;
            }
            form = new Form();
            label = new Label();

            form.Click += delegate {form.Close();};
            form.Paint += delegate (object sender, PaintEventArgs e) {
                int borderRadius = 10;
                float borderThickness = 3f;
                RectangleF Rect = new RectangleF(0, 0, form.Width, form.Height);
                GraphicsPath GraphPath = GetRoundPath(Rect, borderRadius);

                form.Region = new Region(GraphPath);
                using (Pen pen = new Pen(Color.Red, borderThickness))
                {
                    pen.Alignment = PenAlignment.Inset;
                    e.Graphics.DrawPath(pen, GraphPath);
                }
            };
            form.TopMost = true;
            form.FormBorderStyle = FormBorderStyle.None;
            form.ClientSize = new Size(parent.ClientSize.Width, 0);
            form.AutoSize = true;
            form.BackColor = Color.FromArgb(150, 150, 150);
            form.ForeColor = Color.White;
            form.Padding = new Padding(10);

            label.MaximumSize = new Size(parent.ClientSize.Width - 10 , 0);
            label.ForeColor = Color.DarkRed;
            label.BackColor = default;
            label.Text = promptText;
            label.AutoSize = true;
            label.Padding = new Padding(10);
            label.Location = new Point(10, 10);
            label.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            form.Controls.Add(label);

            form.Show(parent);
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(parent.Location.X + 5, parent.Location.Y + parent.Height - parent.Font.Height * 2);

            if (autoClose)
            {
                ///Automatically close a form after x minutes
                ///https://stackoverflow.com/a/45146407
                Timer tmr = new Timer();
                tmr.Tick += delegate {
                    tmr.Dispose();
                    form.Close();
                };
                tmr.Interval = 3000;
                tmr.Start();
            }

            return form;
        }

        /// <summary>
        /// Rounded edges in button C# (WinForms)
        /// https://stackoverflow.com/a/28486964
        /// </summary>
        private static GraphicsPath GetRoundPath(RectangleF Rect, int radius)
        {
            float m = 1.75F;
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();

            GraphPath.AddArc(Rect.X + m, Rect.Y + m, radius, radius, 180, 90);
            GraphPath.AddLine(Rect.X + r2 + m, Rect.Y + m, Rect.Width - r2 - m, Rect.Y + m);
            GraphPath.AddArc(Rect.X + Rect.Width - radius - m, Rect.Y + m, radius, radius, 270, 90);
            GraphPath.AddLine(Rect.Width - m, Rect.Y + r2, Rect.Width - m, Rect.Height - r2 - m);
            GraphPath.AddArc(Rect.X + Rect.Width - radius - m,
                           Rect.Y + Rect.Height - radius - m, radius, radius, 0, 90);
            GraphPath.AddLine(Rect.Width - r2 - m, Rect.Height - m, Rect.X + r2 - m, Rect.Height - m);
            GraphPath.AddArc(Rect.X + m, Rect.Y + Rect.Height - radius - m, radius, radius, 90, 90);
            GraphPath.AddLine(Rect.X + m, Rect.Height - r2 - m, Rect.X + m, Rect.Y + r2 + m);

            GraphPath.CloseFigure();
            return GraphPath;
        }
    }
}
