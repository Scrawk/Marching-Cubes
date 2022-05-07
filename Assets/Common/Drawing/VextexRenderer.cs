using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Unity.Drawing
{

    public class VertexRenderer : BaseRenderer
    {

        public VertexRenderer(float size)
        {
            Size = size;
            Orientation = DRAW_ORIENTATION.XY;
        }

        public VertexRenderer(float size, DRAW_ORIENTATION orientation)
        {
            Size = size;
            Orientation = orientation;
        }

        public float Size = 0.1f;

        public void Load(Vector2 vertex)
        {
            var v = vertex;

            if (Orientation == DRAW_ORIENTATION.XY)
                Vertices.Add(v);
            else if (Orientation == DRAW_ORIENTATION.XZ)
                Vertices.Add(new Vector4(v.x, 0, v.y, 1));

            Colors.Add(DefaultColor);
        }

        public void Load(Vector2 vertex, Color color)
        {
            var v = vertex;

            if (Orientation == DRAW_ORIENTATION.XY)
                Vertices.Add(v);
            else if (Orientation == DRAW_ORIENTATION.XZ)
                Vertices.Add(new Vector4(v.x, 0, v.y, 1));

            Colors.Add(color);
        }

        public void Load(IList<Vector2> vertices)
        {
            for(int i = 0; i < vertices.Count; i++)
            {
                var v = vertices[i];

                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector2> vertices, IList<Color> colors)
        {
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

        public void Load(IList<Vector2> vertices, Color color)
        {
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

        public void Load(Vector3 vertex)
        {
            Vertices.Add(vertex);
            Colors.Add(DefaultColor);
        }

        public void Load(Vector3 vertex, Color color)
        {
            Vertices.Add(vertex);
            Colors.Add(color);
        }

        public void Load(IList<Vector3> vertices)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector3> vertices, IList<Color> colors)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector3> vertices, Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(color);
            }
        }

        public void Load(Vector4 vertex)
        {
            Vertices.Add(vertex);
            Colors.Add(DefaultColor);
        }

        public void Load(Vector4 vertex, Color color)
        {
            Vertices.Add(vertex);
            Colors.Add(color);
        }

        public void Load(IList<Vector4> vertices)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector4> vertices, IList<Color> colors)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(colors[i]);
            }
        }

        public void Load(IList<Vector4> vertices, Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                Vertices.Add(vertices[i]);
                Colors.Add(color);
            }
        }

        protected override void OnDraw(Camera camera, Matrix4x4 localToWorld)
        {
            if (Size <= 0) return;
            float size = Size;

            if (camera.orthographic && ScaleOnZoom)
                size *= camera.orthographicSize / 10.0f;

            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.QUADS);

            switch (Orientation)
            {
                case DRAW_ORIENTATION.XY:
                    DrawXY(size);
                    break;

                case DRAW_ORIENTATION.XZ:
                    DrawXZ(size);
                    break;
            }

            GL.End();

            GL.PopMatrix();
        }

        private  void DrawXY(float size)
        {
            float half = size * 0.5f;
            for (int i = 0; i < Vertices.Count; i++)
            {
                float x = Vertices[i].x;
                float y = Vertices[i].y;
                float z = Vertices[i].z;
                Color color = Colors[i];

                GL.Color(color);
                GL.Vertex3(x + half, y + half, z);
                GL.Vertex3(x + half, y - half, z);
                GL.Vertex3(x - half, y - half, z);
                GL.Vertex3(x - half, y + half, z);
            }
        }

        private  void DrawXZ(float size)
        {
            float half = size * 0.5f;
            for (int i = 0; i < Vertices.Count; i++)
            {
                float x = Vertices[i].x;
                float y = Vertices[i].y;
                float z = Vertices[i].z;
                Color color = Colors[i];

                GL.Color(color);
                GL.Vertex3(x + half, y, z + half);
                GL.Vertex3(x + half, y, z - half);
                GL.Vertex3(x - half, y, z - half);
                GL.Vertex3(x - half, y, z + half);
            }
        }
      
    }

}
