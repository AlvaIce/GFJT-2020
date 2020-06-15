//----------------------------------------------------------------------------
// NAME: Sky
// VERSION: 1.3
// DESCRIPTION: Renders a textured sky dome above the camera. Adds itself as a layer in Layer Manager (key: L). Right click on layer for settings.
// DEVELOPER: Patrick Murris
// WEBSITE: http://www.alpix.com/3d/worldwin
//----------------------------------------------------------------------------
// Based on Bjorn Reppen 'Atmosphere' plugin
// 1.3 Nov   5, 2006	WW 1.4 : added 'recenter' fix in Render()
// 1.2 Nov  13, 2005	uncommented mashi's clipping code, raised trigger alt to 200Km
// 1.1 June 18, 2005	Settings dialog, bitmap select, read/save settings
// 1.0 June 15, 2005	First version, one bitmap
//----------------------------------------------------------------------------

using System;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using QRST.WorldGlobeTool.Utility;
using System.Windows.Forms;
using System.IO;
using QRST.WorldGlobeTool.Camera;

namespace QRST.WorldGlobeTool.Renderable
{
    /// <summary>
    /// Sky dome
    /// </summary>
    public class SkyLayer : RenderableObject
    {

        string pluginPath;
        public World world;
        public DrawArgs drawArgs;
        Texture texture;

        // default sky bitmap
        public string textureFileName = "Sky_Day1.jpg";

        /// <summary>
        /// Constructor
        /// </summary>
        public SkyLayer(string LayerName, QRSTWorldGlobeControl globe)
            : base(LayerName)
        {
            this.pluginPath = Path.Combine(globe.DataDirectory, @"Space");
            this.world = globe.CurrentWorld;
            this.drawArgs = globe.DrawArgs;
            this.RenderPriority = RenderPriority.SurfaceImages;
        }


        #region RenderableObject

        /// <summary>
        /// This is where we do our rendering 
        /// Called from UI thread = UI code safe in this function
        /// </summary>
        public override void Render(DrawArgs drawArgs)
        {
            if (!IsInitialized)
                Initialize(drawArgs);

            // Camera & Device shortcuts ;)
            CameraBase camera = drawArgs.WorldCamera;
            Device device = drawArgs.device;

            if (camera.Altitude > 200e3) return;

            double distToCenterOfPlanet = (camera.Altitude + camera.WorldRadius);
            double tangentalDistance = Math.Sqrt(distToCenterOfPlanet * distToCenterOfPlanet - camera.WorldRadius * camera.WorldRadius);
            double domeRadius = tangentalDistance * 0.5;
            double domeToCenterOfPlanet = Math.Sqrt(camera.WorldRadius * camera.WorldRadius - domeRadius * domeRadius) - 2e3;

            // Create sky dome
            Mesh skyMesh = TexturedDome(device, (float)domeRadius, 24, 12);
            // set texture
            device.SetTexture(0, texture);
            device.TextureState[0].ColorOperation = TextureOperation.BlendCurrentAlpha;
            drawArgs.device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
            drawArgs.device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;
            drawArgs.device.TextureState[0].AlphaOperation = TextureOperation.SelectArg1;
            drawArgs.device.TextureState[0].AlphaArgument1 = TextureArgument.TextureColor;
            device.VertexFormat = CustomVertex.PositionTextured.Format;

            // save world and projection transform
            Matrix origWorld = device.Transform.World;
            Matrix origProjection = device.Transform.Projection;

            // move sky dome
            Matrix skyTrans;
            Angle camLat = camera.Latitude;
            Angle camLon = camera.Longitude;
            //Vector3 groundPos = MathEngine.SphericalToCartesian(camLat, camLon, this.world.EquatorialRadius);
            skyTrans = Matrix.Translation(0, 0, (float)domeToCenterOfPlanet);
            skyTrans = Matrix.Multiply(skyTrans, Matrix.RotationY(-(float)camLat.Radians + (float)Math.PI / 2));
            skyTrans = Matrix.Multiply(skyTrans, Matrix.RotationZ((float)camLon.Radians));

            device.Transform.World = skyTrans;

            // Recenter world
            Recenter(drawArgs);

            // Save fog status
            bool origFog = device.RenderState.FogEnable;
            device.RenderState.FogEnable = false;

            // Set new one (to avoid being clipped) - probably better ways of doing this?
            float aspectRatio = (float)device.Viewport.Width / device.Viewport.Height;
            device.Transform.Projection = Matrix.PerspectiveFovRH((float)camera.Fov.Radians, aspectRatio, 1, float.MaxValue);

            // draw
            skyMesh.DrawSubset(0);

            // Restore device states
            device.Transform.World = origWorld;
            device.Transform.Projection = origProjection;
            device.RenderState.FogEnable = origFog;
            // dispose of sky - for now
            skyMesh.Dispose();
        }

