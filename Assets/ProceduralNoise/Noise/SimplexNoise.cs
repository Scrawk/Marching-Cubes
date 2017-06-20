using System;
using UnityEngine;

namespace ProceduralNoiseProject
{
    /// <summary>
    /// Implementation of the Perlin simplex noise, an improved Perlin noise algorithm.
    /// Based loosely on SimplexNoise1234 by Stefan Gustavson 
    /// <http://staffwww.itn.liu.se/~stegu/aqsis/aqsis-newnoise/>
    /// </summary>
	public class SimplexNoise : Noise
    {

        private PermutationTable Perm { get; set; }

        /// <summary>
        /// Create a simplex noise object.
        /// </summary>
        public SimplexNoise(int seed, float frequency, float amplitude = 1.0f)
        {

            Frequency = frequency;
            Amplitude = amplitude;
            Offset = Vector3.zero;

            Perm = new PermutationTable(1024, 255, seed);
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
            //The 0.5 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.5f;

            int i0 = (int)Mathf.Floor(x);
            int i1 = i0 + 1;
            float x0 = x - i0;
            float x1 = x0 - 1.0f;

            float n0, n1;

            float t0 = 1.0f - x0*x0;
            t0 *= t0;
			n0 = t0 * t0 * Grad(Perm[i0], x0);

            float t1 = 1.0f - x1*x1;
            t1 *= t1;
			n1 = t1 * t1 * Grad(Perm[i1], x1);

            // The maximum value of this noise is 8*(3/4)^4 = 2.53125
            // A factor of 0.395 scales to fit exactly within [-1,1]
            return 0.395f * (n0 + n1) * Amplitude;
        }

