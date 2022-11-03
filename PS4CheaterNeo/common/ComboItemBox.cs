using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public class ComboItemBox : ComboBox
    {
        public ComboItemBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void AddItem(object text, object value) => AddItem(text, value, Color.Black, Color.White, null);
        
        public void AddItem(object text, object value, Color backColor, Color foreColor, Icon icon = null) => Items.Add(new ComboItem(text, value, backColor, foreColor, icon));

        /// <summary>
        /// Draws the items into the ComboBoxImg object
        /// https://stackoverflow.com/a/9706102
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();
            e.DrawFocusRectangle();

            Rectangle rect = e.Bounds; //Rectangle of item
            ComboItem item = (ComboItem)Items[e.Index];
            using (Brush brush = new SolidBrush(e.ForeColor))
            using (Graphics g = e.Graphics)
            {
                if (item.ICON == null) g.DrawString(item.Text.ToString(), e.Font, brush, rect.X, rect.Top); //Draw the item name
                else
                {
                    using (Pen pen = new Pen(item.ForeColor, 2)) g.DrawRectangle(pen, rect);
                    // Draw the colored 16 x 16 square
                    var img = item.ICON.ToBitmap();
                    if (img.Width > 16 || img.Height > 16) img = ResizeImage(img, 16, 16);
                    // Draw the ICON
                    g.DrawImage(img, e.Bounds.Left, e.Bounds.Top);
                    //Draw the item name
                    g.DrawString(item.Text.ToString(), e.Font, brush, e.Bounds.Left + img.Width, e.Bounds.Top + 2);
                }
            }

            base.OnDrawItem(e);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// https://stackoverflow.com/a/24199315
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var wrapMode = new ImageAttributes())
            using (var graphics = Graphics.FromImage(destImage))
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }
    }

    public class ComboItem
    {
        public object Text { get; set; }
        public object Value { get; set; }
        public Icon ICON { get; set; }
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }

        public ComboItem(object text, object value = null) : this(text, value, Color.Black, Color.White, null) { }

        public ComboItem(object text, object value, Color backColor, Color foreColor, Icon icon = null)
        {
            Text = text;
            Value = value;
            BackColor = backColor;
            ForeColor = foreColor;
            ICON = icon;
        }

        public override string ToString() => Text.ToString();
    }
}
