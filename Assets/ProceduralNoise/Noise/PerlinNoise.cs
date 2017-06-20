using System;
using UnityEngine;

namespace ProceduralNoiseProject
{

	public class PerlinNoise : Noise
	{

        private PermutationTable Perm { get; set; }

        public PerlinNoise(int seed, float frequency, float amplitude = 1.0f)
		{

            Frequency = frequency;
            Amplitude = amplitude;
            Offset = Vector3.zero;

            Perm = new PermutationTable(1024, 255, seed);
		}

        public override void UpdateSeed(int seed)
        {
            Perm.Build(seed);
        }
		
        /// <summary>
        /// Sample the noise in 1 dimension.
        /// </summary>
		public override float Sample1D( float x )
		{
            x = (x + Offset.x) * Frequency;

		    int ix0;
		    float fx0, fx1;
		    float s, n0, n1;
		
		    ix0 = (int)Mathf.Floor(x); 	// Integer part of x
		    fx0 = x - ix0;              // Fractional part of x
		    fx1 = fx0 - 1.0f;
			
		    s = FADE(fx0);
		
		    n0 = Grad(Perm[ix0], fx0);
		    n1 = Grad(Perm[ix0 + 1], fx1);

            return 0.25f * LERP(s, n0, n1) * Amplitude;
		}
		
        /// <summary>
        /// Sample the noise in 2 dimensions.
        /// </summary>
		public override float Sample2D( float x, float y )
		{
            x = (x + Offset.x) * Frequency;
            y = (y + Offset.y) * Frequency;

		    int ix0, iy0;
		    float fx0, fy0, fx1, fy1, s, t, nx0, nx1, n0, n1;
		
			ix0 = (int)Mathf.Floor(x); 		// Integer part of x
			iy0 = (int)Mathf.Floor(y); 		// Integer part of y

		    fx0 = x - ix0;        		// Fractional part of x
		    fy0 = y - iy0;        		// Fractional part of y
		    fx1 = fx0 - 1.0f;
		    fy1 = fy0 - 1.0f;
		    
		    t = FADE( fy0 );
		    s = FADE( fx0 );

			nx0 = Grad(Perm[ix0, iy0], fx0, fy0);
            nx1 = Grad(Perm[ix0, iy0 + 1], fx0, fy1);

		    n0 = LERP( t, nx0, nx1 );

		    nx0 = Grad(Perm[ix0 + 1, iy0], fx1, fy0);
		    nx1 = Grad(Perm[ix0 + 1, iy0 + 1], fx1, fy1);

		    n1 = LERP(t, nx0, nx1);

            return 0.66666f * LERP(s, n0, n1) * Amplitude;
		}
		
        /// <summary>
        /// Sample the noise in 3 dimensions.
        /// </summary>
		public override float Sample3D( float x, float y, float z )
		{
            x = (x + Offset.x) * Frequency;
            y = (y + Offset.y) * Frequency;
            z = (z + Offset.z) * Frequency;

            int ix0, iy0, iz0;
		    float fx0, fy0, fz0, fx1, fy1, fz1;
		    float s, t, r;
		    float nxy0, nxy1, nx0, nx1, n0, n1;
		
			ix0 = (int)Mathf.Floor(x);   		// Integer part of x
			iy0 = (int)Mathf.Floor(y);   		// Integer part of y
			iz0 = (int)Mathf.Floor(z);   		// Integer part of z
		    fx0 = x - ix0;        		        // Fractional part of x
		    fy0 = y - iy0;        		        // Fractional part of y
		    fz0 = z - iz0;        		        // Fractional part of z
		    fx1 = fx0 - 1.0f;
		    fy1 = fy0 - 1.0f;
		    fz1 = fz0 - 1.0f;
		    
		    r = FADE( fz0 );
		    t = FADE( fy0 );
		    s = FADE( fx0 );
		
			nxy0 = Grad(Perm[ix0, iy0, iz0], fx0, fy0, fz0);
		    nxy1 = Grad(Perm[ix0, iy0, iz0 + 1], fx0, fy0, fz1);
		    nx0 = LERP( r, nxy0, nxy1 );
		
		    nxy0 = Grad(Perm[ix0, iy0 + 1, iz0], fx0, fy1, fz0);
		    nxy1 = Grad(Perm[ix0, iy0 + 1, iz0 + 1], fx0, fy1, fz1);
		    nx1 = LERP( r, nxy0, nxy1 );
		
		    n0 = LERP( t, nx0, nx1 );
		
		    nxy0 = Grad(Perm[ix0 + 1, iy0, iz0], fx1, fy0, fz0);
		    nxy1 = Grad(Perm[ix0 + 1, iy0, iz0 + 1], fx1, fy0, fz1);
		    nx0 = LERP( r, nxy0, nxy1 );
		
		    nxy0 = Grad(Perm[ix0 + 1, iy0 + 1, iz0], fx1, fy1, fz0);
		   	nxy1 = Grad(Perm[ix0 + 1, iy0 + 1, iz0 + 1], fx1, fy1, fz1);
		    nx1 = LERP( r, nxy0, nxy1 );
		
		    n1 = LERP( t, nx0, nx1 );

            return 1.1111f * LERP(s, n0, n1) * Amplitude;
		}

        private float FADE(float t) { return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f); }

        private float LERP(float t, float a, float b) { return a + t * (b - a); }

        private float Grad(int hash, float x)
        {
            int h = hash & 15;
            float grad = 1.0f + (h & 7);    // Gradient value 1.0, 2.0, ..., 8.0
            if ((h & 8) != 0) grad = -grad; // Set a random sign for the gradient
            return (grad * x);              // Multiply the gradient with the distance
        }

        private float Grad(int hash, float x, float y)
        {
            int h = hash & 7;           // Convert low 3 bits of hash code
            float u = h < 4 ? x : y;  // into 8 simple gradient directions,
            float v = h < 4 ? y : x;  // and compute the dot product with (x,y).
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0f * v : 2.0f * v);
        }

        private float Grad(int hash, float x, float y, float z)
        {
            int h = hash & 15;     // Convert low 4 bits of hash code into 12 simple
            float u = h < 8 ? x : y; // gradient directions, and compute dot product.
            float v = h < 4 ? y : h == 12 || h == 14 ? x : z; // Fix repeats at h = 12 to 15
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v);
        }

        private float Grad(int hash, float x, float y, float z, float t)
        {
            int h = hash & 31;          // Convert low 5 bits of hash code into 32 simple
            float u = h < 24 ? x : y; // gradient directions, and compute dot product.
            float v = h < 16 ? y : z;
            float w = h < 8 ? z : t;
            return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v) + ((h & 4) != 0 ? -w : w);
        }


    }

}













