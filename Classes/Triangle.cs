using System.Numerics;

namespace MeshFiller.Classes
{
    public class Triangle(Vertex v1, Vertex v2, Vertex v3)
    {
        public Vertex V1 = v1, V2 = v2, V3 = v3;
        public Vector2 P0, P1;
        public Vector3 B0, B1;
        public float d00;
        public float d01;
        public float d11;
        public float p00;
        public float p01;
        public float p11;
        public float pInvDenom;
        public float invDenom;

        public void Recalculate()
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;

            P0 = new Vector2(V2.X - V1.X, V2.Y - V1.Y);
            P1 = new Vector2(V3.X - V1.X, V3.Y - V1.Y);

            p00 = Vector2.Dot(P0, P0);
            p01 = Vector2.Dot(P0, P1);
            p11 = Vector2.Dot(P1, P1);
            pInvDenom = 1f / (p00 * p11 - p01 * p01);

            B0 = v2.RotP - v1.RotP;
            B1 = v3.RotP - v1.RotP;

            d00 = Vector3.Dot(B0, B0);
            d01 = Vector3.Dot(B0, B1);
            d11 = Vector3.Dot(B1, B1);
            invDenom = 1f / (d00 * d11 - d01 * d01);
        }
    }
}
