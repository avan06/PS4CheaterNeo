using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    /// <summary>
    /// Select all listview
    /// https://stackoverflow.com/a/1118396
    /// </summary>
    public class ListViewLVITEM
    {
        private const int LVM_FIRST = 0x1000;
        private const int LVM_SETITEMSTATE = LVM_FIRST + 43;

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, ref LVITEM lvi);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LVITEM
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszText;
            public int cchTextMax;
            public int iImage;
            public IntPtr lParam;
            public int iIndent;
            public int iGroupId;
            public int cColumns;
            public IntPtr puColumns;

            public LVITEM(int stateMask, int state) : this()
            {
                this.stateMask = stateMask;
                this.state = state;
            }
        }

        /// <summary>
        /// Select all rows on the given listview
        /// </summary>
        /// <param name="list">The listview whose items are to be selected</param>
        public static void SelectAllItems(ListView list) => SetItemState(list, -1, 2, 2);

        /// <summary>
        /// Deselect all rows on the given listview
        /// </summary>
        /// <param name="list">The listview whose items are to be deselected</param>
        public static void DeselectAllItems(ListView list) => SetItemState(list, -1, 2, 0);

        /// <summary>
        /// Set the item state on the given item
        /// </summary>
        /// <param name="list">The listview whose item's state is to be changed</param>
        /// <param name="itemIndex">The index of the item to be changed</param>
        /// <param name="mask">Which bits of the value are to be set?</param>
        /// <param name="value">The value to be set</param>
        public static void SetItemState(ListView list, int itemIndex, int mask, int value)
        {
            LVITEM lvItem = new LVITEM(mask, value);
            SendMessage(list.Handle, LVM_SETITEMSTATE, itemIndex, ref lvItem);
        }

        /// <summary>
        /// Get all the SelectedItems of a ListView.
        /// </summary>
        /// <param name="listView">listView</param>
        /// <returns>SelectedItems</returns>
        public static List<ListViewItem> GetSelectedItems(ListView listView)
        {
            List<ListViewItem> selectedItems = new List<ListViewItem>();

            // Use ListView.SelectedIndices to get the indices of selected items.
            foreach (int selectedIndex in listView.SelectedIndices)
            {
                // Use VirtualListSize to check if the index is within the visible range.
                if (selectedIndex < 0 || selectedIndex >= listView.VirtualListSize) continue;
                // If it's within the visible range, then get the corresponding ListViewItem.
                ListViewItem item = listView.Items[selectedIndex];
                selectedItems.Add(item);
            }

            return selectedItems;
        }
    }
}
