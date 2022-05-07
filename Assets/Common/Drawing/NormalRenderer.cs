using UnityEngine;
using System;
using System.Collections.Generic;


namespace Common.Unity.Drawing
{

    public class NormalRenderer : BaseRenderer
    {

        private List<Vector4> m_normals = new List<Vector4>();

        public NormalRenderer()
        {

        }

        public NormalRenderer(DRAW_ORIENTATION orientation)
        {
            Orientation = orientation;
        }

        public override void Clear()
        {
            base.Clear();
            m_normals.Clear();
        }

        public float Length = 1;

        public void Load(IList<Vector2> vertices)
        {
            foreach (var v in vertices)
            {
                var n = v.normalized;
                if (Orientation == DRAW_ORIENTATION.XY)
                {
                    Vertices.Add(v);
                    m_normals.Add(n);
                }
                else if (Orientation == DRAW_ORIENTATION.XZ)
                {
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));
                    m_normals.Add(new Vector4(n.x, 0, n.y, 1));
                }
                    
                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector2> vertices, IList<Vector2> normals)
        {
            foreach (var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(DefaultColor);
            }

            foreach (var n in normals)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_normals.Add(n);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_normals.Add(new Vector4(n.x, 0, n.y, 1));
            }
        }

        public void Load(IList<Vector2> vertices, IList<Vector2> normals, Color col)
        {
            foreach (var v in vertices)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    Vertices.Add(v);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    Vertices.Add(new Vector4(v.x, 0, v.y, 1));

                Colors.Add(col);
            }

            foreach (var n in normals)
            {
                if (Orientation == DRAW_ORIENTATION.XY)
                    m_normals.Add(n);
                else if (Orientation == DRAW_ORIENTATION.XZ)
                    m_normals.Add(new Vector4(n.x, 0, n.y, 1));
            }
        }

        public void Load(IList<Vector3> vertices)
        {
            foreach (var v in vertices)
            {
                var n = v.normalized;
                Vertices.Add(v);
                m_normals.Add(n);
                Colors.Add(DefaultColor);
            }
        }

        public void Load(IList<Vector3> vertices, IList<Vector3> normals)
        {
            foreach (var v in vertices)
            {
                Vertices.Add(v);
                Colors.Add(DefaultColor);
            }

            foreach (var n in normals)
                m_normals.Add(n);
        }

        public void Load(IList<Vector3> vertices, IList<Vector3> normals, Color col)
        {
            foreach (var v in vertices)
            {
                Vertices.Add(v);
                Colors.Add(col);
            }

            foreach (var n in normals)
                m_normals.Add(n);
        }

        protected override void OnDraw(Camera camera, Matrix4x4 localToWorld)
        {
            GL.PushMatrix();

            GL.LoadIdentity();
            GL.modelview = camera.worldToCameraMatrix * localToWorld;
            GL.LoadProjectionMatrix(camera.projectionMatrix);

            Material.SetPass(0);
            GL.Begin(GL.LINES);

            int vertexCount = Vertices.Count;
            for (int i = 0; i < vertexCount; i++)
            {
                GL.Color(Colors[i]);
                GL.Vertex(Vertices[i]);
                GL.Vertex(Vertices[i] + m_normals[i] * Length);
            }

            GL.End();

            GL.PopMatrix();
        }

    }

}