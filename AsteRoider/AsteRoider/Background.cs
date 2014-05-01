using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AsteRoider
{
    class Background
    {
        #region Fields
        public const int groundWidth = 20;
        public const int groundHeight = 20;
        GraphicsDevice device;
        VertexBuffer floorBuffer;
        Color[] floorColors = new Color[2] { Color.Pink, Color.Black };
        #endregion
        #region Constructor
        public Background(GraphicsDevice device)
        {
            this.device = device;
            BuildFloorBuffer();
        }
        #endregion
        #region The Floor
        private void BuildFloorBuffer()
        {
            List<VertexPositionColor> vertexList =
            new List<VertexPositionColor>();
            int counter = 0;
            for (int x = 0; x < groundWidth; x++)
            {
                counter++;
                for (int z = 0; z < groundHeight; z++)
                {
                    counter++;
                    foreach (VertexPositionColor vertex in
                    FloorTile(x, z, floorColors[counter % 2]))
                    {
                        vertexList.Add(vertex);
                    }
                }
            }
            floorBuffer = new VertexBuffer(
            device,
            VertexPositionColor.VertexDeclaration,
            vertexList.Count,
            BufferUsage.WriteOnly);
            floorBuffer.SetData<VertexPositionColor>(vertexList.
            ToArray());
        }
        private List<VertexPositionColor> FloorTile(
        int xOffset,
        int zOffset,
        Color tileColor)
        {
            List<VertexPositionColor> vList =
            new List<VertexPositionColor>();
            vList.Add(new VertexPositionColor(
            new Vector3(0 + xOffset, 0, 0 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(
            new Vector3(1 + xOffset, 0, 0 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(
            new Vector3(0 + xOffset, 0, 1 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(
            new Vector3(1 + xOffset, 0, 0 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(
            new Vector3(1 + xOffset, 0, 1 + zOffset), tileColor));
            vList.Add(new VertexPositionColor(
            new Vector3(0 + xOffset, 0, 1 + zOffset), tileColor));
            return vList;
        }
        #endregion
        #region Draw
        public void Draw(Camera camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = true;
            effect.World = Matrix.Identity;
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(floorBuffer);
                device.DrawPrimitives(
                PrimitiveType.TriangleList,
                0,
                floorBuffer.VertexCount / 3);
            }
        }
        #endregion
    }
}
