using System;
using System.Collections.Generic;

namespace ProceduralNoiseProject
{
    internal class PermutationTable
    {

        public int Size { get; private set; }

        public int Seed { get; private set; }

        public int Max { get; private set; }

        public float Inverse { get; private set; }

        private int Wrap;

        private int[] Table;

        internal PermutationTable(int size, int max, int seed)
        {
            Size = size;
            Wrap = Size - 1;
            Max = Math.Max(1, max);
            Inverse = 1.0f / Max;
            Build(seed);
        }

        internal void Build(int seed)
        {
            if (Seed == seed && Table != null) return;

            Seed = seed;
            Table = new int[Size];

            System.Random rnd = new System.Random(Seed);

            for(int i = 0; i < Size; i++)
            {
                Table[i] = rnd.Next();
            }
        }

        internal int this[int i]
        {
            get
            {
                return Table[i & Wrap] & Max;
            }
        }

        internal int this[int i, int j]
        {
            get
            {
                return Table[(j + Table[i & Wrap]) & Wrap] & Max;
            }
        }

        internal int this[int i, int j, int k]
        {
            get
            {
                return Table[(k + Table[(j + Table[i & Wrap]) & Wrap]) & Wrap] & Max;
            }
        }

    }
}
