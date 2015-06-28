using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using OpenTK;
using ThreeAPI.scene;
using ThreeAPI.scene.nodes;

namespace ThreeAPI.Test.scene
{
  [TestFixture()]
  public class BaseSceneNodeTest
  {
    [Test()]
    public void ComposeTransform_NoChilds_TransformIsIdentity()
    {
      var baseSceneNode = new BaseSceneNode();
      Assert.AreEqual(Matrix4.Identity, baseSceneNode.ComposeTransform());

    }

    [Test()]
    public void ComposeTransform_OneAncestor_TransformIsMultiplicationOfChildAndParent()
    {
      var baseSceneNode = new BaseSceneNode();
      var children = new List<ISceneNode> {baseSceneNode};

      var parentMock = new Mock<ISceneNode>();
      parentMock.SetupGet(x => x.Children).Returns(children);
      parentMock.Setup(x => x.ComposeTransform()).Returns(Matrix4.CreateScale(2));

      baseSceneNode.Parent = parentMock.Object;
      
      Assert.AreEqual(Matrix4.CreateScale(2), baseSceneNode.ComposeTransform());

    }

    [Test()]
    public void GetElements_NoElementsOfType_ReturnsEmptyList()
    {
      var baseSceneNode = new BaseSceneNode();
      var elements = baseSceneNode.GetNodes<CameraNode>();
      Assert.AreEqual(0, elements.Count());
    }

    [Test()]
    public void GetElements_OnlyTwoElementsOfTypeInGraph_ReturnsListWithTwoElements()
    {
      var baseSceneNode = new BaseSceneNode();
      var child1 = new PerspectiveCamera();
      var child2 = new OrtographicCamera();
      baseSceneNode.AddChild(child1);
      baseSceneNode.AddChild(child2);
      var elements = baseSceneNode.GetNodes<CameraNode>();
      Assert.AreEqual(2, elements.Count());
    }

    [Test()]
    public void GetElements_TwoElementsOfTypeInGraphAndAlsoOtherElementsPresent_ReturnsListWithTwoElements()
    {
      var baseSceneNode = new BaseSceneNode();
      var child1 = new PerspectiveCamera();
      var child2 = new BaseSceneNode();
      var child1ofChild1 = new OrtographicCamera();
      var child2ofChild1 = new ScaleNode(1,1,1);
      
      baseSceneNode.AddChild(child1);
      baseSceneNode.AddChild(child2);
      child1.AddChild(child1ofChild1);
      child1.AddChild(child2ofChild1);
      var elements = baseSceneNode.GetNodes<CameraNode>();
      Assert.AreEqual(2, elements.Count());
    }

  }
}
