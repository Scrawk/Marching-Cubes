using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Unity.Drawing
{

    public enum LINE_MODE { LINES, TRIANGLES, QUADS, TETRAHEDRON, PENTAGONS, HEXAGONS };

    public class RenderPrimative
    {
        public int[] indices;
        public LINE_MODE mode;

        public override string ToString()
        {
            return string.Format("[RenderPrimative: Indices Count={0}, Mode={1}]",
                indices != null ? indices.Length : 0, mode);
        }
    }

    public class SegmentRenderer : BaseRenderer
    {

        public SegmentRenderer()
        {
            Orientation = DRAW_ORIENTATION.XY;
            LineModes = new List<LINE_MODE>();
            Primatives = new List<RenderPrimative>();
        }

        public SegmentRenderer(DRAW_ORIENTATION orientation)
        {
            Orientation = orientation;
            LineModes = new List<LINE_MODE>();
            Primatives = new List<RenderPrimative>();
        }

        private List<LINE_MODE> LineModes { get; set; }

        private List<RenderPrimative> Primatives { get; set; }

        public override void Clear()
        {
            base.Clear();
            LineModes.Clear();
            Primatives.Clear();
        }

        private void AddPrimative(RenderPrimative prim)
        {
            if(prim == null)
                throw new Exception("Prim is null");

            Primatives.Add(prim);
        }

        public void Load(IList<Vector2> vertices)
        {
            Load(vertices, null, LINE_MODE.LINES);
        }

        public void Load(IList<Vector2> vertices, IList<int> indices, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector2> vertices, IList<Color> colors, IList<int> indices = null, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
 
            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector2> vertices, Color color, IList<int> indices = null, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;

            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(color);
            }
        }

        public void Load(Vector2 a, Vector2 b, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(2, null,  primative);
            Primatives.Add(primative);

            if (Orientation == DRAW_ORIENTATION.XY)
            {
                Vertices.Add(a);
                Vertices.Add(b);
            }
            else if (Orientation == DRAW_ORIENTATION.XZ)
            {
                Vertices.Add(new Vector4(a.x, 0, a.y, 1));
                Vertices.Add(new Vector4(b.x, 0, b.y, 1));
            }

            Colors.Add(DefaultColor);
            Colors.Add(DefaultColor);
        }

        public void Load(Vector2 a, Vector2 b, Color color, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(2, null,  primative);
            Primatives.Add(primative);

            if (Orientation == DRAW_ORIENTATION.XY)
            {
                Vertices.Add(a);
                Vertices.Add(b);
            }
            else if (Orientation == DRAW_ORIENTATION.XZ)
            {
                Vertices.Add(new Vector4(a.x, 0, a.y, 1));
                Vertices.Add(new Vector4(b.x, 0, b.y, 1));
            }

            Colors.Add(color);
            Colors.Add(color);
        }

        public void Load(IList<Vector3> vertices)
        {
            Load(vertices, null, LINE_MODE.LINES);
        }

        public void Load(IList<Vector3> vertices, IList<int> indices, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector3> vertices, IList<Color> colors, IList<int> indices = null, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector3> vertices, Color color, IList<int> indices = null, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(color);
            }
        }

        public void Load(Vector3 a, Vector3 b, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(2, null,  primative);
            Primatives.Add(primative);

            Vertices.Add(a);
            Colors.Add(DefaultColor);
            Vertices.Add(b);
            Colors.Add(DefaultColor);
        }

        public void Load(Vector3 a, Vector3 b, Color color, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(2, null,  primative);
            Primatives.Add(primative);

            Vertices.Add(a);
            Colors.Add(color);
            Vertices.Add(b);
            Colors.Add(color);
        }

        public void Load(IList<Vector4> vertices, IList<int> indices = null, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector4> vertices, IList<Color> colors, IList<int> indices = null, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;

            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector4> vertices, Color color, IList<int> indices = null, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;

            SetSegmentIndices(vertices.Count, indices,  primative);
            Primatives.Add(primative);

            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(color);
            }
        }

        public void Load(Vector4 a, Vector4 b, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
 
            SetSegmentIndices(2, null,  primative);
            Primatives.Add(primative);

            Vertices.Add(a);
            Colors.Add(DefaultColor);
            Vertices.Add(b);
            Colors.Add(DefaultColor);
        }

        public void Load(Vector4 a, Vector4 b, Color color, LINE_MODE mode = LINE_MODE.LINES)
        {
            var primative = new RenderPrimative();
            primative.mode = mode;
            
            SetSegmentIndices(2, null,  primative);
            Primatives.Add(primative);

            Vertices.Add(a);
            Colors.Add(color);
            Vertices.Add(b);
            Colors.Add(color);
        }

        private void SetSegmentIndices(int vertexCount, IList<int> indices,  RenderPrimative primative)
        {
            int current = Vertices.Count;

            if (indices == null)
            {
                primative.indices = new int[(vertexCount - 1) * 2];
                for (int i = 0; i < vertexCount - 1; i++)
                {
                    primative.indices[i * 2 + 0] = current + i + 0;
                    primative.indices[i * 2 + 1] = current + i + 1;
                }
            }
            else
            {
                primative.indices = indices.ToArray();
                for (int i = 0; i < indices.Count; i++)
                    primative.indices[i] += current;
            }
        }

        protected override void OnDraw(Camera camera, Matrix4x4 localToWorld)
        {

            foreach(var primative in Primatives)
            {
                if (primative == null) continue;

                switch (primative.mode)
                {
                    case LINE_MODE.LINES:
                        DrawVerticesAsLines(camera, localToWorld, primative.indices);
                        break;

                    case LINE_MODE.TRIANGLES:
                        DrawVerticesAsTriangles(camera, localToWorld, primative.indices);
                        break;

                    case LINE_MODE.QUADS:
                        DrawVerticesAsQuads(camera, localToWorld, primative.indices);
                        break;

                    case LINE_MODE.TETRAHEDRON:
                        DrawVerticesAsTetrahedron(camera, localToWorld, primative.indices);
                        break;

                    case LINE_MODE.PENTAGONS:
                        DrawVerticesAsPentagons(camera, localToWorld, primative.indices);
                        break;

                    case LINE_MODE.HEXAGONS:
                        DrawVerticesAsHexagons(camera, localToWorld, primative.indices);
                        break;
                }
            }

 
        }

        private void DrawVerticesAsLines(Camera camera, Matrix4x4 localToWorld, int[] indices)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < indices.Length / 2; i++)
            {
                int i0 = indices[i * 2 + 0];
                int i1 = indices[i * 2 + 1];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private void DrawVerticesAsTriangles(Camera camera, Matrix4x4 localToWorld, int[] indices)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(DefaultColor);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < indices.Length / 3; i++)
            {
                int i0 = indices[i * 3 + 0];
                int i1 = indices[i * 3 + 1];
                int i2 = indices[i * 3 + 2];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private void DrawVerticesAsQuads(Camera camera, Matrix4x4 localToWorld, int[] indices)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(DefaultColor);

            int vertexCount = Vertices.Count;

            for(int i = 0; i < indices.Length / 4; i++)
            {

                int i0 = indices[i * 4 + 0];
                int i1 = indices[i * 4 + 1];
                int i2 = indices[i * 4 + 2];
                int i3 = indices[i * 4 + 3];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;
                if (i3 < 0 || i3 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);

                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);

                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private void DrawVerticesAsTetrahedron(Camera camera, Matrix4x4 localToWorld, int[] indices)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(DefaultColor);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < indices.Length / 4; i++)
            {
                int i0 = indices[i * 4 + 0];
                int i1 = indices[i * 4 + 1];
                int i2 = indices[i * 4 + 2];
                int i3 = indices[i * 4 + 3];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;
                if (i3 < 0 || i3 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);

                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private void DrawVerticesAsPentagons(Camera camera, Matrix4x4 localToWorld, int[] indices)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(DefaultColor);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < indices.Length / 5; i++)
            {
                int i0 = indices[i * 5 + 0];
                int i1 = indices[i * 5 + 1];
                int i2 = indices[i * 5 + 2];
                int i3 = indices[i * 5 + 3];
                int i4 = indices[i * 5 + 4];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;
                if (i3 < 0 || i3 >= vertexCount) continue;
                if (i4 < 0 || i4 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);

                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);
                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);

                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);
                GL.Color(Colors[i4]);
                GL.Vertex(Vertices[i4]);

                GL.Color(Colors[i4]);
                GL.Vertex(Vertices[i4]);
                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
            }

            GL.End();

            GL.PopMatrix();
        }

        private void DrawVerticesAsHexagons(Camera camera, Matrix4x4 localToWorld, int[] indices)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.MultMatrix(camera.worldToCameraMatrix * localToWorld);
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Color(DefaultColor);

            int vertexCount = Vertices.Count;

            for (int i = 0; i < indices.Length / 6; i++)
            {
                int i0 = indices[i * 6 + 0];
                int i1 = indices[i * 6 + 1];
                int i2 = indices[i * 6 + 2];
                int i3 = indices[i * 6 + 3];
                int i4 = indices[i * 6 + 4];
                int i5 = indices[i * 6 + 5];

                if (i0 < 0 || i0 >= vertexCount) continue;
                if (i1 < 0 || i1 >= vertexCount) continue;
                if (i2 < 0 || i2 >= vertexCount) continue;
                if (i3 < 0 || i3 >= vertexCount) continue;
                if (i4 < 0 || i4 >= vertexCount) continue;
                if (i5 < 0 || i5 >= vertexCount) continue;

                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);

                GL.Color(Colors[i1]);
                GL.Vertex(Vertices[i1]);
                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);

                GL.Color(Colors[i2]);
                GL.Vertex(Vertices[i2]);
                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);

                GL.Color(Colors[i3]);
                GL.Vertex(Vertices[i3]);
                GL.Color(Colors[i4]);
                GL.Vertex(Vertices[i4]);

                GL.Color(Colors[i4]);
                GL.Vertex(Vertices[i4]);
                GL.Color(Colors[i5]);
                GL.Vertex(Vertices[i5]);

                GL.Color(Colors[i5]);
                GL.Vertex(Vertices[i5]);
                GL.Color(Colors[i0]);
                GL.Vertex(Vertices[i0]);
            }

            GL.End();

            GL.PopMatrix();
        }

    }


}