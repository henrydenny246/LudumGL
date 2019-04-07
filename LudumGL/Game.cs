﻿using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

using LudumGL.UserInterface;

namespace LudumGL
{
    /// <summary>
    /// The link between your game and the engine.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The initial settings of the game.
        /// </summary>
        public static InitialSettings initialSettigns = InitialSettings.Default;

        /// <summary>
        /// Game loop implementation.
        /// </summary>
        public static GameLoop gameLoop;

        /// <summary>
        /// Array that stores the currently used
        /// lights in the scene.
        /// </summary>
        public static Light[] activeLights;


        /// <summary>
        /// The camera that GameObjects will be viewed from.
        /// </summary>
        public static Camera mainCamera;

        /// <summary>
        /// The aspect ratio of the window.
        /// </summary>
        public static float AspectRatio { get => window.Width / (float)window.Height; }

        /// <summary>
        /// Ambient light color of the scene.
        /// </summary>
        public static Vector3 AmbientLightColor { get; set; } = new Vector3(0.1f, 0.1f, 0.1f);

        /// <summary>
        /// Gets or sets the mouse lock status.
        /// </summary>
        public static bool MouseLocked { get; set; }

        internal static GameWindow window;

        public static void Start()
        {
            window = new GameWindow(initialSettigns.Width, initialSettigns.Height)
            {
                Title = initialSettigns.Title
            };
            if(initialSettigns.Fullscreen)
                window.WindowState = WindowState.Fullscreen;

            window.UpdateFrame += PreUpdate;
            window.UpdateFrame += gameLoop.Update;
            window.UpdateFrame += PostUpdate;
            window.RenderFrame += PreRender;
            window.RenderFrame += gameLoop.Render;
            window.RenderFrame += PostRender;
            window.KeyDown += Input.KeyPress;
            window.KeyUp += Input.KeyRelease;
            window.Resize += Resize;

            Initialize();
            gameLoop.Start();

            window.Run(60);
        }

        public static void Exit()
        {
            window.Exit();
        }

        static void Initialize()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            activeLights = new Light[10];

            Physics.Initialize();
            Mesh.InitializeDefaultMeshes();
        }

        static void PreUpdate(object sender, FrameEventArgs e)
        {
            Input.Update();
            GameObject.Update();
            UI.Update();
            
            if(MouseLocked)
            {
                Mouse.SetPosition(0, 0);
                window.CursorVisible = false;
            }else
            {
                window.CursorVisible = true;
            }
        }

        static void PostUpdate(object sender, FrameEventArgs e)
        {
            Physics.Update(2f / 100f);
        }

        static void PreRender(object sender, FrameEventArgs e)
        {
            GameObject.Render();
            UI.Render();
        }

        static void PostRender(object sender, FrameEventArgs e)
        {
            window.SwapBuffers();
            GL.ClearColor(0, 0, 0, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        
        static void Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, window.Width, window.Height);
        }
    }

    /// <summary>
    /// Contains the initial settigns of the game window.
    /// </summary>
    public struct InitialSettings
    {
        public static InitialSettings Default
        {
            get
            {
                return new InitialSettings
                {
                    Width = 640,
                    Height = 480,
                    Title = "LudumGL"
                };
            }
        }

        public bool Fullscreen { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }
    }
}
