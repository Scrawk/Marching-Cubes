using System;
using System.Collections;
using UnityEngine;

namespace ProceduralNoiseProject
{

    public enum VORONOI_DISTANCE { EUCLIDIAN, MANHATTAN, CHEBYSHEV };

    public enum VORONOI_COMBINATION { D0, D1_D0, D2_D0 };

    public class VoronoiNoise : Noise
    {

        public VORONOI_DISTANCE Distance { get; set; }

        public VORONOI_COMBINATION Combination { get; set; }

        private PermutationTable Perm { get; set; }

        public VoronoiNoise(int seed, float frequency, float amplitude = 1.0f)
        {

            Frequency = frequency;
            Amplitude = amplitude;
            Offset = Vector3.zero;

            Distance = VORONOI_DISTANCE.EUCLIDIAN;
            Combination = VORONOI_COMBINATION.D1_D0;

            Perm = new PermutationTable(1024, int.MaxValue, seed);

        }

        /// <summary>
        /// Update the seed.
        /// </summary>
        public override void UpdateSeed(int seed)
        {
            Perm.Build(seed);
        }

        /// <summary>
        /// Sample the noise in 1 dimension.
        /// </summary>
        public override float Sample1D(float x)
        {
            //The 0.75 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.75f;

            int lastRandom, numberFeaturePoints;
            float randomDiffX;
            float featurePointX;
            int cubeX;

            Vector3 distanceArray = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

            //1. Determine which cube the evaluation point is in
            int evalCubeX = (int)Mathf.Floor(x);

            for (int i = -1; i < 2; ++i)
            {
                cubeX = evalCubeX + i;

                //2. Generate a reproducible random number generator for the cube
                lastRandom = Perm[cubeX];

                //3. Determine how many feature points are in the cube
                numberFeaturePoints = ProbLookup(lastRandom * Perm.Inverse);

                //4. Randomly place the feature points in the cube
                for (int l = 0; l < numberFeaturePoints; ++l)
                {
                    lastRandom = Perm[lastRandom];
                    randomDiffX = lastRandom * Perm.Inverse;

                    lastRandom = Perm[lastRandom];

                    featurePointX = randomDiffX + cubeX;

                    //5. Find the feature point closest to the evaluation point. 
                    //This is done by inserting the distances to the feature points into a sorted list
                    distanceArray = Insert(distanceArray, Distance1(x, featurePointX));
                }

                //6. Check the neighboring cubes to ensure their are no closer evaluation points.
                // This is done by repeating steps 1 through 5 above for each neighboring cube
            }

            return Combine(distanceArray) * Amplitude;
        }

        /// <summary>
        /// Sample the noise in 2 dimensions.
        /// </summary>
        public override float Sample2D(float x, float y)
        {
            //The 0.75 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.75f;
            y = (y + Offset.y) * Frequency * 0.75f;

            int lastRandom, numberFeaturePoints;
            float randomDiffX, randomDiffY;
            float featurePointX, featurePointY;
            int cubeX, cubeY;

            Vector3 distanceArray = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

            //1. Determine which cube the evaluation point is in
            int evalCubeX = (int)Mathf.Floor(x);
            int evalCubeY = (int)Mathf.Floor(y);

            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    cubeX = evalCubeX + i;
                    cubeY = evalCubeY + j;

                    //2. Generate a reproducible random number generator for the cube
                    lastRandom = Perm[cubeX, cubeY];

                    //3. Determine how many feature points are in the cube
                    numberFeaturePoints = ProbLookup(lastRandom * Perm.Inverse);

                    //4. Randomly place the feature points in the cube
                    for (int l = 0; l < numberFeaturePoints; ++l)
                    {
                        lastRandom = Perm[lastRandom];
                        randomDiffX = lastRandom * Perm.Inverse;

                        lastRandom = Perm[lastRandom];
                        randomDiffY = lastRandom * Perm.Inverse;

                        featurePointX = randomDiffX + cubeX;
                        featurePointY = randomDiffY + cubeY;

                        //5. Find the feature point closest to the evaluation point. 
                        //This is done by inserting the distances to the feature points into a sorted list
                        distanceArray = Insert(distanceArray, Distance2(x, y, featurePointX, featurePointY));
                    }

                    //6. Check the neighboring cubes to ensure their are no closer evaluation points.
                    // This is done by repeating steps 1 through 5 above for each neighboring cube
                }
            }

