using System;
using System.Collections;
using UnityEngine;

namespace ProceduralNoiseProject
{
    /// <summary>
    /// Interface for generating noise.
    /// </summary>
	public interface INoise 
	{

        /// <summary>
        /// The frequency of the fractal.
        /// </summary>
        float Frequency { get; set; }

        /// <summary>
        /// The amplitude of the fractal.
        /// </summary>
        float Amplitude { get; set; }

        /// <summary>
        /// The offset applied to each dimension.
        /// </summary>
        Vector3 Offset { get; set; }

        /// <summary>
        /// Sample the noise in 1 dimension.
        /// </summary>
		float Sample1D(float x);

        /// <summary>
        /// Sample the noise in 2 dimensions.
        /// </summary>
		float Sample2D(float x, float y);

        /// <summary>
        /// Sample the noise in 3 dimensions.
        /// </summary>
		float Sample3D(float x, float y, float z);

        /// <summary>
        /// Update the seed.
        /// </summary>
        void UpdateSeed(int seed);

	}

}
