using System.Numerics;
using Raytracer.Core.Materials;

namespace Raytracer.Core.Hitables
{
    struct HitRecord
    {
        public Vector3 position;
        public Vector3 normal;
        public Material material;
        public double t;
        public double u;
        public double v;
        public bool frontFace;

        public void set_face_normal(Ray ray, Vector3 outwardNormal)
        {
            frontFace = Vector3.Dot(ray.Direction, outwardNormal) < 0;
            normal = frontFace ? outwardNormal : -outwardNormal;
        }
    };

    abstract class Hitable
    {
        public abstract bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec);
        public abstract bool BoundingBox(ref AABB outputBox);
    }
}
