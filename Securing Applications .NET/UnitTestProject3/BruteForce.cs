using System;
using System.Collections;
using System.Text;

namespace UnitTestProject3
{
    class BruteForce : IEnumerable
    {
        private StringBuilder sb = new StringBuilder();
        //the string we want to permutate 
        public string charset = "abcdefghijklmnopqrstuvwxyz";
        private ulong len;
        private int _max;
        public int Max { get { return _max; } set { _max = value; } }
        private int _min;
        public int Min { get { return _min; } set { _min = value; } }

        public System.Collections.IEnumerator GetEnumerator()
        {
            len = (ulong)this.charset.Length;
            for (double x = Min; x <= Max; x++)
            {
                ulong total = (ulong)Math.Pow((double)charset.Length, (double)x);
                ulong counter = 0;
                while (counter < total)
                {
                    string a = factoradic(counter, x - 1);
                    yield return a;
                    counter++;
                }
            }
        }
        private string factoradic(ulong l, double power)
        {
            sb.Length = 0;
            while (power >= 0)
            {
                sb = sb.Append(this.charset[(int)(l % len)]);
                l /= len;
                power--;
            }
            return sb.ToString();
        }
    }
}
