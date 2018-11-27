using System;
using System.Linq;
using Urho;
using Urho.Shapes;

namespace WhackABox
{
    public partial class Game : Application
    {
        private Scene scene;
        private Camera camera;
        private Viewport viewport;
        private static Random random = new Random();
        private float newBoxTtl;
        private readonly float newBoxIntervalInSeconds = 2;

        public Game(ApplicationOptions options) : base(options)
        {
        }

        private void CreateSubPlane(PlaneNode planeNode)
        {
            var node = planeNode.CreateChild("subplane");
            node.Position = new Vector3(0, 0.05f, 0);

            var box = node.CreateComponent<Box>();
            box.Color = Color.FromHex("#22ff0000");
        }

        private void UpdateSubPlane(PlaneNode planeNode, Vector3 position)
        {
            var subPlaneNode = planeNode.GetChild("subplane");
            subPlaneNode.Scale = new Vector3(planeNode.ExtentX, 0.05f, planeNode.ExtentZ);
            subPlaneNode.Position = position;
        }

        private PlaneNode FindNodeByPlaneId(string planeId) =>
                    scene.Children.OfType<PlaneNode>()
                    .FirstOrDefault(e => e.PlaneId == planeId);

        private void InitializeCamera()
        {
            var cameraNode = scene.CreateChild("Camera");
            camera = cameraNode.CreateComponent<Camera>();
        }

        private void InitializeRenderer()
        {
            viewport = new Viewport(Context, scene, camera, null);
            Renderer.SetViewport(0, viewport);
        }

        private void InitializeLights()
        {
            var lightNode = camera.Node.CreateChild();
            lightNode.SetDirection(new Vector3(1f, -1.0f, 1f));
            var light = lightNode.CreateComponent<Light>();
            light.Range = 10;
            light.LightType = LightType.Directional;
            light.CastShadows = true;
            Renderer.ShadowMapSize *= 4;
        }

        protected override void Start()
        {
            scene = new Scene(Context);
            scene.NodeAdded += (e) => SendStats();
            scene.NodeRemoved += (e) => SendStats();
            var octree = scene.CreateComponent<Octree>();

            InitializeCamera();
            InitializeLights();
            InitializeRenderer();

            Input.TouchBegin += OnTouchBegin;

            InitializeAR();
        }

        private void AddBox(PlaneNode planeNode)
        {
            var subPlaneNode = planeNode.GetChild("subplane");

            var boxNode = planeNode.CreateChild("Box");
            boxNode.SetScale(0.1f);

            var x = planeNode.ExtentX * (float)(random.NextDouble() - 0.5f);
            var z = planeNode.ExtentZ * (float)(random.NextDouble() - 0.5f);

            boxNode.Position = new Vector3(x, 0.1f, z) + subPlaneNode.Position;

            var box = boxNode.CreateComponent<Box>();
            box.Color = Color.Blue;

            var rotationSpeed = new Vector3(10.0f, 20.0f, 30.0f);
            var rotator = new Rotator() { RotationSpeed = rotationSpeed };
            boxNode.AddComponent(rotator);
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            newBoxTtl -= timeStep;

            if (newBoxTtl < 0)
            {
                foreach (var node in scene.Children.OfType<PlaneNode>())
                {
                    AddBox(node);
                }

                newBoxTtl += newBoxIntervalInSeconds;
            }
        }

        private void DetermineHit(float x, float y)
        {
            var cameraRay = camera.GetScreenRay(x, y);
            var result = scene.GetComponent<Octree>().RaycastSingle(cameraRay);

            if (result?.Node?.Name?.StartsWith("Box") == true)
            {
                var node = result?.Node;

                if (node.Components.OfType<Death>().Any())
                {
                    return;
                }

                node.CreateComponent<Death>();
            }
        }

        private void OnTouchBegin(TouchBeginEventArgs e)
        {
            var x = (float)e.X / Graphics.Width;
            var y = (float)e.Y / Graphics.Height;

            DetermineHit(x, y);
        }

        private void SendStats()
        {
            var planes = scene.Children.OfType<PlaneNode>();
            var boxCount = 0;

            foreach (var plane in planes)
            {
                boxCount += plane.Children.Count(e => e.Name == "Box");
            }

            var stats = new GameStats()
            {
                NumberOfBoxes = boxCount,
                NumberOfPlanes = planes.Count()
            };

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                Xamarin.Forms.MessagingCenter.Send(this, "stats_updated", stats);
            });
        }
    }
}