using System.Collections;
using System.Windows.Forms;

namespace PS4CheaterNeo
{
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface for ListView.
    /// https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/sort-listview-by-column
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn { set; get; }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order { set; get; }

        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor. Initializes various elements
        /// </summary>
        public ListViewColumnSorter(int sortColumn = 0, SortOrder order = SortOrder.None)
        {
            SortColumn = sortColumn;                    // Initialize the column to '0'
            Order = order;                         // Initialize the sort order to 'none'
            ObjectCompare = new CaseInsensitiveComparer(); // Initialize the CaseInsensitiveComparer object
        }

        /// <summary>
        /// This method is inherited from the IComparer interface. It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Compare the two items
            compareResult = ObjectCompare.Compare(
                listviewX.SubItems.Count > SortColumn ? listviewX.SubItems[SortColumn].Text : "",
                listviewY.SubItems.Count > SortColumn ? listviewY.SubItems[SortColumn].Text : "");

            // Calculate correct return value based on object comparison
            if (Order == SortOrder.Ascending) return compareResult; //Ascending sort is selected, return normal result of compare operation
            else if (Order == SortOrder.Descending) return (-compareResult); //Descending sort is selected, return negative result of compare operation
            else return 0; //Return '0' to indicate they are equal
        }
    }
}
