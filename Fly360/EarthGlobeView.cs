using System;
using System.Diagnostics;
using Urho;
using Urho.Actions;
using Urho.Shapes;
using Xamarin.Forms;

namespace Fly360
{
    public class EarthGlobeView : Urho.Application
    {
        Scene scene;
        Node earthNode;
        Camera camera;
        Octree octree;
        DateTime _lastTrackedInput;
        bool _isPaused = false;

        [Preserve]
        public EarthGlobeView(ApplicationOptions options = null) : base(options) { }

        static EarthGlobeView()
        {
            UnhandledException += (s, e) =>
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
                
                e.Handled = true;
            };
        }

        protected override void Start()
        {
            base.Start();
            CreateScene();
            SetupViewport();

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (_isPaused && (DateTime.Now - _lastTrackedInput) > TimeSpan.FromSeconds(1))
                {
                    AutoRotate();
                }
                return true;
            });
        }

        void CreateScene()
        {
            scene = new Scene();
            octree = scene.CreateComponent<Octree>();

            earthNode = scene.CreateChild(name: "Earth");
            var earth = earthNode.CreateComponent<Sphere>();
            earth.Color = Urho.Color.Blue;
            earthNode.SetScale(4f);
            earth.SetMaterial(Material.FromImage("earth.jpg"));
            AutoRotate();

            var cameraNode = scene.CreateChild();
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3((float)9.5, (float)9.90, 10) / 1.75f;
            cameraNode.Rotation = new Quaternion(-0.121f, 0.878f, -0.305f, -0.35f);

            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 100;
            light.Brightness = 1.3f;
        }

        private bool AutoRotate()
        {
            _isPaused = false;
            try
            {
                earthNode.RunActions(new RepeatForever(
                            new RotateBy(
                                duration: 1f,
                                deltaAngleX: 0,
                                deltaAngleY: -15,
                                deltaAngleZ: 0)));
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void OnUpdate(float timeStep)
        {
            try
            {
                if (Input.NumTouches >= 1)
                {
                    _lastTrackedInput = DateTime.Now;
                    _isPaused = true;

                    var touch = Input.GetTouch(0);
                    earthNode.RemoveAllActions();
                    earthNode.Rotate(new Quaternion(0, -touch.Delta.X / 3, 0), TransformSpace.Local);
                }
                base.OnUpdate(timeStep);
            }
            catch
            {
                
            }
        }

        public void Rotate(float toValue)
        {
            earthNode.Rotate(new Quaternion(0, toValue, 0), TransformSpace.Local);
        }

        void SetupViewport()
        {
            var renderer = Renderer;
            var vp = new Viewport(Context, scene, camera, null);
            renderer.SetViewport(0, vp);
        }
    }
}

