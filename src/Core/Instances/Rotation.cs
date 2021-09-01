using System;
using System.Numerics;
using OpenTK.Mathematics;
using Raytracer.Core.Hitables;
using Raytracer.Core.Materials;
using Raytracer.Utility;

namespace Raytracer.Core.Instances
{
    class RotateY : Hitable
    {
        private Hitable _material;
        private double _sinTheta;
        private double _cosTheta;
        private bool _hasBox;
        private AABB _boundingBox;

        public RotateY() { }

        public RotateY(Hitable hitable, double angle)
        {
            _material = hitable;
            var radians = GeneralHelper.ConvertToRadians(angle);
            _sinTheta = Math.Sin(radians);
            _cosTheta = Math.Cos(radians);
            _hasBox = _material.BoundingBox(ref _boundingBox);

            Vector3d min = new(Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity);
            Vector3d max = new(Double.NegativeInfinity, Double.NegativeInfinity, Double.NegativeInfinity);

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var x = i * _boundingBox.Maximum.X + (1 - i) * _boundingBox.Minimum.X;
                        var y = j * _boundingBox.Maximum.Y + (1 - j) * _boundingBox.Minimum.Y;
                        var z = k * _boundingBox.Maximum.Z + (1 - k) * _boundingBox.Minimum.Z;

                        var newX = _cosTheta * x + _sinTheta * z;
                        var newZ = -_sinTheta * x + _cosTheta * z;

                        Vector3d tester = new(newX, y, newZ);

                        for (int c = 0; c < 3; c++)
                        {
                            min[c] = Math.Min(min[c], tester[c]);
                            max[c] = Math.Max(max[c], tester[c]);
                        }
                    }
                }
            }

            _boundingBox = new AABB(min, max);
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            var origin = ray.Origin;
            var direction = ray.Direction;

            origin[0] = _cosTheta * ray.Origin[0] - _sinTheta * ray.Origin[2];
            origin[2] = _sinTheta * ray.Origin[0] + _cosTheta * ray.Origin[2];

            direction[0] = _cosTheta * ray.Direction[0] - _sinTheta * ray.Direction[2];
            direction[2] = _sinTheta * ray.Direction[0] + _cosTheta * ray.Direction[2];

            Ray rotated_r = new(origin, direction);

            if (!_material.Hit(rotated_r, tMin, tMax, ref rec))
                return false;

            var p = rec.position;
            var normal = rec.normal;

            p[0] = _cosTheta * rec.position[0] + _sinTheta * rec.position[2];
            p[2] = -_sinTheta * rec.position[0] + _cosTheta * rec.position[2];

            normal[0] = _cosTheta * rec.normal[0] + _sinTheta * rec.normal[2];
            normal[2] = -_sinTheta * rec.normal[0] + _cosTheta * rec.normal[2];

            rec.position = p;
            rec.SetFaceNormal(rotated_r, normal);

            return true;
        }

        public override bool BoundingBox(ref AABB outputBox)
        {
            outputBox = _boundingBox;
            return _hasBox;
        }
    }
}
