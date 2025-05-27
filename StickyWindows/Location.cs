namespace StickyWindows
{
    internal class Location
    {
        internal double Left { get; set; }
        internal double Top { get; set; }

        internal Location(double left, double top)
        {
            Left = left;
            Top = top;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Location location)
            {
                return this.Left == location.Left && this.Top == location.Top;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}