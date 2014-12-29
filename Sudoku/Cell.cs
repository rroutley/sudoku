using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    [Serializable]
    public class Cell
    {
        public const int Empty = 0;
        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get { return 3 * (X / 3) + (Y / 3); } }

        public int Value { get; set; }
        public HashSet<int> Candidates { get; set; }
        public bool IsNew { get; set; }

        public override string ToString()
        {
            if (Value != Empty)
            {
                return string.Format("({0},{1})={2}", X, Y, Value);
            }
            else
            {
                return string.Format("({0},{1})=[{2}]", X, Y, string.Join(",", Candidates));
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as Cell;
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() * 31 + Y.GetHashCode();
        }

        public static bool operator ==(Cell left, Cell right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Cell left, Cell right)
        {
            return !left.Equals(right);
        }

        internal void RemoveCandidate(int value)
        {
            this.Candidates.Remove(value);
            if (this.Candidates.Count == 0) throw new Exception();
        }
    }
}

