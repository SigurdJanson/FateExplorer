using System;


namespace FateExplorer.RollLogic
{
    /// <summary>
    /// The Mersenne Twister is a pseudorandom number generator (PRNG). 
    /// It is by far the most widely used general-purpose PRNG (quote by Wikipedia).
    /// </summary>
    /// <remarks>As implemented by https://www.prowaretech.com/Computer/DotNet/Mersenne</remarks>
    public class RandomMersenne : IRandomNG
    {
        const int MERS_N = 624;
        const int MERS_M = 397;
        const int MERS_U = 11;
        const int MERS_S = 7;
        const int MERS_T = 15;
        const int MERS_L = 18;
        const uint MERS_A = 0x9908B0DF;
        const uint MERS_B = 0x9D2C5680;
        const uint MERS_C = 0xEFC60000;

        private uint[] mt = new uint[MERS_N];        // state vector
        private uint mti;                            // index into mt

        /// <summary>
        /// Constructor. Initialises the seed with <c>Guid.NewGuid().GetHashCode()</c>.
        /// </summary>
        public RandomMersenne()
        {
            int seed = Guid.NewGuid().GetHashCode(); // this should always generate a good number for the seed
            RandomInit((uint)seed);
        }

        /// <summary>
        /// Constructor. Initialises the seed with the given value.
        /// </summary>
        /// <param name="seed"></param>
        public RandomMersenne(uint seed)
        {   // constructor
            RandomInit(seed);
        }

        /// <inheritdoc/>
        public void RandomInit(uint seed)
        {
            mt[0] = seed;
            for (mti = 1; mti < MERS_N; mti++)
                mt[mti] = 1812433253U * (mt[mti - 1] ^ (mt[mti - 1] >> 30)) + mti;
        }

        /// <summary>
        /// Set a new seed for the random number generator using a byte array as seed..
        /// </summary>
        /// <param name="seeds">An array of unisigned integers.</param>
        public void RandomInitByArray(uint[] seeds)
        {
            // seed by more than 32 bits
            uint i, j;
            int k;
            int length = seeds.Length;
            RandomInit(19650218U);
            if (length <= 0) return;
            i = 1; j = 0;
            k = (MERS_N > length ? MERS_N : length);
            for (; k != 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1664525U)) + seeds[j] + j;
                i++; j++;
                if (i >= MERS_N) { mt[0] = mt[MERS_N - 1]; i = 1; }
                if (j >= length) j = 0;
            }
            for (k = MERS_N - 1; k != 0; k--)
            {
                mt[i] = (mt[i] ^ ((mt[i - 1] ^ (mt[i - 1] >> 30)) * 1566083941U)) - i;
                if (++i >= MERS_N) { mt[0] = mt[MERS_N - 1]; i = 1; }
            }
            mt[0] = 0x80000000U; // MSB is 1; assuring non-zero initial array
        }

        /// <inheritdoc/>
        public int IRandom(int min, int max)
        {
            if (max <= min) 
                throw new ArgumentException("max argument shall be greater than min", nameof(max));
            // output random integer in the interval min <= x <= max
            int r;
            r = (int)((max - min + 1) * Random()) + min; // multiply interval with random and truncate
            if (r > max)
                r = max;

            return r;
        }


        /// <inheritdoc/>
        public double Random()
        {
            // output random float number in the interval 0 <= x < 1
            uint r = BRandom(); // get 32 random bits
            if (BitConverter.IsLittleEndian)
            {
                byte[] i0 = BitConverter.GetBytes((r << 20));
                byte[] i1 = BitConverter.GetBytes(((r >> 12) | 0x3FF00000));
                byte[] bytes = { i0[0], i0[1], i0[2], i0[3], i1[0], i1[1], i1[2], i1[3] };
                double f = BitConverter.ToDouble(bytes, 0);
                return f - 1.0;
            }
            return r * (1.0 / (0xFFFFFFFF + 1.0));
        }

        /// <inheritdoc/>
        public uint BRandom()
        {
            // generate 32 random bits
            uint y;

            if (mti >= MERS_N)
            {
                const uint LOWER_MASK = 2147483647;
                const uint UPPER_MASK = 0x80000000;
                uint[] mag01 = { 0, MERS_A };

                int kk;
                for (kk = 0; kk < MERS_N - MERS_M; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + MERS_M] ^ (y >> 1) ^ mag01[y & 1];
                }

                for (; kk < MERS_N - 1; kk++)
                {
                    y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
                    mt[kk] = mt[kk + (MERS_M - MERS_N)] ^ (y >> 1) ^ mag01[y & 1];
                }

                y = (mt[MERS_N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
                mt[MERS_N - 1] = mt[MERS_M - 1] ^ (y >> 1) ^ mag01[y & 1];
                mti = 0;
            }

            y = mt[mti++];

            // Tempering (May be omitted):
            y ^= y >> MERS_U;
            y ^= (y << MERS_S) & MERS_B;
            y ^= (y << MERS_T) & MERS_C;
            y ^= y >> MERS_L;
            return y;
        }
    }
}
