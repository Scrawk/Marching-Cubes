using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MarchingCubesProject
{
    public interface IMarching
    {

        float Surface { get; set; }

        void Generate(IList<float> voxels, int width, int height, int depth, IList<Vector3> verts, IList<int> indices);

    }

}