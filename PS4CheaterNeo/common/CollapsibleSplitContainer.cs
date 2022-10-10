// forked from https://www.codeproject.com/Articles/820888/Collapsible-Split-Container
// Collapsible Split Container
// (c) 2014 Ed Gadziemski, v. 1.0.0.2
// Last updated 9/18/2014
// Licensed under Code Project Open License
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    public enum ButtonStyle { None, Image, SingleImage };
    public enum ButtonLocation { Panel, Panel1, Panel2 };
    public enum ButtonPosition { TopLeft, Center, BottomRight };
    public enum CollapseDistance { MinSize, Collapsed };

    public class CollapsibleSplitContainer : SplitContainer, ISupportInitialize
    {
        #region Variables
        private bool panel1Minimized = false;
        private bool panel2Minimized = false;
        private int splitterDistanceOriginal = 0;

        // Left-oriented bitmap from which the other three directional bitmaps are derived
        private Bitmap splitterButtonBitmap = null, bitmapRight = null, bitmapUp = null, bitmapDown = null;

        // Property fields
        private int splitterButtonSize;
        private ButtonStyle splitterButtonStyle;
        private ButtonLocation splitterButtonLocation;
        private ButtonPosition splitterButtonPosition;
        private CollapseDistance splitterCollapseDistance;

        private Button splitterButton1;
        private Button splitterButton2;
        private readonly string TableFillLeft = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAAuElEQVR4XqXTsQ3DIBAF0Is7iyYV9F4gK1hikWQEOjwBdIyQbJAsgJQVsgA9VGksSgdXJxFFoMs1V6D/kPjisG0btCbnfCxrHsfxUZ8NneHnuq73Ap27gDrsnDtpraEg1xoZesLee0gpQYwRdQTaYcYYGGNgmqZLeYdbBZDCCJDDCNDDCNAHgXLLu6xZKfWSUu6VwbIsEELA6n4BRAQBIoIAGUGgjXDOQQjR30KNWGv3ar8q/fs7fwCWA6ahjYFLQgAAAABJRU5ErkJggg==";
        #endregion

        public CollapsibleSplitContainer()
        {
            // Bug fix for SplitContainer problems with flickering and resizing
            ControlStyles cs = ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer;
            SetStyle(cs, true);
            object[] objArgs = new object[] { cs, true };
            MethodInfo objMethodInfo = typeof(Control).GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance);
            objMethodInfo.Invoke(Panel1, objArgs);
            objMethodInfo.Invoke(Panel2, objArgs);
            splitterButton1 = new Button();
            splitterButton2 = new Button();
            splitterButton1.Size = new Size(16, 16);
            splitterButton2.Size = new Size(16, 16);
            splitterButton1.Margin = new Padding(0);
            splitterButton2.Margin = new Padding(0);
            splitterButton1.BackgroundImageLayout = ImageLayout.Zoom;
            splitterButton2.BackgroundImageLayout = ImageLayout.Zoom;
            splitterButton1.BackColor = Color.Transparent;
            splitterButton2.BackColor = Color.Transparent;
            splitterButton1.FlatStyle = FlatStyle.Flat;
            splitterButton2.FlatStyle = FlatStyle.Flat;
            splitterButton1.FlatAppearance.BorderSize = 0;
            splitterButton2.FlatAppearance.BorderSize = 0;
            splitterButton1.Click += SplitterButton1_Click;
            splitterButton2.Click += SplitterButton2_Click;

            SplitterButtonSize = 16;
            SplitterButtonStyle = ButtonStyle.Image;
            SplitterButtonBitmap = Base64ToBitmap(TableFillLeft);
            SplitterCollapseDistance = CollapseDistance.MinSize;
            SplitterButtonLocation = ButtonLocation.Panel;
        }

        #region Properties
        [Category("Collapsible"), Description("The bitmap used on the splitter pushbuttons")]
        [DefaultValue(null)]
        public Bitmap SplitterButtonBitmap
        {
            get => splitterButtonBitmap;
            set
            {
                if (splitterButtonBitmap == value) return;

                splitterButtonBitmap = value;

                if (splitterButtonBitmap == null) splitterButtonBitmap = Base64ToBitmap(TableFillLeft);

                splitterButtonBitmap.MakeTransparent();
                // Create the bitmaps for the remaining directions
                bitmapRight = (Bitmap)splitterButtonBitmap.Clone();
                bitmapRight.RotateFlip(RotateFlipType.RotateNoneFlipX);
                bitmapUp = (Bitmap)splitterButtonBitmap.Clone();
                bitmapUp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                bitmapDown = (Bitmap)splitterButtonBitmap.Clone();
                bitmapDown.RotateFlip(RotateFlipType.Rotate270FlipNone);

                Refresh();
                UpdateSplitterButtonsImage();
            }
        }

        [Category("Collapsible"), Description("Where the collapse buttons are located on the splitter")]
        [DefaultValue(ButtonLocation.Panel)]
        public ButtonLocation SplitterButtonLocation
        {
            get => splitterButtonLocation;
            set
            {
                splitterButtonLocation = value;
                if (splitterCollapseDistance == CollapseDistance.Collapsed || splitterButtonLocation == ButtonLocation.Panel)
                {
                    if (!Panel1.Controls.Contains(splitterButton1)) Panel1.Controls.Add(splitterButton1);
                    if (!Panel2.Controls.Contains(splitterButton2)) Panel2.Controls.Add(splitterButton2);
                    if (Panel2.Controls.Contains(splitterButton1)) Panel2.Controls.Remove(splitterButton1);
                    if (Panel1.Controls.Contains(splitterButton2)) Panel1.Controls.Remove(splitterButton2);
                }
                else if (splitterButtonLocation == ButtonLocation.Panel1)
                {
                    if (!Panel1.Controls.Contains(splitterButton1)) Panel1.Controls.Add(splitterButton1);
                    if (!Panel1.Controls.Contains(splitterButton2)) Panel1.Controls.Add(splitterButton2);
                    if (Panel2.Controls.Contains(splitterButton1)) Panel2.Controls.Remove(splitterButton1);
                    if (Panel2.Controls.Contains(splitterButton2)) Panel2.Controls.Remove(splitterButton2);
                }
                else if (splitterButtonLocation == ButtonLocation.Panel2)
                {
                    if (!Panel2.Controls.Contains(splitterButton1)) Panel2.Controls.Add(splitterButton1);
                    if (!Panel2.Controls.Contains(splitterButton2)) Panel2.Controls.Add(splitterButton2);
                    if (Panel1.Controls.Contains(splitterButton1)) Panel1.Controls.Remove(splitterButton1);
                    if (Panel1.Controls.Contains(splitterButton2)) Panel1.Controls.Remove(splitterButton2);
                }
                else
                {
                    Panel1.Controls.Remove(splitterButton1);
                    Panel1.Controls.Remove(splitterButton2);
                    Panel2.Controls.Remove(splitterButton1);
                    Panel2.Controls.Remove(splitterButton2);
                }
                Refresh();
                UpdateSplitterButtonsPosition();
            }
        }

        [Category("Collapsible"), Description("Where the collapse buttons are located on the splitter")]
        [DefaultValue(ButtonPosition.TopLeft)]
        public ButtonPosition SplitterButtonPosition
        {
            get => splitterButtonPosition;
            set
            {
                if (splitterButtonPosition == value) return;
                splitterButtonPosition = value;
                Refresh();
                UpdateSplitterButtonsPosition();
            }
        }

        [Category("Collapsible"), Description("The technique used to generate the splitter buttons")]
        [DefaultValue(ButtonStyle.Image)]
        public ButtonStyle SplitterButtonStyle
        {
            get => splitterButtonStyle;
            set
            {
                if (splitterButtonStyle == value) return;
                splitterButtonStyle = value;
                if (splitterButtonStyle == ButtonStyle.None)
                {
                    splitterButton1.Hide();
                    splitterButton2.Hide();
                }
                else
                {
                    splitterButton1.Show();
                    splitterButton2.Show();
                }
                Refresh();
            }
        }

        [Category("Collapsible"), Description("Determines the splitter button size, the default is 16")]
        [DefaultValue(16)]
        public int SplitterButtonSize
        {
            get => splitterButtonSize;
            set
            {
                if (splitterButtonSize == value) return;
                splitterButtonSize = value;
                var size = splitterButton1.Size;
                size.Width = SplitterButtonSize;
                size.Height = SplitterButtonSize;
                splitterButton1.Size = size;
                splitterButton2.Size = size;
                Refresh();
            }
        }

        [Category("Collapsible"), Description("How completely the affected panel collapses")]
        [DefaultValue(CollapseDistance.MinSize)]
        public CollapseDistance SplitterCollapseDistance
        {
            get => splitterCollapseDistance;
            set
            {
                if (splitterCollapseDistance == value) return;
                if (value == CollapseDistance.MinSize)
                {
                    if (Panel1Collapsed)
                    {
                        panel1Minimized = true;
                        SplitterDistance = Panel1MinSize;
                    }
                    else if (Panel2Collapsed)
                    {
                        panel2Minimized = true;

                        // Calculate the splitter position
                        int distance = -1 * (Panel2MinSize + SplitterWidth);
                        distance += Orientation == Orientation.Vertical ? Panel1.Width : Panel1.Height;

                        SplitterDistance = distance;
                    }

                    Panel1Collapsed = false;
                    Panel2Collapsed = false;
                }
                else if (value == CollapseDistance.Collapsed)
                {
                    if (panel1Minimized) Panel1Collapsed = true;
                    else if (panel2Minimized) Panel2Collapsed = true;

                    panel1Minimized = false;
                    panel2Minimized = false;
                    SplitterButtonLocation = ButtonLocation.Panel;
                }

                splitterCollapseDistance = value;
                Refresh();
                UpdateSplitterButtonsPosition();
            }
        }

        [Category("Collapsible"), Description("Determines whether to collapse Panel2 when SplitterButtonStyle is SingleImage, otherwise Panel1 collapse")]
        [DefaultValue(true)]
        public bool SingleImageCollapsePanel2 { get; set; } = true;

        // Forces designer to refresh and reflect changes to the property
        public new bool IsSplitterFixed
        {
            get => base.IsSplitterFixed;
            set
            {
                if (base.IsSplitterFixed == value) return;
                base.IsSplitterFixed = value;
                Refresh();
            }
        }

        // SplitContainerOrientationDescr
        public new Orientation Orientation
        {
            get => base.Orientation;
            set
            {
                if (base.Orientation == value) return;

                base.Orientation = value;
                Refresh();
                UpdateSplitterButtonsPosition();
                UpdateSplitterButtonsImage();
            }
        }
        #endregion

        #region General Event Handlers
        private void SplitterButton1_Click(object sender, EventArgs e)
        {
            if (splitterButtonStyle == ButtonStyle.None) return;

            if (splitterCollapseDistance == CollapseDistance.Collapsed)
            {
                // Hide the panel associated with the clicked button
                if (Panel1Collapsed && !Panel2Collapsed) Panel2Collapsed = !Panel2Collapsed;
                else if (!Panel1Collapsed && Panel2Collapsed) Panel1Collapsed = !Panel1Collapsed;
                else Panel1Collapsed = true;
            }
            else if (splitterCollapseDistance == CollapseDistance.MinSize)
            {
                // If the panel for the clicked button is already minimized, do nothing
                // Otherwise, have the panel shrink to or return from the minimum size
                if (panel1Minimized)
                {
                    if (splitterButtonStyle == ButtonStyle.SingleImage) splitterButton2.BringToFront();
                    return;
                }
                else if (panel2Minimized) // Panel 2
                {
                    SplitterDistance = splitterDistanceOriginal;
                    panel2Minimized = false;
                }
                else // Panel 1
                {
                    splitterDistanceOriginal = SplitterDistance;
                    SplitterDistance = Panel1MinSize;
                    panel1Minimized = true;
                }
            }
            Refresh();
        }

        private void SplitterButton2_Click(object sender, EventArgs e)
        {
            if (splitterButtonStyle == ButtonStyle.None) return;

            if (splitterCollapseDistance == CollapseDistance.Collapsed)
            {
                // Hide the panel associated with the clicked button
                if (!Panel1Collapsed && Panel2Collapsed) Panel1Collapsed = !Panel1Collapsed;
                else if (Panel1Collapsed && !Panel2Collapsed) Panel2Collapsed = !Panel2Collapsed;
                else Panel2Collapsed = true;
            }
            else if (splitterCollapseDistance == CollapseDistance.MinSize)
            {
                // If the panel for the clicked button is already minimized, do nothing
                // Otherwise, have the panel shrink to or return from the minimum size
                if (panel2Minimized)
                {
                    if (splitterButtonStyle == ButtonStyle.SingleImage) splitterButton1.BringToFront();
                    return;
                }
                else if (panel1Minimized) // Panel 1
                {
                    SplitterDistance = splitterDistanceOriginal;
                    panel1Minimized = false;
                }
                else // Panel 2
                {
                    splitterDistanceOriginal = SplitterDistance;

                    // When the splitter is vertical, set the location of the splitter to
                    // the splitcontainer control width minus the minimum size of panel 2.
                    // For horizontal, set it to height minus panel 2 minimum size
                    if (Orientation == Orientation.Vertical) SplitterDistance = Width - Panel2MinSize;
                    else SplitterDistance = Height - Panel2MinSize;

                    panel2Minimized = true;
                }
            }
            Refresh();
        }

        /// <summary>
        /// Paint splitter and, if enabled, the buttons
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (IsSplitterFixed || splitterButtonStyle == ButtonStyle.None) return;

            UpdateSplitterButtonsPosition();
            UpdateSplitterButtonsImage();
        }
        #endregion

        #region Update Splitter Buttons
        /// <summary>
        /// Update the splitter buttons based on system capability
        /// </summary>
        private void UpdateSplitterButtonsPosition()
        {
            if (splitterButtonStyle == ButtonStyle.None) return;

            int position = GetButtonPosition();
            if (Orientation == Orientation.Vertical)
            {
                int width = splitterCollapseDistance == CollapseDistance.Collapsed ? 0 : (Panel1Collapsed ? Panel2.ClientRectangle.Right - splitterButton2.Width : Panel1.ClientRectangle.Right - splitterButton1.Width);

                if (splitterCollapseDistance == CollapseDistance.Collapsed || splitterButtonLocation == ButtonLocation.Panel)
                {
                    splitterButton1.Location = new Point(width, position);
                    splitterButton2.Location = new Point(0, position);
                }
                else if (SplitterButtonLocation == ButtonLocation.Panel1)
                {
                    splitterButton1.Location = new Point(width, position);
                    splitterButton2.Location = new Point(width, position + (splitterButtonStyle == ButtonStyle.SingleImage ? 0 : splitterButton1.Height));
                }
                else if (SplitterButtonLocation == ButtonLocation.Panel2)
                {
                    splitterButton1.Location = new Point(0, position);
                    splitterButton2.Location = new Point(0, position + (splitterButtonStyle == ButtonStyle.SingleImage ? 0 : splitterButton1.Height));
                }
            }
            else
            {
                int height = splitterCollapseDistance == CollapseDistance.Collapsed ? 0 : (Panel1Collapsed ? Panel2.ClientRectangle.Bottom - splitterButton2.Height : Panel1.ClientRectangle.Bottom - splitterButton1.Height);

                if (splitterCollapseDistance == CollapseDistance.Collapsed || splitterButtonLocation == ButtonLocation.Panel)
                {
                    splitterButton1.Location = new Point(position, height);
                    splitterButton2.Location = new Point(position, 0);
                }
                else if (SplitterButtonLocation == ButtonLocation.Panel1)
                {
                    splitterButton1.Location = new Point(position, height);
                    splitterButton2.Location = new Point(position + (splitterButtonStyle == ButtonStyle.SingleImage ? 0 : splitterButton1.Width), height);
                }
                else if (SplitterButtonLocation == ButtonLocation.Panel2)
                {
                    splitterButton1.Location = new Point(position, 0);
                    splitterButton2.Location = new Point(position + (splitterButtonStyle == ButtonStyle.SingleImage ? 0 : splitterButton1.Width), 0);
                }
            }
            if (splitterCollapseDistance == CollapseDistance.Collapsed || splitterButtonStyle == ButtonStyle.Image)
            {
                splitterButton1.BringToFront();
                splitterButton2.BringToFront();
            }
            else if (SplitterButtonStyle == ButtonStyle.SingleImage)
            {
                if (SingleImageCollapsePanel2)
                {
                    if (panel2Minimized) splitterButton1.BringToFront();
                    else splitterButton2.BringToFront();
                }
                else
                {
                    if (panel1Minimized) splitterButton2.BringToFront();
                    else splitterButton1.BringToFront();
                }
            }
        }

        /// <summary>
        /// Render the splitter buttons based on system capability and button style
        /// </summary>
        private void UpdateSplitterButtonsImage()
        {
            if (splitterButtonStyle == ButtonStyle.None) return;

            splitterButton1.BackgroundImage = Orientation == Orientation.Vertical ? splitterButtonBitmap : bitmapUp;
            splitterButton2.BackgroundImage = Orientation == Orientation.Vertical ? bitmapRight : bitmapDown;
        }
        #endregion

        #region Miscellaneous Helpers
        /// <summary>
        /// Get the button position based on splitterButtonPosition
        /// </summary>
        private int GetButtonPosition()
        {
            int position;

            int offset = (splitterButtonLocation == ButtonLocation.Panel || splitterButtonStyle == ButtonStyle.SingleImage) ? 0 : splitterButtonSize;
            Rectangle rect = Panel1Collapsed ? Panel2.ClientRectangle : Panel1.ClientRectangle;

            if (Orientation == Orientation.Vertical)
            {
                position = rect.Top;
                if (splitterButtonPosition == ButtonPosition.Center) position = rect.Bottom / 2 - splitterButton1.Height / 2 - offset / 2;
                else if (splitterButtonPosition == ButtonPosition.BottomRight) position = rect.Bottom - splitterButton1.Height - offset;
            }
            else
            {
                position = rect.Left;
                if (splitterButtonPosition == ButtonPosition.Center) position = rect.Right / 2 - splitterButton1.Width / 2 - offset / 2;
                else if (splitterButtonPosition == ButtonPosition.BottomRight) position = rect.Right - splitterButton1.Width - offset;
            }
            return position;
        }

        /// <summary>
        /// Decoding/Converting Base64 strings to Bitmap images
        /// https://softwarebydefault.com/2013/03/01/base64-strings-bitmap/
        /// </summary>
        public Bitmap Base64ToBitmap(string base64)
        {
            Bitmap bitmap = null;
            byte[] byteBuffer = Convert.FromBase64String(base64);
            using (MemoryStream memoryStream = new MemoryStream(byteBuffer))
            {
                memoryStream.Position = 0;
                bitmap = new Bitmap(Image.FromStream(memoryStream));
                byteBuffer = null;
            }

            return bitmap;
        }
        #endregion
    }
}