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
        private int splitterDistanceOriginal = 0;

        // Left-oriented bitmap from which the other three directional bitmaps are derived
        private Bitmap splitterButtonBitmap = null, bitmapRight = null, bitmapUp = null, bitmapDown = null;

        // Property fields
        private int splitterButtonSize;
        private ButtonStyle splitterButtonStyle;
        private ButtonLocation splitterButtonLocation;
        private ButtonPosition splitterButtonPosition;
        private CollapseDistance splitterCollapseDistance;

        private Button SplitterButton1;
        private Button SplitterButton2;
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
            SplitterButton1 = new Button();
            SplitterButton2 = new Button();
            SplitterButton1.Size = new Size(16, 16);
            SplitterButton2.Size = new Size(16, 16);
            SplitterButton1.Margin = new Padding(0);
            SplitterButton2.Margin = new Padding(0);
            SplitterButton1.BackgroundImageLayout = ImageLayout.Zoom;
            SplitterButton2.BackgroundImageLayout = ImageLayout.Zoom;
            SplitterButton1.BackColor = Color.Transparent;
            SplitterButton2.BackColor = Color.Transparent;
            SplitterButton1.FlatStyle = FlatStyle.Flat;
            SplitterButton2.FlatStyle = FlatStyle.Flat;
            SplitterButton1.FlatAppearance.BorderSize = 0;
            SplitterButton2.FlatAppearance.BorderSize = 0;
            SplitterButton1.Click += SplitterButton1_Click;
            SplitterButton2.Click += SplitterButton2_Click;

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

        [Category("Collapsible"), Description("Determines whether to place collapse buttons on both sides of the splitter or on one side of Panel1 and Panel2")]
        [DefaultValue(ButtonLocation.Panel)]
        public ButtonLocation SplitterButtonLocation
        {
            get => splitterButtonLocation;
            set
            {
                splitterButtonLocation = value;
                if (splitterCollapseDistance == CollapseDistance.Collapsed || splitterButtonLocation == ButtonLocation.Panel)
                {
                    if (!Panel1.Controls.Contains(SplitterButton1)) Panel1.Controls.Add(SplitterButton1);
                    if (!Panel2.Controls.Contains(SplitterButton2)) Panel2.Controls.Add(SplitterButton2);
                    if (Panel2.Controls.Contains(SplitterButton1)) Panel2.Controls.Remove(SplitterButton1);
                    if (Panel1.Controls.Contains(SplitterButton2)) Panel1.Controls.Remove(SplitterButton2);
                }
                else if (splitterButtonLocation == ButtonLocation.Panel1)
                {
                    if (!Panel1.Controls.Contains(SplitterButton1)) Panel1.Controls.Add(SplitterButton1);
                    if (!Panel1.Controls.Contains(SplitterButton2)) Panel1.Controls.Add(SplitterButton2);
                    if (Panel2.Controls.Contains(SplitterButton1)) Panel2.Controls.Remove(SplitterButton1);
                    if (Panel2.Controls.Contains(SplitterButton2)) Panel2.Controls.Remove(SplitterButton2);
                }
                else if (splitterButtonLocation == ButtonLocation.Panel2)
                {
                    if (!Panel2.Controls.Contains(SplitterButton1)) Panel2.Controls.Add(SplitterButton1);
                    if (!Panel2.Controls.Contains(SplitterButton2)) Panel2.Controls.Add(SplitterButton2);
                    if (Panel1.Controls.Contains(SplitterButton1)) Panel1.Controls.Remove(SplitterButton1);
                    if (Panel1.Controls.Contains(SplitterButton2)) Panel1.Controls.Remove(SplitterButton2);
                }
                else
                {
                    Panel1.Controls.Remove(SplitterButton1);
                    Panel1.Controls.Remove(SplitterButton2);
                    Panel2.Controls.Remove(SplitterButton1);
                    Panel2.Controls.Remove(SplitterButton2);
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

        [Category("Collapsible"), Description("Determines the style of splitter buttons: Image - displayed on both sides of the panel; SingleImage - displayed on one side; None - not displayed.")]
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
                    SplitterButton1.Hide();
                    SplitterButton2.Hide();
                }
                else
                {
                    SplitterButton1.Show();
                    SplitterButton2.Show();
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
                var size = SplitterButton1.Size;
                size.Width = SplitterButtonSize;
                size.Height = SplitterButtonSize;
                SplitterButton1.Size = size;
                SplitterButton2.Size = size;
                Refresh();
            }
        }

        [Category("Collapsible"), Description("Determines the style of the splitter after collapsing: MinSize - the panel collapses to the specified MinSize; Collapsed - One-sided panel completely collapsed")]
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
                        Panel1Minimized = true;
                        SplitterDistance = Panel1MinSize;
                    }
                    else if (Panel2Collapsed)
                    {
                        Panel2Minimized = true;

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
                    if (Panel1Minimized) Panel1Collapsed = true;
                    else if (Panel2Minimized) Panel2Collapsed = true;

                    Panel1Minimized = false;
                    Panel2Minimized = false;
                    SplitterButtonLocation = ButtonLocation.Panel;
                }

                splitterCollapseDistance = value;
                Refresh();
                UpdateSplitterButtonsPosition();
            }
        }

        [Category("Collapsible"), Description("Determines whether to collapse Panel2; if not, Panel1 will collapse. This setting only takes effect when SplitterButtonStyle is SingleImage and the location of the splitter button is on either Panel1 or Panel2.")]
        [DefaultValue(true)]
        public bool SingleImageCollapsePanel2 { get; set; } = true;

        /// <summary>
        /// Can be used to confirm whether panel1 is currently minimized.
        /// </summary>
        public bool Panel1Minimized { get; private set; } = false;

        /// <summary>
        /// Can be used to confirm whether panel2 is currently minimized.
        /// </summary>
        public bool Panel2Minimized { get; private set; } = false;

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
        private void SplitterButton1_Click(object sender, EventArgs e) => SplitterPanelCollapseExpand(true);

        private void SplitterButton2_Click(object sender, EventArgs e) => SplitterPanelCollapseExpand(false);

        private bool clickSplitterButtonStaus = false;

        /// <summary>
        /// Trigger the collapse or expand of the splitter panel. 
        /// When status parameter is true, Panel1 collapses; when status is false, Panel2 collapses. 
        /// If the status parameter is not provided, it will automatically determine whether to collapse or expand.
        /// </summary>
        public void SplitterPanelCollapseExpand(bool? status = null)
        {
            if (splitterButtonStyle == ButtonStyle.None) return;

            if (status == null)
            {
                status = clickSplitterButtonStaus;
                clickSplitterButtonStaus = !clickSplitterButtonStaus;
            }
            object sender = (bool)status ? SplitterButton1 : SplitterButton2;

            if (splitterCollapseDistance == CollapseDistance.Collapsed)
            {
                // Hide the panel associated with the clicked button
                if (Panel1Collapsed && !Panel2Collapsed) Panel2Collapsed = !Panel2Collapsed;
                else if (!Panel1Collapsed && Panel2Collapsed) Panel1Collapsed = !Panel1Collapsed;
                else if (sender == SplitterButton1) Panel1Collapsed = true;
                else if (sender == SplitterButton2) Panel2Collapsed = true;
            }
            else if (splitterCollapseDistance == CollapseDistance.MinSize)
            {
                if (!Panel1Minimized && !Panel2Minimized)
                {
                    splitterDistanceOriginal = SplitterDistance;
                    if (sender == SplitterButton1)
                    { // Panel 1
                        (SplitterDistance, Panel1Minimized) = (Panel1MinSize, true);
                    }
                    else if (sender == SplitterButton2)
                    { // Panel 2
                        // When the splitter is vertical, set the location of the splitter to
                        // the splitcontainer control width minus the minimum size of panel 2.
                        // For horizontal, set it to height minus panel 2 minimum size
                        (SplitterDistance, Panel2Minimized) = (Orientation == Orientation.Vertical ? Width - Panel2MinSize : Height - Panel2MinSize, true);
                    }
                }
                // If the panel for the clicked button is already minimized, do nothing
                // Otherwise, have the panel shrink to or return from the minimum size
                else if (Panel1Minimized && sender == SplitterButton2)
                    (SplitterDistance, Panel1Minimized) = (splitterDistanceOriginal, false);
                else if (Panel2Minimized && sender == SplitterButton1)
                    (SplitterDistance, Panel2Minimized) = (splitterDistanceOriginal, false);
                else
                {
                    if (Panel1Minimized && sender == SplitterButton1 && splitterButtonStyle == ButtonStyle.SingleImage)
                        SplitterButton2.BringToFront();
                    else if (Panel2Minimized && sender == SplitterButton2 && splitterButtonStyle == ButtonStyle.SingleImage)
                        SplitterButton1.BringToFront();
                    return;
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
                int width = splitterCollapseDistance == CollapseDistance.Collapsed ? 0 : (Panel1Collapsed ? Panel2.ClientRectangle.Right - SplitterButton2.Width : Panel1.ClientRectangle.Right - SplitterButton1.Width);

                if (splitterCollapseDistance == CollapseDistance.Collapsed || splitterButtonLocation == ButtonLocation.Panel)
                {
                    SplitterButton1.Location = new Point(width, position);
                    SplitterButton2.Location = new Point(0, position);
                }
                else if (SplitterButtonLocation == ButtonLocation.Panel1)
                {
                    SplitterButton1.Location = new Point(width, position);
                    SplitterButton2.Location = new Point(width, position + (splitterButtonStyle == ButtonStyle.SingleImage ? 0 : SplitterButton1.Height));
                }
                else if (SplitterButtonLocation == ButtonLocation.Panel2)
                {
                    SplitterButton1.Location = new Point(0, position);
                    SplitterButton2.Location = new Point(0, position + (splitterButtonStyle == ButtonStyle.SingleImage ? 0 : SplitterButton1.Height));
                }
            }
            else
            {
                int height = splitterCollapseDistance == CollapseDistance.Collapsed ? 0 : (Panel1Collapsed ? Panel2.ClientRectangle.Bottom - SplitterButton2.Height : Panel1.ClientRectangle.Bottom - SplitterButton1.Height);

                if (splitterCollapseDistance == CollapseDistance.Collapsed || splitterButtonLocation == ButtonLocation.Panel)
                {
                    SplitterButton1.Location = new Point(position, height);
                    SplitterButton2.Location = new Point(position, 0);
                }
                else if (SplitterButtonLocation == ButtonLocation.Panel1)
                {
                    SplitterButton1.Location = new Point(position, height);
                    SplitterButton2.Location = new Point(position + (splitterButtonStyle == ButtonStyle.SingleImage ? 0 : SplitterButton1.Width), height);
                }
                else if (SplitterButtonLocation == ButtonLocation.Panel2)
                {
                    SplitterButton1.Location = new Point(position, 0);
                    SplitterButton2.Location = new Point(position + (splitterButtonStyle == ButtonStyle.SingleImage ? 0 : SplitterButton1.Width), 0);
                }
            }
            if (splitterCollapseDistance == CollapseDistance.Collapsed || splitterButtonStyle == ButtonStyle.Image)
            {
                SplitterButton1.BringToFront();
                SplitterButton2.BringToFront();
            }
            else if (SplitterButtonStyle == ButtonStyle.SingleImage)
            {
                if (SingleImageCollapsePanel2) SplitterBtnBringToFront(!Panel2Minimized);
                else SplitterBtnBringToFront(Panel1Minimized);
            }
        }

        private void SplitterBtnBringToFront(bool? Panel1Minimized = null)
        {
            if (Panel1Minimized == null)
            {
                SplitterButton1.BringToFront();
                SplitterButton2.BringToFront();
            }
            if ((bool)Panel1Minimized) SplitterButton2.BringToFront();
            else SplitterButton1.BringToFront();
        }

        /// <summary>
        /// Render the splitter buttons based on system capability and button style
        /// </summary>
        private void UpdateSplitterButtonsImage()
        {
            if (splitterButtonStyle == ButtonStyle.None) return;

            SplitterButton1.BackgroundImage = Orientation == Orientation.Vertical ? splitterButtonBitmap : bitmapUp;
            SplitterButton2.BackgroundImage = Orientation == Orientation.Vertical ? bitmapRight : bitmapDown;
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
                if (splitterButtonPosition == ButtonPosition.Center) position = rect.Bottom / 2 - SplitterButton1.Height / 2 - offset / 2;
                else if (splitterButtonPosition == ButtonPosition.BottomRight) position = rect.Bottom - SplitterButton1.Height - offset;
            }
            else
            {
                position = rect.Left;
                if (splitterButtonPosition == ButtonPosition.Center) position = rect.Right / 2 - SplitterButton1.Width / 2 - offset / 2;
                else if (splitterButtonPosition == ButtonPosition.BottomRight) position = rect.Right - SplitterButton1.Width - offset;
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