            return Combine(distanceArray) * Amplitude;
        }

        /// <summary>
        /// Sample the noise in 3 dimensions.
        /// </summary>
        public override float Sample3D(float x, float y, float z)
        {
            //The 0.75 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.75f;
            y = (y + Offset.y) * Frequency * 0.75f;
            z = (z + Offset.z) * Frequency * 0.75f;

            int lastRandom, numberFeaturePoints;
            float randomDiffX, randomDiffY, randomDiffZ;
            float featurePointX, featurePointY, featurePointZ;
            int cubeX, cubeY, cubeZ;

            Vector3 distanceArray = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

            //1. Determine which cube the evaluation point is in
            int evalCubeX = (int)Mathf.Floor(x);
            int evalCubeY = (int)Mathf.Floor(y);
            int evalCubeZ = (int)Mathf.Floor(z);

            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    for (int k = -1; k < 2; ++k)
                    {
                        cubeX = evalCubeX + i;
                        cubeY = evalCubeY + j;
                        cubeZ = evalCubeZ + k;

                        //2. Generate a reproducible random number generator for the cube
                        lastRandom = Perm[cubeX, cubeY, cubeZ];

                        //3. Determine how many feature points are in the cube
                        numberFeaturePoints = ProbLookup(lastRandom * Perm.Inverse);

                        //4. Randomly place the feature points in the cube
                        for (int l = 0; l < numberFeaturePoints; ++l)
                        {
                            lastRandom = Perm[lastRandom];
                            randomDiffX = lastRandom * Perm.Inverse;

                            lastRandom = Perm[lastRandom];
                            randomDiffY = lastRandom * Perm.Inverse;

                            lastRandom = Perm[lastRandom];
                            randomDiffZ = lastRandom * Perm.Inverse;

                            featurePointX = randomDiffX + cubeX;
                            featurePointY = randomDiffY + cubeY;
                            featurePointZ = randomDiffZ + cubeZ;

                            //5. Find the feature point closest to the evaluation point. 
                            //This is done by inserting the distances to the feature points into a sorted list
                            distanceArray = Insert(distanceArray, Distance3(x, y, z, featurePointX, featurePointY, featurePointZ));
                        }

                        //6. Check the neighboring cubes to ensure their are no closer evaluation points.
                        // This is done by repeating steps 1 through 5 above for each neighboring cube
                    }
                }
            }

            return Combine(distanceArray) * Amplitude;
        }

        private float Distance1(float p1x, float p2x)
        {
            switch (Distance)
            {
                case VORONOI_DISTANCE.EUCLIDIAN:
                    return (p1x - p2x) * (p1x - p2x);

                case VORONOI_DISTANCE.MANHATTAN:
                    return Math.Abs(p1x - p2x);

                case VORONOI_DISTANCE.CHEBYSHEV:
                    return Math.Abs(p1x - p2x);
            }

            return 0;
        }

        private float Distance2(float p1x, float p1y, float p2x, float p2y)
        {
            switch(Distance)
            {
                case VORONOI_DISTANCE.EUCLIDIAN:
                    return (p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y);

                case VORONOI_DISTANCE.MANHATTAN:
                    return Math.Abs(p1x - p2x) + Math.Abs(p1y - p2y);

                case VORONOI_DISTANCE.CHEBYSHEV:
                    return Math.Max(Math.Abs(p1x - p2x), Math.Abs(p1y - p2y));
            }

            return 0;
        }

        private float Distance3(float p1x, float p1y, float p1z, float p2x, float p2y, float p2z)
        {
            switch (Distance)
            {
                case VORONOI_DISTANCE.EUCLIDIAN:
                    return (p1x - p2x) * (p1x - p2x) + (p1y - p2y) * (p1y - p2y) + (p1z - p2z) * (p1z - p2z);

                case VORONOI_DISTANCE.MANHATTAN:
                    return Math.Abs(p1x - p2x) + Math.Abs(p1y - p2y) + Math.Abs(p1z - p2z);

                case VORONOI_DISTANCE.CHEBYSHEV:
                    return Math.Max(Math.Max(Math.Abs(p1x - p2x), Math.Abs(p1y - p2y)), Math.Abs(p1z - p2z));
            }

            return 0;
        }

        private float Combine(Vector3 arr)
        {
            switch(Combination)
            {
                case VORONOI_COMBINATION.D0:
                    return arr[0];

                case VORONOI_COMBINATION.D1_D0:
                    return arr[1] - arr[0];

                case VORONOI_COMBINATION.D2_D0:
                    return arr[2] - arr[0];
            }

            return 0;
        }

        /// <summary>
        /// Given a uniformly distributed random number this function returns the number of feature points in a given cube.
        /// </summary>
        /// <param name="value">a uniformly distributed random number</param>
        /// <returns>The number of feature points in a cube.</returns>
        int ProbLookup(float value)
        {
            //Poisson Distribution
            if (value < 0.0915781944272058) return 1;
            if (value < 0.238103305510735) return 2;
            if (value < 0.433470120288774) return 3;
            if (value < 0.628836935299644) return 4;
            if (value < 0.785130387122075) return 5;
            if (value < 0.889326021747972) return 6;
            if (value < 0.948866384324819) return 7;
            if (value < 0.978636565613243) return 8;

            return 9;
        }

        /// <summary>
        /// Inserts value into array using insertion sort. If the value is greater than the largest value in the array
        /// it will not be added to the array.
        /// </summary>
        /// <param name="arr">The array to insert the value into.</param>
        /// <param name="value">The value to insert into the array.</param>
        Vector3 Insert(Vector3 arr, float value)
        {
            float temp;
            for (int i = 3 - 1; i >= 0; i--)
            {
                if (value > arr[i]) break;
                temp = arr[i];
                arr[i] = value;
                if (i + 1 < 3) arr[i + 1] = temp;
            }

            return arr;
        }

    }


}