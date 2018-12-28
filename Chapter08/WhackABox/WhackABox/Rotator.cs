using Urho;

namespace WhackABox
{
    public class Rotator : Component
    {
        public Vector3 RotationSpeed { get; set; }

        public Rotator()
        {
            ReceiveSceneUpdates = true;
        }

        protected override void OnUpdate(float timeStep)
        {
            Node.Rotate(new Quaternion(
                RotationSpeed.X * timeStep,
                RotationSpeed.Y * timeStep,
                RotationSpeed.Z * timeStep),
                TransformSpace.Local);
        }
    }
}