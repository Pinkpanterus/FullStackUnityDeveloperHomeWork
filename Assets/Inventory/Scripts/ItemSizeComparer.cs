using System.Collections.Generic;

namespace Inventories
{
    public class SizeComparer : IComparer<Item>
    {
        public int Compare(Item x, Item y)
        {
            return x.Size.x * x.Size.y.CompareTo(y.Size.x * y.Size.y);
        }
    }
}
