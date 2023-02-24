using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;

namespace WebApplication4
{
    public class fft
    {
        public Complex [] wektor;
        public Complex[] danna;
        public Complex[] buffer;
        public fft(Complex[] zm)
        {
            wektor = zm;
                danna = new Complex[8];
            danna[0] = zm[0];
            danna[1] = zm[1];
            danna[2] = zm[2];
            danna[3] = zm[3];
            danna[4] = zm[4];
            danna[5] = zm[5];
            danna[6] = zm[6];
            danna[7] = zm[7];
        }
        public int BitReverse(int n, int bits)
        {
            int reversedN = n;
            int count = bits - 1;

            n >>= 1;
            while (n > 0)
            {
                reversedN = (reversedN << 1) | (n & 1);
                count--;
                n >>= 1;
            }

            return ((reversedN << count) & ((1 << bits) - 1));
        }

        public void FFT()
        {

           buffer = wektor;

#if false
        int bits = (int)Math.Log(buffer.Length, 2);
        for (int j = 1; j < buffer.Length / 2; j++) {

            int swapPos = BitReverse(j, bits);
            var temp = buffer[j];
            buffer[j] = buffer[swapPos];
            buffer[swapPos] = temp;
        }
// Said Zandian
// The above section of the code is incorrect and does not work correctly and has two bugs.
// BUG 1
// The bug is that when you reach and index that was swapped previously it does swap it again
// Ex. binary value n = 0010 and Bits = 4 as input to BitReverse routine and  returns 4. The code section above //     swaps it. Cells 2 and 4 are swapped. just fine.
//     now binary value n = 0010 and Bits = 4 as input to BitReverse routine and returns 2. The code Section
//     swap it. Cells 4 and 2 are swapped.     WROOOOONG
// 
// Bug 2
// The code works on the half section of the cells. In the case of Bits = 4 it means that we are having 16 cells
// The code works on half the cells        for (int j = 1; j < buffer.Length / 2; j++) buffer.Length returns 16
// and divide by 2 makes 8, so j goes from 1 to 7. This covers almost everything but what happened to 1011 value
// which must be swap with 1101. and this is the second bug.
// 
// use the following corrected section of the code. I have seen this bug in other languages that uses bit
// reversal routine. 

#else
            int bits = (int)Math.Log(buffer.Length, 2);
            for (int j = 1; j < buffer.Length; j++)
            {
                int swapPos = BitReverse(j, bits);
                if (swapPos <= j)
                {
                    continue;
                }
                var temp = buffer[j];
                buffer[j] = buffer[swapPos];
                buffer[swapPos] = temp;
            }

            // First the full length is used and 1011 value is swapped with 1101. Second if new swapPos is less than j
            // then it means that swap was happen when j was the swapPos.

#endif

            for (int N = 2; N <= buffer.Length; N <<= 1)
            {
                for (int i = 0; i < buffer.Length; i += N)
                {
                    for (int k = 0; k < N / 2; k++)
                    {

                        int evenIndex = i + k;
                        int oddIndex = i + k + (N / 2);
                        var even = buffer[evenIndex];
                        var odd = buffer[oddIndex];

                        double term = -2 * Math.PI * k / (double)N;
                        Complex exp = new Complex(Math.Cos(term), Math.Sin(term)) * odd;

                        buffer[evenIndex] = even + exp;
                        buffer[oddIndex] = even - exp;

                    }
                }
            }
        }

    }
}
