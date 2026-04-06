using System.Drawing;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    internal static class FormGeometryHelper
    {
        public static void Save(Form form, OptionTreeView.Option<System.String> settingKey)
        {
            Rectangle bounds = form.WindowState == FormWindowState.Normal ? form.Bounds : form.RestoreBounds;
            int state = form.WindowState == FormWindowState.Maximized ? 1 : 0;
            settingKey.Value = $"{bounds.X},{bounds.Y},{bounds.Width},{bounds.Height},{state}";
            Properties.Settings.Default.Save();
        }

        public static void Restore(Form form, OptionTreeView.Option<System.String> settingKey)
        {
            string value = settingKey.Value;
            if (string.IsNullOrEmpty(value)) return;

            string[] parts = value.Split(',');
            if (parts.Length < 4) return;

            if (!int.TryParse(parts[0], out int x) ||
                !int.TryParse(parts[1], out int y) ||
                !int.TryParse(parts[2], out int w) ||
                !int.TryParse(parts[3], out int h))
                return;

            Rectangle savedBounds = new Rectangle(x, y, w, h);

            bool visible = false;
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(savedBounds))
                {
                    visible = true;
                    break;
                }
            }
            if (!visible) return;

            form.StartPosition = FormStartPosition.Manual;
            form.Location = savedBounds.Location;

            if (form.FormBorderStyle == FormBorderStyle.Sizable ||
                form.FormBorderStyle == FormBorderStyle.SizableToolWindow)
                form.Size = savedBounds.Size;

            if (parts.Length >= 5 && int.TryParse(parts[4], out int state) && state == 1)
                form.WindowState = FormWindowState.Maximized;
        }
    }
}
