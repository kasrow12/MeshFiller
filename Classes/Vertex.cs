using System.Numerics;

namespace MeshFiller.Classes
{
    public class Vertex
    {
        // Pre-rotation
        public Vector3 P;
        public Vector3 Pu; // Tangent
        public Vector3 Pv; // Tangent
        public Vector3 N; // Normal

        // Post-rotation
        public Vector3 RotP;
        public Vector3 RotPu;
        public Vector3 RotPv;
        public Vector3 RotN;
    }
}
