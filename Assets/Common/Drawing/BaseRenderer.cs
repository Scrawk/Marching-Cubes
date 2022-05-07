using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Common.Unity.Drawing
{

    public enum DRAW_ORIENTATION { XY, XZ };

    public abstract class BaseRenderer
    {
        public static readonly IList<int> CUBE_INDICES = new int[]
        {
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };

        public static readonly IList<int> SQUARE_LINE_INDICES = new int[]
        {
            0, 1, 1, 2, 2, 3, 3, 0
        };

        public static readonly IList<int> SQUARE_FACE_INDICES = new int[]
        {
            0, 1, 2, 0, 2, 3
        };

        private Material m_material;

        public BaseRenderer()
        {
            Vertices = new List<Vector4>();
            Colors = new List<Color>();
            Indices = new List<int>();
            LocalToWorld = Matrix4x4.identity;
            Orientation = DRAW_ORIENTATION.XY;
            DefaultColor = Color.white;
            CullMode = CullMode.Off;
            Enabled = true;
        }

        public string Name { get; set; }

        protected List<Vector4> Vertices { get; set; }

        protected List<Color> Colors { get; set; }

        protected List<int> Indices { get; set; }

        public Matrix4x4 LocalToWorld { get; set; }

        public DRAW_ORIENTATION Orientation { get; set;  }

        public Color DefaultColor { get; set; }

        public bool ScaleOnZoom { get; set; }

        public bool Enabled { get; set; }

        public CompareFunction ZTest
        {
            get { return (CompareFunction)Material.GetInt("_ZTest"); }
            set { Material.SetInt("_ZTest", (int)value); }
        }

        public CullMode CullMode
        {
            get { return (CullMode)Material.GetInt("_Cull"); }
            set { Material.SetInt("_Cull", (int)value); }
        }

        public bool ZWrite
        {
            get { return Material.GetInt("_ZWrite") == 0 ? false : true; }
            set { Material.SetInt("_ZWrite", value == false ? 0 : 1); }
        }

        public BlendMode SrcBlend
        {
            get { return (BlendMode)Material.GetInt("_SrcBlend"); }
            set { Material.SetInt("_SrcBlend", (int)value); }
        }

        public BlendMode DstBlend
        {
            get { return (BlendMode)Material.GetInt("_DstBlend"); }
            set { Material.SetInt("_DstBlend", (int)value); }
        }

        public int Renderqueue
        {
            get { return Material.renderQueue; }
            set { Material.renderQueue = value; }
        }

        protected Material Material
        {
            get
            {
                if (m_material == null)
                    m_material = new Material(Shader.Find("Hidden/Internal-Colored"));

                return m_material;
            }
        }

        public virtual void Clear()
        {
            Vertices.Clear();
            Colors.Clear();
            Indices.Clear();
        }

        public void SetColor(Color color)
        {
            DefaultColor = color;
            for (int i = 0; i < Colors.Count; i++)
                Colors[i] = color;
        }

        public void Translate(Vector3 translation)
        {
            LocalToWorld = Matrix4x4.TRS(translation, Quaternion.identity, Vector3.one);
        }

        public void Translate(float x, float y, float z)
        {
            LocalToWorld = Matrix4x4.TRS(new Vector3(x, y, z), Quaternion.identity, Vector3.one);
        }

        public void SetFaceIndices(int vertexCount, IList<int> indices)
        {
            int current = Vertices.Count;

            if (indices == null)
            {
                for (int i = 0; i < vertexCount; i++)
                    Indices.Add(i + current);
            }
            else
            {
                for (int i = 0; i < indices.Count; i++)
                    Indices.Add(indices[i] + current);
            }

        }

        public static int[] PolygonIndices(int count)
        {
            int[] indices = new int[count * 2];

            for (int i = 0; i < count; i++)
            {
                indices[i * 2 + 0] = i;

                if (i == count - 1)
                    indices[i * 2 + 1] = 0;
                else
                    indices[i * 2 + 1] = i + 1;
            }

            return indices;
        }

        public static int[] SegmentIndices(int count)
        {
            int[] indices = new int[count * 2];

            for (int i = 0; i < count; i++)
            {
                indices[i * 2 + 0] = i * 2 + 0;
                indices[i * 2 + 1] = i * 2 + 1;
            }

            return indices;
        }

        //public abstract void Load(IList<Vector2> points);

        //public abstract void Load(IList<Vector3> points);

        public void Draw()
        {
            if (Enabled)
                Draw(Camera.current);
        }

        public void Draw(Camera camera)
        {
            if (Enabled)
                OnDraw(camera, LocalToWorld);
        }

        public void Draw(Camera camera, Matrix4x4 localToWorld)
        {
            if (Enabled)
                OnDraw(camera, localToWorld);
        }

        protected abstract void OnDraw(Camera camera, Matrix4x4 localToWorld);

    }

}
