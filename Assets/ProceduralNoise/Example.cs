using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralNoiseProject
{

    public enum NOISE_TYPE {  PERLIN, VALUE, SIMPLEX, VORONOI, WORLEY }

    public class Example : MonoBehaviour
    {

        public NOISE_TYPE noiseType = NOISE_TYPE.PERLIN;

        public int seed = 0;

        public int octaves = 4;

        public float frequency = 1.0f;

        public int width = 512;

        public int height = 512;

        Texture2D texture;

        void Start()
        {

            texture = new Texture2D(width, height);

            //Create the noise object and use a fractal to apply it.
            //The same noise object will be used for each fractal octave but you can 
            //manually set each individual ocatve like so...
            // fractal.Noises[3] = noise;
            INoise noise = GetNoise();
            FractalNoise fractal = new FractalNoise(noise, octaves, frequency);

            float[,] arr = new float[width, height];

            //Sample the 2D noise and add it into a array.
            for(int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float fx = x / (width - 1.0f);
                    float fy = y / (height - 1.0f);

                    arr[x,y] = fractal.Sample2D(fx, fy);
                }
            }

            //Some of the noises range from -1-1 so normalize the data to 0-1 to make it easier to see.
            NormalizeArray(arr);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float n = arr[x, y];
                    texture.SetPixel(x, y, new Color(n, n, n, 1));
                }
            }

            texture.Apply();

        }

        void OnGUI()
        {

            Vector2 center = new Vector2(Screen.width / 2, Screen.height/2);
            Vector2 offset = new Vector2(width / 2, height / 2);

            Rect rect = new Rect();
            rect.min = center - offset;
            rect.max = center + offset;

            GUI.DrawTexture(rect, texture);

        }

        private INoise GetNoise()
        {
            switch (noiseType)
            {
                case NOISE_TYPE.PERLIN:
                    return new PerlinNoise(seed, 20);

                case NOISE_TYPE.VALUE:
                    return new ValueNoise(seed, 20);

                case NOISE_TYPE.SIMPLEX:
                    return new SimplexNoise(seed, 20);

                case NOISE_TYPE.VORONOI:
                    return new VoronoiNoise(seed, 20);

                case NOISE_TYPE.WORLEY:
                    return new WorleyNoise(seed, 20, 1.0f);

                default:
                    return new PerlinNoise(seed, 20);
            }
        }

        private void NormalizeArray(float[,] arr)
        {

            float min = float.PositiveInfinity;
            float max = float.NegativeInfinity;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    float v = arr[x, y];
                    if (v < min) min = v;
                    if (v > max) max = v;

                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float v = arr[x, y];
                    arr[x, y] = (v - min) / (max - min);
                }
            }

        }

    }

}
