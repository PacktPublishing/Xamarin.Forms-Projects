#if __IOS__
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ARKit;
using Urho;
using Urho.iOS;

namespace WhackABox
{
    public partial class Game
    {
        private ARKitComponent arkitComponent;

        private void SetPositionAndRotation(ARPlaneAnchor anchor, PlaneNode node)
        {
            arkitComponent.ApplyOpenTkTransform(node, anchor.Transform, true);

            node.ExtentX = anchor.Extent.X;
            node.ExtentZ = anchor.Extent.Z;

            var position = new Vector3(anchor.Center.X, anchor.Center.Y, -anchor.Center.Z);
            UpdateSubPlane(node, position);
        }

        private void UpdateOrAddPlaneNode(ARPlaneAnchor anchor)
        {
            var node = FindNodeByPlaneId(anchor.Identifier.ToString());

            if (node == null)
            {
                node = new PlaneNode()
                {
                    PlaneId = anchor.Identifier.ToString(),
                    Name = $"plane{anchor.GetHashCode()}"
                };

                CreateSubPlane(node);
                scene.AddChild(node);
            }

            SetPositionAndRotation(anchor, node);
        }

        private void OnAddAnchor(ARAnchor[] anchors)
        {
            foreach (var anchor in anchors.OfType<ARPlaneAnchor>())
            {
                UpdateOrAddPlaneNode(anchor);
            }
        }

        private void OnUpdateAnchors(ARAnchor[] anchors)
        {
            foreach (var anchor in anchors.OfType<ARPlaneAnchor>())
            {
                UpdateOrAddPlaneNode(anchor);
            }
        }

        private void OnRemoveAnchors(ARAnchor[] anchors)
        {
            foreach (var anchor in anchors.OfType<ARPlaneAnchor>())
            {
                FindNodeByPlaneId(anchor.Identifier.ToString())?.Remove();
            }
        }

        private void InitializeAR()
        {
            arkitComponent = scene.CreateComponent<ARKitComponent>();
            arkitComponent.Orientation = UIKit.UIInterfaceOrientation.Portrait;
            arkitComponent.ARConfiguration = new ARWorldTrackingConfiguration
            {
                PlaneDetection = ARPlaneDetection.Horizontal
            };
            arkitComponent.DidAddAnchors += OnAddAnchor;
            arkitComponent.DidUpdateAnchors += OnUpdateAnchors;
            arkitComponent.DidRemoveAnchors += OnRemoveAnchors;
            arkitComponent.RunEngineFramesInARKitCallbakcs = Options.DelayedStart;
            arkitComponent.Run();
        }
    }
}
#endif