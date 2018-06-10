using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SkiaSharp;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Shapes;
using Urho.Urho2D;
using Xamarin.Forms;

namespace Fly360
{
    public class EarthGlobeView : Urho.Application
    {
        Scene scene;
        Node rootNode;
        Node cameraNode;
        Node selectedNode;
        Camera camera;
        Octree octree;
        DateTime _lastTrackedInput;
        bool _isPaused = false;
        bool _isBusy = false;

        public event EventHandler<string> CitySelected;

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
                if (_isPaused && (DateTime.Now - _lastTrackedInput) > TimeSpan.FromSeconds(3))
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

            rootNode = scene.CreateChild(name: "Root");

            var earthNode = rootNode.CreateChild(name: "Earth");
            var earth = earthNode.CreateComponent<Sphere>();
            earth.Color = Urho.Color.Blue;
            earthNode.SetScale(4.5f);
            earth.SetMaterial(Material.FromImage("earth3.jpg"));
            earthNode.Rotate(new Quaternion(x: 2f, y: -0.75f, z: 0), TransformSpace.Local);

            cameraNode = scene.CreateChild();
            camera = cameraNode.CreateComponent<Camera>();
            cameraNode.Position = new Vector3((float)9.5, (float)9.90, 10) / 1.75f;
            cameraNode.Rotation = new Quaternion(-0.121f, 0.878f, -0.305f, -0.35f);

            Node lightNode = cameraNode.CreateChild();
            var light = lightNode.CreateComponent<Light>();
            light.LightType = LightType.Point;
            light.Range = 100;
            light.Brightness = 1.3f;

            var markersNode = rootNode.CreateChild(name: "Markers");

            var positions = new Dictionary<string, float[]> {
                { "hnd", new float[] { 35.549140f, 139.780053f }},//tokyo
                { "ams", new float[] { 52.309876f, 4.768231f } },//amsterdam
                { "del", new float[] { 28.639446f, 77.123433f } },//delhi
                { "dab", new float[] { 29.182539f, -81.053463f } },//daytona
                { "eyw", new float[] { 24.555340f, -81.757469f } },//keywest
                { "fll", new float[] { 26.072924f, -80.151160f } },//fort lautrade
                { "gcn", new float[] { 35.951838f, -112.144504f } },//grand canyon
                { "hnl", new float[] { 21.323464f, -157.925074f } },//honulu
                { "iad", new float[] { 38.952591f, -77.455981f } },//washington
                { "las", new float[] { 36.083722f, -115.153288f } },//las vegas
                { "lax", new float[] { 33.940965f, -118.408637f } },//los angeles
                { "myr", new float[] { 33.682854f, -78.928141f } },//myrtle
                { "nyc", new float[] { 40.729441f, -73.969534f } },//new york
                { "ord", new float[] { 41.973764f, -87.907429f } },//orlando
                { "san", new float[] { 32.733358f, -117.193808f } },//san diego
                { "sfo", new float[] { 37.620752f, -122.380050f } },//san fransico
                { "snu", new float[] { 22.491947f, -79.945303f } },//santa calra
                { "vps", new float[] { 30.494920f, -86.550364f } },//destin
                { "mes", new float[] { 37.215442f, -108.458600f } },//mesa verde
                { "den", new float[] { 39.855932f, -104.673233f } },//denver
                { "ase", new float[] { 39.219123f, -106.864559f } },//aspen
            };

            var colors = new Urho.Color[] {
                Urho.Color.FromHex("#f3b50c"),
                Urho.Color.FromHex("#4ab075"),
                Urho.Color.FromHex("#f0399d")
            };

            foreach (var row in positions.Keys)
                AddMarker(markersNode,
                          lat: positions[row][0],
                          lon: positions[row][1],
                          id: row,
                          color: colors.GetRandomElement());

            AutoRotate();
        }

        Node AddMarker(Node parent, float lat, float lon, string id, Urho.Color color)
        {
            var markerNode = parent.CreateChild("MarkerRoot_" + id);
            var markerHeadNode = markerNode.CreateChild("MarkerHeadModel_" + id);
            var markerTailNode = markerNode.CreateChild("MarkerTailModel_" + id);

            var pinCone = markerTailNode.CreateComponent<Urho.Shapes.Cone>();
            markerTailNode.Scale = new Vector3(0.045f, 0.09f, 0.045f);
            pinCone.Color = color;

            var pinHead = markerHeadNode.CreateComponent<Urho.Shapes.Sphere>();
            markerHeadNode.SetScale(0.1f);
            //pinHead.SetMaterial(Material.FromImage(id + ".jpg"));
            pinHead.Color = color;

            GetPositionForHeight(lat, lon, 2.25f, out double x1, out double y1, out double z1);
            markerTailNode.Position = new Vector3((float)x1, (float)y1, (float)z1);
            markerTailNode.LookAt(new Vector3(0, 0, 0), new Vector3(0, 1, 0),
                   TransformSpace.Parent);
            markerTailNode.Rotate(new Quaternion(90, 0, 0), TransformSpace.Local);

            GetPositionForHeight(lat, lon, 2.3f, out double x2, out double y2, out double z2);
            markerHeadNode.Position = new Vector3((float)x2, (float)y2, (float)z2);
            return markerNode;
        }

