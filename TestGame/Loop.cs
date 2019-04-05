﻿using System;
using System.Collections.Generic;
using LudumGL;
using OpenTK;
using OpenTK.Input;

namespace TestGame
{
    class Loop : GameLoop
    {
        static Light light;
        static Camera camera;
        static GameObject cube;
        static GameObject monkey;
        static GameObject sphere;

        public override void Start()
        {
            light = new Light
            {
                enabled = true,
                type=LightType.Point,
                color = new Vector4(1, 1, 1, 1),
                range = 30,
                position = new Vector3(0, 0, 5),
                rotation = Quaternion.FromEulerAngles(0, 0, 0)
            };
            Game.activeLights[0] = light;
            camera = new Camera();
            camera.Transform.localPosition = new Vector3(0, 0, 0);
            Game.mainCamera = camera;
            Input.MouseSensitivity = 0.1f;

            cube = new GameObject
            {
                Drawable = Drawable.MakeDrawable(Mesh.Load("assets/mesh/cube.dae"), Shaders.Lit),
                Transform = new Transform() { localPosition = new Vector3(0, 0, -5) }
            };
            monkey = new GameObject
            {
                Drawable = Drawable.MakeDrawable(Mesh.Load("assets/mesh/monkey.dae"), Shaders.Lit),
                Transform = new Transform() { localPosition = new Vector3(0, 0, -5), Parent=cube.Transform},
            };
            sphere = new GameObject
            {
                Drawable = Drawable.MakeDrawable(Mesh.Load("assets/mesh/sphere.dae"), Shaders.Lit),
                Transform = new Transform() { localPosition = new Vector3(0, -2, 0), localScale=new Vector3(1,1,1)*0.1f, Parent = monkey.Transform },
            };
            GameObject.Add(cube);
            GameObject.Add(monkey);
            GameObject.Add(sphere);

        }

        static float t;
        public override void Update(object sender, FrameEventArgs e)
        {
            t += 0.01f;
            if(Input.GetKey(Key.W))
            {
                camera.Transform.localPosition += Game.mainCamera.Transform.Forward * 0.1f;
            }
            if (Input.GetKey(Key.S))
            {
                camera.Transform.localPosition -= Game.mainCamera.Transform.Forward * 0.1f;
            }
            if (Input.GetKey(Key.A))
            {
                camera.Transform.localPosition -= Game.mainCamera.Transform.Right * 0.1f;
            }
            if (Input.GetKey(Key.D))
            {
                camera.Transform.localPosition += Game.mainCamera.Transform.Right * 0.1f;
            }
            if (Input.GetKey(Key.Q))
            {
                camera.Transform.Rotate(0, 0, -2);
            }
            if (Input.GetKey(Key.E))
            {
                camera.Transform.Rotate(0, 0, 2);
            }
            camera.Transform.Rotate(-Input.MouseDelta.Y * Input.MouseSensitivity, -Input.MouseDelta.X * Input.MouseSensitivity, 0);
            light.position = camera.Transform.Position;

            cube.Transform.Rotate(0, 1, 0);
            monkey.Transform.Rotate(0, 0, 1);
        }

        public override void Render(object sender, FrameEventArgs e)
        {

        }
    }
}
