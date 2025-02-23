using System;
using iShape.Geometry;
using iShape.Geometry.Container;
using Unity.Collections;
using UnityEngine;

namespace Synesthesias.Snap.Runtime
{
    public struct Shape
    {
        public Vector2[] hull;
        public Vector2[][] holes;

        public PlainShape ToPlainShape(IntGeom iGeom, Allocator allocator)
        {
            var iHull = iGeom.Int(hull);

            IntShape iShape;
            if (holes != null && holes.Length > 0)
            {
                var iHoles = iGeom.Int(holes);
                iShape = new IntShape(iHull, iHoles);
            }
            else
            {
                iShape = new IntShape(iHull, Array.Empty<IntVector[]>());
            }

            var pShape = new PlainShape(iShape, allocator);

            return pShape;
        }
    }
    public class ShapeManager
    {
        public static Shape[] Data = new Shape[] {
            new Shape {
                hull = new Vector2[] {
                    new Vector2(-15, -15),
                    new Vector2(-15, 15),
                    new Vector2(15, 15),
                    new Vector2(15, -15)
                }
            },
            new Shape {
                hull = new Vector2[] {
                    new Vector2(-5, -15),
                    new Vector2(-15, 15),
                    new Vector2(15, 15),
                    new Vector2(15, -15)
                }
            }
        };
    }
}