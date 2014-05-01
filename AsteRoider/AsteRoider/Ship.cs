using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteRoider
{
    class Ship
    {
        Vector3 heading;
        private GraphicsDevice device;
        private Texture2D texture;
        private Vector3 location;


        private VertexBuffer shipVertexBuffer;
        private List<VertexPositionTexture> vertices = new List<VertexPositionTexture>();

        public Ship (GraphicsDevice graphicsdev, Texture2D texture)
        {
            this.texture = texture;
            device = graphicsdev;

        }
    }
}
//<><>