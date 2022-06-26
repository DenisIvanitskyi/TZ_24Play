
using UnityEngine;

namespace Assets.Common.Base
{
    public class PerlinNoise
    {
        private float[,] noiseData;

        public PerlinNoise(int mapW, int mapH, int seed = 0, float spawnScale = 1)
        {
            if (seed == 0)
            {
                seed = Random.Range(0, 999999);
            }
            noiseData = new float[mapW, mapH];
            for (int i = 0; i < noiseData.GetLength(0); i++)
            {
                for (int j = 0; j < noiseData.GetLength(1); j++)
                {
                    float xCoord = (float)i / (float)mapW * spawnScale + seed;
                    float yCoord = (float)j / (float)mapH * spawnScale + seed;
                    noiseData[i, j] = Mathf.PerlinNoise(xCoord, yCoord);
                }
            }
        }

        public float NoiseValueAt(Vector2Int pos)
        {
            return noiseData[pos.x, pos.y];
        }

        public float NoiseValueAt(int w, int h)
        {
            return noiseData[w, h];
        }
    }
}
