using System.Collections.Generic;

namespace Inventories
{
    public class SizeComparer : IComparer<Item>
    {
        public int Compare(Item x, Item y)
        {
            return (y.Size.x * y.Size.y).CompareTo((x.Size.x * x.Size.y));
        }
    }
}