        // Recenter world projection in WW 1.4
        public void Recenter(DrawArgs drawArgs)
        {
            drawArgs.device.Transform.World *= Matrix.Translation(
                (float)-drawArgs.WorldCamera.ReferenceCenter.X,
                (float)-drawArgs.WorldCamera.ReferenceCenter.Y,
                (float)-drawArgs.WorldCamera.ReferenceCenter.Z
                );
        }

        /// <summary>
        /// RenderableObject abstract member (needed) 
        /// OBS: Worker thread (don't update UI directly from this thread)
        /// </summary>
        public override void Initialize(DrawArgs drawArgs)
        {
            try
            {
                texture = TextureLoader.FromFile(drawArgs.device, Path.Combine(pluginPath, textureFileName));
                IsInitialized = true;
            }
            catch
            {
                isOn = false;
                MessageBox.Show("Error loading texture " + Path.Combine(pluginPath, textureFileName) + ".", "Layer initialization failed.", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// RenderableObject abstract member (needed)
        /// OBS: Worker thread (don't update UI directly from this thread)
        /// </summary>
        public override void Update(DrawArgs drawArgs)
        {
            if (!IsInitialized)
                Initialize(drawArgs);
        }

        /// <summary>
        /// RenderableObject abstract member (needed)
        /// OBS: Worker thread (don't update UI directly from this thread)
        /// </summary>
        public override void Dispose()
        {
            IsInitialized = false;
            if (texture != null)
            {
                texture.Dispose();
                texture = null;
            }
        }

        /// <summary>
        /// Gets called when user left clicks.
        /// RenderableObject abstract member (needed)
        /// Called from UI thread = UI code safe in this function
        /// </summary>
        public override bool PerformSelectionAction(DrawArgs drawArgs)
        {
            return false;
        }

        /// <summary>
        /// Fills the context menu with menu items specific to the layer.
        /// </summary>
        public override void BuildContextMenu(ContextMenu menu)
        {

        }



        /// <summary>
        /// Creates a PositionNormalTextured dome above X-Y plane
        /// </summary>
        /// <param name="device">The current direct3D drawing device.</param>
        /// <param name="radius">The dome's radius</param>
        /// <param name="slices">Number of slices (Horizontal resolution).</param>
        /// <param name="stacks">Number of stacs. (Vertical resolution)</param>
        /// <returns></returns>
        /// <remarks>
        /// Number of vertices in the dome will be (slices+1)*(stacks+1)<br/>
        /// Number of faces	:slices*stacks*2
        /// Number of Indexes	: Number of faces * 3;
        /// </remarks>
        private Mesh TexturedDome(Device device, float radius, int slices, int stacks)
        {
            int numVertices = (slices + 1) * (stacks + 1);
            int numFaces = slices * stacks * 2;
            int indexCount = numFaces * 3;

            Mesh mesh = new Mesh(numFaces, numVertices, MeshFlags.Managed, CustomVertex.PositionNormalTextured.Format, device);

            // Get the original sphere's vertex buffer.
            int[] ranks = new int[1];
            ranks[0] = mesh.NumberVertices;
            System.Array arr = mesh.VertexBuffer.Lock(0, typeof(CustomVertex.PositionNormalTextured), LockFlags.None, ranks);

            // Set the vertex buffer
            int vertIndex = 0;
            for (int stack = 0; stack <= stacks; stack++)
            {
                double latitude = (float)stack / stacks * (float)90.0;
                for (int slice = 0; slice <= slices; slice++)
                {
                    CustomVertex.PositionNormalTextured pnt = new CustomVertex.PositionNormalTextured();
                    double longitude = 180 - ((float)slice / slices * (float)360);
                    Vector3 v = MathEngine.SphericalToCartesian(latitude, longitude, radius);
                    pnt.X = v.X;
                    pnt.Y = v.Y;
                    pnt.Z = v.Z;
                    pnt.Tu = (float)slice / slices;
                    pnt.Tv = 1.0f - (float)stack / stacks;
                    arr.SetValue(pnt, vertIndex++);
                }
            }

            mesh.VertexBuffer.Unlock();
            ranks[0] = indexCount;
            arr = mesh.LockIndexBuffer(typeof(short), LockFlags.None, ranks);
            int i = 0;
            short bottomVertex = 0;
            short topVertex = 0;
            for (short x = 0; x < stacks; x++)
            {
                bottomVertex = (short)((slices + 1) * x);
                topVertex = (short)(bottomVertex + slices + 1);
                for (int y = 0; y < slices; y++)
                {
                    arr.SetValue(bottomVertex, i++);
                    arr.SetValue((short)(topVertex + 1), i++);
                    arr.SetValue(topVertex, i++);
                    arr.SetValue(bottomVertex, i++);
                    arr.SetValue((short)(bottomVertex + 1), i++);
                    arr.SetValue((short)(topVertex + 1), i++);
                    bottomVertex++;
                    topVertex++;
                }
            }
            mesh.IndexBuffer.SetData(arr, 0, LockFlags.None);
            mesh.ComputeNormals();

            return mesh;
        }


        #endregion
    }
}