		/// <summary>
		/// Sample the noise in 2 dimensions.
		/// </summary>
		public override float Sample2D(float x, float y)
        {
            //The 0.5 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.5f;
            y = (y + Offset.y) * Frequency * 0.5f;

            const float F2 = 0.366025403f; // F2 = 0.5*(sqrt(3.0)-1.0)
            const float G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

            float n0, n1, n2; // Noise contributions from the three corners

            // Skew the input space to determine which simplex cell we're in
            float s = (x+y)*F2; // Hairy factor for 2D
            float xs = x + s;
            float ys = y + s;
            int i = (int)Mathf.Floor(xs);
            int j = (int)Mathf.Floor(ys);

            float t = (i+j)*G2;
            float X0 = i-t; // Unskew the cell origin back to (x,y) space
            float Y0 = j-t;
            float x0 = x-X0; // The x,y distances from the cell origin
            float y0 = y-Y0;

            // For the 2D case, the simplex shape is an equilateral triangle.
            // Determine which simplex we are in.
            int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
            if(x0>y0) {i1=1; j1=0;} // lower triangle, XY order: (0,0)->(1,0)->(1,1)
            else {i1=0; j1=1;}      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

            // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
            // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
            // c = (3-sqrt(3))/6

            float x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
            float y1 = y0 - j1 + G2;
            float x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
            float y2 = y0 - 1.0f + 2.0f * G2;

            // Calculate the contribution from the three corners
            float t0 = 0.5f - x0*x0-y0*y0;
            if(t0 < 0.0) n0 = 0.0f;
            else {
                t0 *= t0;
				n0 = t0 * t0 * Grad(Perm[i, j], x0, y0); 
            }

            float t1 = 0.5f - x1*x1-y1*y1;
            if(t1 < 0.0) n1 = 0.0f;
            else {
                t1 *= t1;
				n1 = t1 * t1 * Grad(Perm[i+i1, j+j1], x1, y1);
            }

            float t2 = 0.5f - x2*x2-y2*y2;
            if(t2 < 0.0) n2 = 0.0f;
            else {
                t2 *= t2;
				n2 = t2 * t2 * Grad(Perm[i+1, j+1], x2, y2);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to return values in the interval [-1,1].
            return 40.0f * (n0 + n1 + n2) * Amplitude; 
        }

		/// <summary>
		/// Sample the noise in 3 dimensions.
		/// </summary>
        public override float Sample3D(float x, float y, float z)
        {
            //The 0.5 is to make the scale simliar to the other noise algorithms
            x = (x + Offset.x) * Frequency * 0.5f;
            y = (y + Offset.y) * Frequency * 0.5f;
            z = (z + Offset.z) * Frequency * 0.5f;

            // Simple skewing factors for the 3D case
            const float F3 = 0.333333333f;
            const float G3 = 0.166666667f;

            float n0, n1, n2, n3; // Noise contributions from the four corners

            // Skew the input space to determine which simplex cell we're in
            float s = (x+y+z)*F3; // Very nice and simple skew factor for 3D
            float xs = x+s;
            float ys = y+s;
            float zs = z+s;
            int i = (int)Mathf.Floor(xs);
            int j = (int)Mathf.Floor(ys);
            int k = (int)Mathf.Floor(zs);

            float t = (i+j+k)*G3; 
            float X0 = i-t; // Unskew the cell origin back to (x,y,z) space
            float Y0 = j-t;
            float Z0 = k-t;
            float x0 = x-X0; // The x,y,z distances from the cell origin
            float y0 = y-Y0;
            float z0 = z-Z0;

            // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
            // Determine which simplex we are in.
            int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
            int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

            /* This code would benefit from a backport from the GLSL version! */
            if(x0>=y0) {
                if(y0>=z0)
                { i1=1; j1=0; k1=0; i2=1; j2=1; k2=0; } // X Y Z order
                else if(x0>=z0) { i1=1; j1=0; k1=0; i2=1; j2=0; k2=1; } // X Z Y order
                else { i1=0; j1=0; k1=1; i2=1; j2=0; k2=1; } // Z X Y order
                }
            else { // x0<y0
                if(y0<z0) { i1=0; j1=0; k1=1; i2=0; j2=1; k2=1; } // Z Y X order
                else if(x0<z0) { i1=0; j1=1; k1=0; i2=0; j2=1; k2=1; } // Y Z X order
                else { i1=0; j1=1; k1=0; i2=1; j2=1; k2=0; } // Y X Z order
            }

            // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
            // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
            // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
            // c = 1/6.

            float x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
            float y1 = y0 - j1 + G3;
            float z1 = z0 - k1 + G3;
            float x2 = x0 - i2 + 2.0f*G3; // Offsets for third corner in (x,y,z) coords
            float y2 = y0 - j2 + 2.0f*G3;
            float z2 = z0 - k2 + 2.0f*G3;
            float x3 = x0 - 1.0f + 3.0f*G3; // Offsets for last corner in (x,y,z) coords
            float y3 = y0 - 1.0f + 3.0f*G3;
            float z3 = z0 - 1.0f + 3.0f*G3;

            // Calculate the contribution from the four corners
            float t0 = 0.6f - x0*x0 - y0*y0 - z0*z0;
            if(t0 < 0.0) n0 = 0.0f;
            else {
                t0 *= t0;
				n0 = t0 * t0 * Grad(Perm[i, j, k], x0, y0, z0);
            }

            float t1 = 0.6f - x1*x1 - y1*y1 - z1*z1;
            if(t1 < 0.0) n1 = 0.0f;
            else {
                t1 *= t1;
				n1 = t1 * t1 * Grad(Perm[i+i1, j+j1, k+k1], x1, y1, z1);
            }

            float t2 = 0.6f - x2*x2 - y2*y2 - z2*z2;
            if(t2 < 0.0) n2 = 0.0f;
            else {
                t2 *= t2;
				n2 = t2 * t2 * Grad(Perm[i+i2, j+j2, k+k2], x2, y2, z2);
            }

            float t3 = 0.6f - x3*x3 - y3*y3 - z3*z3;
            if(t3<0.0) n3 = 0.0f;
            else {
                t3 *= t3;
				n3 = t3 * t3 * Grad(Perm[i+1, j+1, k+1], x3, y3, z3);
            }

            // Add contributions from each corner to get the final noise value.
            // The result is scaled to stay just inside [-1,1]
            return 32.0f * (n0 + n1 + n2 + n3) * Amplitude;
        }

        private float Grad(int hash, float x)
        {
            int h = hash & 15;
            float grad = 1.0f + (h & 7);   // Gradient value 1.0, 2.0, ..., 8.0
            if ((h & 8) != 0) grad = -grad;// Set a random sign for the gradient
            return (grad * x);           // Multiply the gradient with the distance
        }

        private float Grad(int hash, float x, float y)
        {
            int h = hash & 7;           // Convert low 3 bits of hash code
            float u = h < 4 ? x : y;     // into 8 simple gradient directions,
            float v = h < 4 ? y : x;     // and compute the dot product with (x,y).
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0f * v : 2.0f * v);
        }

        private float Grad(int hash, float x, float y, float z)
        {
            int h = hash & 15;      // Convert low 4 bits of hash code into 12 simple
            float u = h < 8 ? x : y; // gradient directions, and compute dot product.
            float v = h < 4 ? y : h == 12 || h == 14 ? x : z; // Fix repeats at h = 12 to 15
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v);
        }

        private float Grad(int hash, float x, float y, float z, float t)
        {
            int h = hash & 31;          // Convert low 5 bits of hash code into 32 simple
            float u = h < 24 ? x : y;    // gradient directions, and compute dot product.
            float v = h < 16 ? y : z;
            float w = h < 8 ? z : t;
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v) + ((h & 4) != 0 ? -w : w);
        }

    }
}




