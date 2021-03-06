﻿using System;
using OpenTK;
using LudumGL.Rendering;

namespace LudumGL.UserInterface
{
    /// <summary>
    /// More advanced version of Panel, allowing you to
    /// have better-looking borders.
    /// </summary>
    public class ScalablePanel : UIElement
    {
        readonly Drawable[,] drawableMatrix;

        internal int TextureHalf { get => Material.Texture.Width / 2; }
        internal int TextureThird { get => Material.Texture.Width / 3; }

        public ScalablePanel()
        {
            drawableMatrix = new Drawable[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Drawable drawable = DrawableMesh.Create(Mesh.Copy(Mesh.Rectangle), Shaders.Unlit);
                    drawableMatrix[i, j] = drawable;

                    DrawableMesh drawableMesh = (DrawableMesh)drawable;
                    drawableMesh.mesh.MapUVs(i / 3f, i / 3f + Mathl.Third, j / 3f, j / 3f + Mathl.Third);
                    drawableMesh.mesh.Refresh(MeshRefreshMode.UVs);

                    drawables.Add(drawable);
                }
            }
        }

        public override void Update()
        {
            Vector2 fullSize = new Vector2(Material.Texture.Width, Material.Texture.Height) * Size;
            fullSize.X += fullSize.X % 2;
            fullSize.Y += fullSize.Y % 2;

            pixelSize = fullSize;

            PositionAndScale(fullSize.X, fullSize.Y, false);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    float ii = i - 1;
                    float jj = j - 1;
                    float x = ii * TextureThird * (Size.X * 1.5f - 0.5f) + pixelPosition.X;
                    float y = jj * TextureThird * (Size.Y * 1.5f - 0.5f) + pixelPosition.Y;

                    Drawable drawable = drawableMatrix[i, j];
                    drawable.Transform.localPosition = new Vector3((int)x, (int)y, -1);
                    drawable.Transform.localScale = new Vector3(TextureThird, TextureThird, 1);
                }
            }

            float scaledWidth = fullSize.X - TextureThird * 2f;
            float scaledHeight = fullSize.Y - TextureThird * 2f;

            drawableMatrix[1, 0].Transform.localScale.X = scaledWidth;
            drawableMatrix[1, 2].Transform.localScale.X = scaledWidth;
            drawableMatrix[0, 1].Transform.localScale.Y = scaledHeight;
            drawableMatrix[2, 1].Transform.localScale.Y = scaledHeight;
            drawableMatrix[1, 1].Transform.localScale = new Vector3(scaledWidth, scaledHeight, 1);

            base.Update();
        }
    }
}
