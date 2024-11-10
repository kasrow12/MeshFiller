using System.Numerics;

namespace MeshFiller.Classes
{
    public struct Matrix3x3
    {
        private Vector3 row1, row2, row3;

        public Matrix3x3(Vector3 col1, Vector3 col2, Vector3 col3)
        {
            row1 = new Vector3(col1.X, col2.X, col3.X);
            row2 = new Vector3(col1.Y, col2.Y, col3.Y);
            row3 = new Vector3(col1.Z, col2.Z, col3.Z);
        }

        public static Vector3 operator *(Matrix3x3 matrix, Vector3 vec)
        {
            return new Vector3(
                Vector3.Dot(matrix.row1, vec),
                Vector3.Dot(matrix.row2, vec),
                Vector3.Dot(matrix.row3, vec)
            );
        }
    }
}
