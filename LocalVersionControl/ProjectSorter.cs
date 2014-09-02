using System.Collections;
using System.Windows.Forms;

namespace LocalVersionControl
{
    /// <summary>
    /// sorts the projects list
    /// </summary>
public class projectSorter : IComparer
{
	private int currentColumn;
    private SortOrder order;
	private Comparer ObjectCompare;

    /// <summary>
    /// set the defaults
    /// </summary>
    public projectSorter()
	{
        currentColumn = 0;
        order = SortOrder.Ascending;
		ObjectCompare = Comparer.Default;
	}

    /// <summary>
    /// compares the two listitem
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
	public int Compare(object x, object y)
	{	
        ListViewItem item1 = (ListViewItem)x;
		ListViewItem item2 = (ListViewItem)y;
        if (item1.SubItems.Count <= currentColumn)
        {
            return 1;
        }
        if (item2.SubItems.Count <= currentColumn)
        {
            return -1;
        }
        int compareResult = ObjectCompare.Compare(item1.SubItems[currentColumn].Text, item2.SubItems[currentColumn].Text);
        //com
        if (order == SortOrder.Ascending)
		{			
			return compareResult;
		}
        else if (order == SortOrder.Descending)
		{
			return (-compareResult);
		}
		else
		{
			return 0;
		}
	}

    /// <summary>
    /// gets and sets the column to sort by
    /// </summary>
	public int SortColumn
	{
		set
		{
            currentColumn = value;
		}
		get
		{
            return currentColumn;
		}
	}

    /// <summary>
    /// gets and sets the order to sort by
    /// </summary>
	public SortOrder Order
	{
		set
		{
            order = value;
		}
		get
		{
            return order;
		}
	}
  }
}
