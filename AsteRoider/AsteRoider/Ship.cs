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


        private float rotation = 0f;
        private float zrotation = 0f;

        private VertexBuffer shipVertexBuffer;
        private List<VertexPositionTexture> vertices = new List<VertexPositionTexture>();

        public Ship(GraphicsDevice graphicsdev, Texture2D texture)
        {
            this.texture = texture;
            device = graphicsdev;
            PositionCube(Vector3.Zero, 0.0f);

            BuildFace(new Vector3(0, 0, 0), new Vector3(0, 1, 1));

            //bottom
            //one corner
            //vertices.Add(BuildVertex(0, 0, 0, 0, 1));
            //vertices.Add(BuildVertex(-1, 0, 0, 1, 1));
            //vertices.Add(BuildVertex(-0.5f, 1, 1, 1, 0));
            
            //vertices.Add(BuildVertex(0.5f, 1, 1, 1, 0));
            //vertices.Add(BuildVertex(1, 0, 0, 1, 0));
            //vertices.Add(BuildVertex(0, 0, 0, 0, 1));

            //vertices.Add(BuildVertex(-0.5f, 1, 1, 1, 0));
            //vertices.Add(BuildVertex(0.5f, 1, 1, 1, 0));
            //vertices.Add(BuildVertex(0, 0, 0, 0, 1));

            //FAN TRIANGLE NOPE THERE IS NO FAN MOFO
            vertices.Add(BuildVertex(0, 0, 0.25f, 0, 1));   // 4
            vertices.Add(BuildVertex(1, -1, 0, 1, 0));  // 2
            
            vertices.Add(BuildVertex(-1, -1, 0, 1, 1)); // 3

            vertices.Add(BuildVertex(0, 0, 0.25f, 0, 1));   // 4
            vertices.Add(BuildVertex(-1, -1, 0, 1, 1)); // 3

            vertices.Add(BuildVertex(0, 1, 0, 0, 0));  // 1
            vertices.Add(BuildVertex(0, 0, 0.25f, 0, 1));   // 4
            vertices.Add(BuildVertex(0, 1, 0, 0, 0));  // 1
            vertices.Add(BuildVertex(1, -1, 0, 1, 0));  // 2
            vertices.Add(BuildVertex(1, -1, 0, 1, 0));  // 2
            vertices.Add(BuildVertex(0, 1, 0, 0, 0));  // 1
            vertices.Add(BuildVertex(-1, -1, 0, 1, 1)); // 3



            // Create the cube's vertical faces
            ////BuildFace(new Vector3(0, 0, 0), new Vector3(0, 1, 1));
            ////BuildFace(new Vector3(0, 0, 1), new Vector3(1, 1, 1));
            ////BuildFace(new Vector3(1, 0, 1), new Vector3(1, 1, 0));
            ////BuildFace(new Vector3(1, 0, 0), new Vector3(0, 1, 0));
            //// Create the cube's horizontal faces
            //BuildFaceHorizontal(new Vector3(0, 1, 0), new Vector3(1, 1, 1));

            //BuildFaceHorizontal(new Vector3(0, 0, 1), new Vector3(1, 0, 0));

            shipVertexBuffer = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, vertices.Count, BufferUsage.WriteOnly);
            shipVertexBuffer.SetData<VertexPositionTexture>(vertices.ToArray());
        }

        public void PositionCube(Vector3 playerLocation, float minDistance)
        {
            location = new Vector3(1.5f, 0.5f, 1.5f);
        }
        private void BuildTripleFace(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 0, 1));

        }



        private void BuildFaceHorizontal(Vector3 p1, Vector3 p2)
        {
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 0, 1));
            vertices.Add(BuildVertex(p2.X, p1.Y, p1.Z, 1, 1));
            vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, 1, 0));
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 0, 1));
            vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, 1, 0));
            vertices.Add(BuildVertex(p1.X, p1.Y, p2.Z, 0, 0));
        }

        private VertexPositionTexture BuildVertex(float x1, float y1, float z1, int x2, int y2)
        {
            return new VertexPositionTexture(new Vector3(x1, y1, z1), new Vector2(x2, y2));
        }


        private void BuildFace(Vector3 p1, Vector3 p2)
        {
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 1, 0));
            vertices.Add(BuildVertex(p1.X, p2.Y, p1.Z, 1, 1));
            vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, 0, 1));
            vertices.Add(BuildVertex(p2.X, p2.Y, p2.Z, 0, 1));
            vertices.Add(BuildVertex(p2.X, p1.Y, p2.Z, 0, 0));
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 1, 0));
        }
        public void Draw(Camera camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;
            effect.Texture = texture;

            Matrix rot = Matrix.CreateRotationY(rotation);
            Matrix zrot = Matrix.CreateRotationZ(rotation); 

            Matrix center = Matrix.CreateTranslation(new Vector3(-0.5f, -0.5f, -0.5f));
            Matrix scale = Matrix.CreateScale(0.5f);
            Matrix translate = Matrix.CreateTranslation(location);
            effect.World = center * rot * zrot * scale * translate;

            effect.View = camera.View;
            effect.Projection = camera.Projection;
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.SetVertexBuffer(shipVertexBuffer);
                device.DrawPrimitives(
                PrimitiveType.TriangleList,
                0,
                shipVertexBuffer.VertexCount / 3);

            }
        }
        public void Update(float elapsedtime)
        {
        //    rotation = MathHelper.WrapAngle(rotation + 0.017f);
        //    zrotation = MathHelper.WrapAngle(zrotation + 0.015f);
            
        }
    }
}
//<><>