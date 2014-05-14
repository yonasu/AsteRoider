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
        private IndexBuffer indexbuffer;
        private GraphicsDevice device;
        private Texture2D texture;
        private Vector3 location;


        private float rotation = 0f;
        private float zrotation = 0f;

        private VertexBuffer shipVertexBuffer;
        private List<VertexPositionNormalTexture> vertices = new List<VertexPositionNormalTexture>();

        public Ship(GraphicsDevice graphicsdev, Texture2D texture)
        {
            this.texture = texture;
            device = graphicsdev;
            PositionShip(Vector3.Zero, 0.0f);

            //FAN-TRIANGLE NOPE THERE IS NO FAN MOFO

            vertices.Add(BuildVertex(0, 1, 0, 0, 1));  // 1
            vertices.Add(BuildVertex(1, -1, 0, 1, -1));  // 2
            vertices.Add(BuildVertex(-1, -1, 0, -1, -1)); // 3
            vertices.Add(BuildVertex(0, 0, 0.25f, 0, 0));   // 4


            BuildIndexBuffer();
            shipVertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), vertices.Count, BufferUsage.None);
            shipVertexBuffer.SetData<VertexPositionNormalTexture>(vertices.ToArray());
            CalculateNormals();
            
        }

        private void BuildIndexBuffer()
        {
            int indexCount = 12;
            short[] indices = new short[indexCount];
            //for (int i = 1; i <= 11; i++)
            //{
            //    indices[i-1] = (short)i;
            //}
            indices[0] = (short)3;
            indices[1] = (short)1;
            indices[2] = (short)2;
            indices[3] = (short)3;
            indices[4] = (short)2;
            indices[5] = (short)0;
            indices[6] = (short)3;
            indices[7] = (short)0;
            indices[8] = (short)1;
            indices[9] = (short)1;
            indices[10] = (short)0;
            indices[11] = (short)2;

            indexbuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            indexbuffer.SetData(indices);
        }

        public void PositionShip(Vector3 playerLocation, float minDistance)
        {
            location = new Vector3(1.5f, 0.5f, 1.5f);
        }
        private void BuildTripleFace(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            vertices.Add(BuildVertex(p1.X, p1.Y, p1.Z, 0, 1));
        }
        private VertexPositionNormalTexture BuildVertex(float x1, float y1, float z1, int x2, int y2)
        {
            return new VertexPositionNormalTexture(new Vector3(x1, y1, z1),new Vector3(0,0,0), new Vector2(x2, y2));
        }
        private void CalculateNormals()
        {
            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[shipVertexBuffer.VertexCount];
            short[] indices = new short[indexbuffer.IndexCount];
            shipVertexBuffer.GetData(vertices);
            indexbuffer.GetData(indices);
            for (int x = 0; x < vertices.Length; x++)
            {
                vertices[x].Normal = Vector3.Zero;
            }
            int triangleCount = indices.Length / 3;
            for (int x = 0; x < triangleCount; x++)
            {
                int v1 = indices[x * 3];
                int v2 = indices[(x * 3) + 1];
                int v3 = indices[(x * 3) + 2];
                Vector3 firstSide = vertices[v2].Position - vertices[v1].Position;
                Vector3 secondSide = vertices[v1].Position - vertices[v3].Position;
                Vector3 triangleNormal =
                Vector3.Cross(firstSide, secondSide);
                triangleNormal.Normalize();
                vertices[v1].Normal += triangleNormal;
                vertices[v2].Normal += triangleNormal;
                vertices[v3].Normal += triangleNormal;
            }
            for (int x = 0; x < vertices.Length; x++)
                vertices[x].Normal.Normalize();
            shipVertexBuffer.SetData(vertices);
        }
        public void Draw(Camera camera, Effect effect)
        {
            //vart skeppet befinner sig         
            Matrix rot = Matrix.CreateRotationY(rotation);
            Matrix zrot = Matrix.CreateRotationZ(rotation);
            Matrix center = Matrix.CreateTranslation(new Vector3(-0.5f, -0.5f, -0.5f));
            Matrix scale = Matrix.CreateScale(0.5f);
            Matrix translate = Matrix.CreateTranslation(location);
            Matrix total = rot * center * scale * translate * zrot;

            //HLSL Grejjerna
            effect.CurrentTechnique = effect.Techniques["MegaRenderManiacStreetStyle"];
            effect.Parameters["World"].SetValue(total);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["shipTexture1"].SetValue(texture);
            Vector3 lightDirection = new Vector3(-1f, 1f, -1f);
            lightDirection.Normalize();
            effect.Parameters["lightDirection"].SetValue(lightDirection);
            effect.Parameters["lightColor"].SetValue(new Vector4(1, 1, 1, 1));
            effect.Parameters["lightBrightness"].SetValue(0.8f);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.Indices = indexbuffer;
                device.SetVertexBuffer(shipVertexBuffer);
                device.DrawIndexedPrimitives(
                PrimitiveType.TriangleList, 0,
                0,
                shipVertexBuffer.VertexCount, 0, indexbuffer.IndexCount / 3);

            }
        }

        public void Update(float elapsedtime)
        {
            rotation = MathHelper.WrapAngle(rotation + 0.017f);
            zrotation = MathHelper.WrapAngle(zrotation + 0.015f);

        }
       
    }
}
//<><>