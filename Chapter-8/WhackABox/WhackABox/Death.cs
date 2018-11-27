using Urho;
using System;

namespace WhackABox
{
    public class Death : Component
    {
        private float deathTtl = 1f;
        private float initialScale = 1;

        public Action OnDeath { get; set; }

        public Death()
        {
            ReceiveSceneUpdates = true;
        }

        public override void OnAttachedToNode(Node node)
        {
            initialScale = node.Scale.X;
        }

        protected override void OnUpdate(float timeStep)
        {
            Node.SetScale(deathTtl * initialScale);

            if (deathTtl < 0)
            {
                Node.Remove();
            }

            deathTtl -= timeStep;
        }
    }
}