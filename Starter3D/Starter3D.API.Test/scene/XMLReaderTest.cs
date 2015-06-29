using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using OpenTK;
using ThreeAPI.geometry;
using ThreeAPI.geometry.factories;
using ThreeAPI.scene;
using ThreeAPI.scene.nodes;
using ThreeAPI.scene.nodes.factories;
using ThreeAPI.scene.persistence;
using ThreeAPI.scene.persistence.factories;
using ThreeAPI.utils;

namespace ThreeAPI.Test.scene
{
  [TestFixture()]
  public class XMLReaderTest
  {
    [Test()]
    public void ReadFile_NestedNodes_CorrectTypesStoredInSceneGraph()
    {
      var testXml = @"<Scene>
                        <Scale x='2' y='2' z='2'>
                           <Rotate x='1' y='0' z='0' angle='90'>
                            <Translate x='10' y='10' z='10'>
                            </Translate>
                           </Rotate>                        
                        </Scale>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);

      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");

      Assert.IsInstanceOf(typeof(BaseSceneNode), scene);
      Assert.IsInstanceOf(typeof(ScaleNode), scene.Children.First());
      Assert.IsInstanceOf(typeof(RotationNode), scene.Children.First().Children.First());
      Assert.IsInstanceOf(typeof(TranslationNode), scene.Children.First().Children.First().Children.First());
    }

    [Test()]
    public void ReadFile_OneScaleNode_CorrectParametersOfScaleNode()
    {
      var testXml = @"<Scene>
                        <Scale x='2' y='3' z='4'>                                                   
                        </Scale>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);

      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");

