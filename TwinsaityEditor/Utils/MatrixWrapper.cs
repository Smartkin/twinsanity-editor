using OpenTK;

namespace TwinsaityEditor.Utils
{
    public static class MatrixWrapper
    {
        public static Matrix4 RotateMatrix4(float angleX, float angleY, float angleZ)
        {
            Matrix4 mat = Matrix4.Identity;

            mat *= Matrix4.CreateRotationX(angleX);
            mat *= Matrix4.CreateRotationY(angleY);
            mat *= Matrix4.CreateRotationZ(angleZ);

            return mat;
        }

        public static void RotateMatrix4(ref Matrix4 mat, float angleX, float angleY, float angleZ)
        {
            mat *= RotateMatrix4(angleX, angleY, angleZ);
        }
    }
}