        private void SetTextAsMaterial(string id, string txt, Node node, Shape shape)
        {
            // Load image to SkiaSharp
            // load the image from the embedded resource stream

            var url = string.Format("Fly360.images.Data.{0}.jpg", id);

            using (var resource = this.GetType().Assembly.GetManifestResourceStream(url))
            using (var stream = new SKManagedStream(resource))
            using (var bitmap = SKBitmap.Decode(stream))
            {
                var canvas = new SKCanvas(bitmap);
                canvas.DrawBitmap(bitmap, 0, 0);
                canvas.ResetMatrix();
                var brush = new SKPaint
                {
                    TextSize = 55,
                    IsAntialias = true,

                    Color = SKColors.Yellow
                };
                canvas.DrawText(txt, bitmap.Width / 7f, bitmap.Height / 1.4f, brush);
                var brush2 = new SKPaint
                {
                    TextSize = 55,
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = 1,
                    FakeBoldText = true,
                    Color = SKColors.Black
                };
                canvas.DrawText(txt, bitmap.Width / 7f, bitmap.Height / 1.4f, brush2);
                canvas.Flush();
                var image = SKImage.FromBitmap(bitmap);
                var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);
                var skiaImgBytes = data.ToArray();

                // Create UrhoSharp Texture2D
                Texture2D text = new Texture2D();
                text.Load(new MemoryBuffer(skiaImgBytes));

                var material = new Material();
                material.SetTexture(TextureUnit.Diffuse, text);
                material.SetTechnique(0, CoreAssets.Techniques.Diff, 0, 0);
                shape.SetMaterial(material);
            }
        }

        private void GetPositionForHeight(float lat, float lon, float height, out double x, out double y, out double z)
        {
            var latrad = lat * Math.PI / 180;
            var lonrad = lon * Math.PI / 180;
            x = height * Math.Cos(latrad) * Math.Cos(lonrad);
            y = height * Math.Sin(latrad);
            z = height * Math.Cos(latrad) * Math.Sin(lonrad);
        }

        private bool AutoRotate()
        {
            _isPaused = false;
            try
            {
                ResetSelectedNode();
                if (rootNode.Scale.X > 1f)
                    rootNode.RunActions(new ScaleTo(0.7f, 1f));

                rootNode.RunActions(new RepeatForever(
                            new RotateBy(
                                duration: 5f,
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
            if (_isBusy)
                return;

            try
            {
                _isBusy = true;
                if (Input.NumTouches >= 1)
                {
                    _lastTrackedInput = DateTime.Now;
                    _isPaused = true;

                    rootNode.RemoveAllActions();
                    var touch = Input.GetTouch(0);


                    var cameraRay = camera.GetScreenRay(
                        (float)touch.Position.X / Graphics.Width,
                        (float)touch.Position.Y / Graphics.Height);

                    var result = octree.RaycastSingle(cameraRay,
                        RayQueryLevel.Triangle, 100, DrawableFlags.Geometry);

                    if (result != null)
                    {
                        var node = result.Value.Node;
                        if (node == selectedNode && selectedNode.Scale.X >= 0.5f)
                        {
                            ResetSelectedNode();

                            var id = node.Name.Replace("MarkerHeadModel_", "");
                            CitySelected?.Invoke(this, id);

                            return;
                        }

                        ResetSelectedNode();
                        if (node.Name.StartsWith("Marker"))
                        {

                            if (node.Parent != null && node.Parent.Name.StartsWith("MarkerRoot"))
                            {
                                node = node.Parent;
                                node = node.Children.First(n => n.Name.StartsWith("MarkerHead"));
                            }
                            if (node.Name.StartsWith("MarkerHead"))
                            {
                                selectedNode = node;

                                var id = node.Name.Replace("MarkerHeadModel_", "");

                                var box = node.CreateComponent<Box>();
                                SetTextAsMaterial(id, id.ToUpper(), node, box);
                                node.RunActions(new EaseElasticOut(new ScaleTo(0.7f, 0.5f)));
                            }

                            return;
                        }
                        else if (node.Name.Equals("Earth"))
                        {
                            if (node.Parent != null && node.Parent.Name.Equals("Root"))
                            {
                                node = node.Parent;
                                if (node.Scale.X < 2f)
                                    node.RunActions(new ScaleTo(0.7f, 2f));
                            }

                        }
                    }

                    //else we move the globe
                    var x = Math.Abs(touch.Delta.X);
                    var y = Math.Abs(touch.Delta.Y);

                    //if (x < y)
                    //    rootNode.Rotate(new Quaternion(0, 0, touch.Delta.Y / 2), TransformSpace.Local);
                    //else
                    rootNode.Rotate(new Quaternion(0, -touch.Delta.X / 3, 0), TransformSpace.Local);
                }

                var markersNode = rootNode.Children.First(n => n.Name == "Markers");
                foreach (var mR in markersNode.Children)
                {
                    var head = mR.Children.First(n => n.Name.StartsWith("MarkerHead"));
                    head.LookAt(cameraNode.WorldPosition, Vector3.UnitY, TransformSpace.World);
                }

                base.OnUpdate(timeStep);
            }
            catch
            {

            }
            finally
            {
                _isBusy = false;
            }
        }

        private async void ResetSelectedNode()
        {
            if (selectedNode != null)
            {
                selectedNode.RemoveAllActions();
                selectedNode.RunActions(new EaseElasticIn(new ScaleTo(0.05f, 0.1f)));

                await Task.Delay(500);
                var box = selectedNode.GetComponent<Box>();
                selectedNode.RemoveComponent(box);

                selectedNode = null;
            }
        }

        void SetupViewport()
        {
            var renderer = Renderer;
            var vp = new Viewport(Context, scene, camera, null);
            renderer.SetViewport(0, vp);
        }
    }
}

