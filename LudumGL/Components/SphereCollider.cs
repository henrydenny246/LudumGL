﻿using OpenTK;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionShapes;
using BEPUphysics.CollisionShapes.ConvexShapes;

namespace LudumGL.Components
{
    public class SphereCollider : Collider
    {
        public float Radius { get; set; } = 1;
        public SphereCollider() : base()
        {
            shape = new SphereShape(Radius);
        }

        public override void Update()
        {
            SphereShape sphere = (SphereShape)shape;
            if (sphere.Radius * Parent.Transform.localScale.Y != Radius * Parent.Transform.localScale.Y)
            {
                sphere.Radius = Radius * Parent.Transform.localPosition.Y;
            }
            base.Update();

        }
    }
}
