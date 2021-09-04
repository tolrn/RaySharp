using System.Numerics;
using Raytracer.Materials;
using OpenTK.Mathematics;
using Raytracer.Core;

namespace Raytracer.Hitables
{
    public struct HitRecord
    {
        public Vector3d position;
        public Vector3d normal;
        public Material material;
        public double t;
        public double u;
        public double v;
        public bool frontFace;

        public void SetFaceNormal(Ray ray, Vector3d outwardNormal)
        {
            frontFace = Vector3d.Dot(ray.Direction, outwardNormal) < 0;
            normal = frontFace ? outwardNormal : -outwardNormal;
        }
    };

    public abstract class Hitable
    {
        public abstract bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec);
        public abstract bool BoundingBox(ref AABB outputBox);
    }
}