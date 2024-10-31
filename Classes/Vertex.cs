using System.Numerics;

namespace MeshFiller.Classes
{
    public struct Vertex
    {
        // Pre-rotation
        public Vector3 P;
        public Vector3 Pu; // Tangent
        public Vector3 Pv; // Tangent
        public Vector3 N; // Normal

        // Post-rotation
        public Vector3 RotatedP;
        public Vector3 RotatedPu;
        public Vector3 RotatedPv;
        public Vector3 RotatedN;
    }
}
