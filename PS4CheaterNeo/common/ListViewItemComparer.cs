using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    /// <summary>
    /// Sort VirtualMode ListView items
    /// https://stackoverflow.com/a/39505452
    /// </summary>
    class ListViewItemComparer : IComparer<ListViewItem>
    {
        public LinkedList<int> SortColumns { get; set; }
        public SortOrder Order { get; set; }
        public ListViewItemComparer() : this(new LinkedList<int>(), SortOrder.Ascending) { }
        public ListViewItemComparer(LinkedList<int> sortColumns) : this(sortColumns, SortOrder.Ascending) { }

        public ListViewItemComparer(LinkedList<int> sortColumns, SortOrder order)
        {
            if (sortColumns == null) sortColumns = new LinkedList<int>();
            if (sortColumns.Count == 0) sortColumns.AddFirst(0);
            SortColumns = sortColumns;
            Order = order;
        }

        public int Compare(ListViewItem x, ListViewItem y)
        {
            int result = -1;
            foreach (int mColonna in SortColumns)
            {

                String mStr1 = "";
                String mStr2 = "";

                if ((x.SubItems[mColonna].Text == "NULL") && (x.SubItems[mColonna].ForeColor == Color.Red)) mStr1 = "-1";
                else mStr1 = x.SubItems[mColonna].Text;

                if ((y.SubItems[mColonna].Text == "NULL") && (y.SubItems[mColonna].ForeColor == Color.Red)) mStr2 = "-1";
                else mStr2 = y.SubItems[mColonna].Text;


                if ((double.TryParse(mStr1, out double mNum1) == true) && (double.TryParse(mStr2, out double mNum2) == true))
                {
                    if (mNum1 == mNum2) result = 0;
                    else if (mNum1 > mNum2) result = 1;
                    else result = -1;
                }
                else if ((double.TryParse(mStr1, out mNum1) == true) && (double.TryParse(mStr2, out mNum2) == false)) result = -1;
                else if ((double.TryParse(mStr1, out mNum1) == false) && (double.TryParse(mStr2, out mNum2) == true)) result = 1;
                else result = String.Compare(mStr1, mStr2);

                if (result != 0) break;
            }

            // Determine whether the sort order is descending.
            if (Order == SortOrder.Descending) result *= -1; // Invert the value returned by String.Compare.
            return result;
        }
    }
}
