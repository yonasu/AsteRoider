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
        Texture2D texture;
        VertexBuffer floorBuffer;
        Color[] floorColors = new Color[2] { Color.Pink, Color.Black };
        #endregion
        #region Constructor
        public Background(GraphicsDevice device, Texture2D texture)
        {
            this.device = device;
            this.texture = texture;
            BuildFloorBuffer();
        }
        #endregion
        #region The Floor
        private void BuildFloorBuffer()
        {
            List<VertexPositionTexture> vertexList = new List<VertexPositionTexture>();
            int counter = 0;
            for (int x = 0; x < groundWidth; x++)
            {
                counter++;
                for (int z = 0; z < groundHeight; z++)
                {
                    counter++;
                    foreach (VertexPositionTexture vertex in
                    FloorTile(x, z, floorColors[counter % 2]))
                    {
                        vertexList.Add(vertex);
                    }
                }
            }
            floorBuffer = new VertexBuffer(device,VertexPositionTexture.VertexDeclaration,vertexList.Count,BufferUsage.WriteOnly);
            floorBuffer.SetData<VertexPositionTexture>(vertexList.
            ToArray());
        }
        private List<VertexPositionTexture> FloorTile(
        int xOffset,
        int zOffset,
        Color tileColor)
        {
            List<VertexPositionTexture> vList =
            new List<VertexPositionTexture>();
            vList.Add(new VertexPositionTexture(new Vector3(0 + xOffset, 0, 0 + zOffset), new Vector2(0, 0)));
            vList.Add(new VertexPositionTexture(new Vector3(1 + xOffset, 0, 0 + zOffset), new Vector2(1, 0)));
            vList.Add(new VertexPositionTexture(new Vector3(0 + xOffset, 0, 1 + zOffset), new Vector2(0, 1)));
            vList.Add(new VertexPositionTexture(new Vector3(1 + xOffset, 0, 0 + zOffset), new Vector2(1, 0)));
            vList.Add(new VertexPositionTexture(new Vector3(1 + xOffset, 0, 1 + zOffset), new Vector2(1, 1)));
            vList.Add(new VertexPositionTexture(new Vector3(0 + xOffset, 0, 1 + zOffset), new Vector2(0, 1)));
            return vList;
        }



        #endregion
        #region Draw
        public void Draw(Camera camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = texture;
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
