#if __ANDROID__
using Com.Google.AR.Core;
using Urho;
using Urho.Droid;
 
namespace WhackABox
{
    public partial class Game
    {
        private ARCoreComponent arCore;

        private void SetPositionAndRotation(Com.Google.AR.Core.Plane plane, PlaneNode node)
        {
            node.ExtentX = plane.ExtentX;
            node.ExtentZ = plane.ExtentZ;
            node.Rotation = new Quaternion(plane.CenterPose.Qx(),
                                           plane.CenterPose.Qy(),
                                           plane.CenterPose.Qz(),
                                           -plane.CenterPose.Qw());

            node.Position = new Vector3(plane.CenterPose.Tx(),
                                        plane.CenterPose.Ty(),
                                        -plane.CenterPose.Tz());
        }

        private void OnARFrameUpdated(Frame arFrame)
        {
            var all = arCore.Session.GetAllTrackables(
                          Java.Lang.Class.FromType(
                          typeof(Com.Google.AR.Core.Plane)));

            foreach (Com.Google.AR.Core.Plane plane in all)
            {
                var node = FindNodeByPlaneId(plane.GetHashCode().ToString());

                if (node == null)
                {
                    node = new PlaneNode
                    {
                        PlaneId = plane.GetHashCode().ToString(),
                        Name = $"plane{plane.GetHashCode()}"
                    };

                    CreateSubPlane(node);
                    scene.AddChild(node);
                }

                SetPositionAndRotation(plane, node);
                UpdateSubPlane(node, Vector3.Zero);
            }
        }

        private void OnConfigRequested(Config config)
        {
            config.SetPlaneFindingMode(Config.PlaneFindingMode.Horizontal);
            config.SetLightEstimationMode(Config.LightEstimationMode.AmbientIntensity);
            config.SetUpdateMode(Config.UpdateMode.LatestCameraImage);
        }

        private void InitializeAR()
        {
            arCore = scene.CreateComponent<ARCoreComponent>();
            arCore.ARFrameUpdated += OnARFrameUpdated;
            arCore.ConfigRequested += OnConfigRequested;
            arCore.Run();
        }
    }
}
#endif