      var scaleNode = (ScaleNode)scene.Children.First();
      Assert.AreEqual(2.0, scaleNode.X);
      Assert.AreEqual(3.0, scaleNode.Y);
      Assert.AreEqual(4.0, scaleNode.Z);
    }

    [Test()]
    public void ReadFile_OneRotateNode_CorrectParametersOfRotateNode()
    {
      var testXml = @"<Scene>
                        <Rotate x='1' y='0' z='4' angle='90'>                                           
                        </Rotate>                      
                      </Scene>";
      File.WriteAllText("test.xml", testXml);

      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");

      var rotateNode = (RotationNode)scene.Children.First();
      Assert.AreEqual(1.0, rotateNode.X);
      Assert.AreEqual(0.0, rotateNode.Y);
      Assert.AreEqual(4.0, rotateNode.Z);
      Assert.AreEqual(90, rotateNode.Angle);
    }

    [Test()]
    public void ReadFile_OneTranslateNode_CorrectParametersOfTranslateNode()
    {
      var testXml = @"<Scene>
                        <Translate x='20' y='1' z='400'>                                                   
                        </Translate>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);

      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");

      var translationNode = (TranslationNode)scene.Children.First();
      Assert.AreEqual(20.0, translationNode.X);
      Assert.AreEqual(1.0, translationNode.Y);
      Assert.AreEqual(400.0, translationNode.Z);
    }

    [Test()]
    public void ReadFile_OneShapeNode_CorrectParametersOfShapeNode()
    {
      var testXml = @"<Scene>
                        <Shape shapeType='Mesh' filePath='test.obj' >                                                   
                        </Shape>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);

      var testObj = @"v 1.0 1.0 0.0
                      v 1.0 -1.0 0.0
                      v -1.0 -1.0 0.0
                      v -1.0 1.0 0.0
                      f 1 2 3
                      f 3 4 1
                    ";
      File.WriteAllText("test.obj", testObj);

      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");
      var shapeNode = (ShapeNode)scene.Children.First();
      Assert.IsInstanceOf(typeof(Mesh), shapeNode.Shape);
      Assert.AreEqual(4, (shapeNode.Shape as Mesh).Vertices.Count());
    }

    [Test()]
    public void ReadFile_OnePerspectiveCameraNode_CorrectParametersOfCameraNode()
    {
      var testXml = @"<Scene>
                        <PerspectiveCamera nearClip='0.01' farClip='100' order='1' fieldOfView='45' aspectRatio='1.33' >                                                   
                        </PerspectiveCamera>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);


      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");
      var perspectiveCameraNode = (PerspectiveCamera)scene.Children.First();
      Assert.AreEqual(0.01f, perspectiveCameraNode.NearClip);
      Assert.AreEqual(100, perspectiveCameraNode.FarClip);
      Assert.AreEqual(1, perspectiveCameraNode.Order);
      Assert.AreEqual(45.0f.ToRadians(), perspectiveCameraNode.FieldOfView);
      Assert.AreEqual(1.33f, perspectiveCameraNode.AspectRatio);
    }

    [Test()]
    public void ReadFile_OnePerspectiveCameraNodeNoOrderParameter_OrderEquals0()
    {
      var testXml = @"<Scene>
                        <PerspectiveCamera nearClip='0.01' farClip='100' fieldOfView='45' aspectRatio='1.33' >                                                   
                        </PerspectiveCamera>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);


      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");
      var perspectiveCameraNode = (PerspectiveCamera)scene.Children.First();
      Assert.AreEqual(0, perspectiveCameraNode.Order);
    }

    [Test()]
    public void ReadFile_OneOrthographicCameraNode_CorrectParametersOfCameraNode()
    {
      var testXml = @"<Scene>
                        <OrthographicCamera nearClip='0.01' farClip='100' order='1' width='150' height='200' >                                                   
                        </OrthographicCamera>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);


      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");
      var cameraNode = (OrtographicCamera)scene.Children.First();
      Assert.AreEqual(0.01f, cameraNode.NearClip);
      Assert.AreEqual(100, cameraNode.FarClip);
      Assert.AreEqual(1, cameraNode.Order);
      Assert.AreEqual(150, cameraNode.Width);
      Assert.AreEqual(200, cameraNode.Height);
    }

    [Test()]
    public void ReadFile_OnePointLightNode_CorrectParametersOfLightNode()
    {
      var testXml = @"<Scene>
                        <PointLight r='0.5' g='0.2' b='0.1' x='100' y='200' z='300'>                                                   
                        </PointLight>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);


      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");
      var lightNode = (PointLight)scene.Children.First();
      Assert.AreEqual(0.5f, lightNode.Color.R);
      Assert.AreEqual(0.2f, lightNode.Color.G);
      Assert.AreEqual(0.1f, lightNode.Color.B);
      Assert.AreEqual(100, lightNode.Position.X);
      Assert.AreEqual(200, lightNode.Position.Y);
      Assert.AreEqual(300, lightNode.Position.Z);
    }

    [Test()]
    public void ReadFile_OneDirectionalLightNode_CorrectParametersOfLightNode()
    {
      var testXml = @"<Scene>
                        <DirectionalLight r='0.5' g='0.2' b='0.1' x='100' y='200' z='300'>                                                   
                        </DirectionalLight>                          
                      </Scene>";
      File.WriteAllText("test.xml", testXml);

      var normalizedDirection = new Vector3(100, 200, 300).Normalized();
      var xmlReader = CreateXMLReader();
      var scene = xmlReader.Read("test.xml");
      var lightNode = (DirectionalLight)scene.Children.First();
      Assert.AreEqual(0.5f, lightNode.Color.R);
      Assert.AreEqual(0.2f, lightNode.Color.G);
      Assert.AreEqual(0.1f, lightNode.Color.B);
      Assert.AreEqual(normalizedDirection.X, lightNode.Direction.X);
      Assert.AreEqual(normalizedDirection.Y, lightNode.Direction.Y);
      Assert.AreEqual(normalizedDirection.Z, lightNode.Direction.Z);
    }

    private static XMLDataNodeReader CreateXMLReader()
    {
      var vertexFactory = new VertexFactory();
      var faceFactory = new FaceFactory();
      var meshLoaderFactory = new MeshLoaderFactory(vertexFactory, faceFactory);
      var meshFactory = new MeshFactory(meshLoaderFactory);
      var shapeFactory = new ShapeFactory(meshFactory);
      var sceneNodeFactory = new SceneNodeFactory(shapeFactory);
      var dataNodeFactory = new DataNodeFactory(sceneNodeFactory);
      var xmlReader = new XMLDataNodeReader(dataNodeFactory);
      return xmlReader;
    }
  }
}
