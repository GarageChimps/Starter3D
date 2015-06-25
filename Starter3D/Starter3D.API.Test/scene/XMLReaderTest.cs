using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ThreeAPI.scene;

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

      var scaleNode = (ScaleNode) scene.Children.First();
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
      Assert.AreEqual((float)Math.PI / 2.0f, rotateNode.Angle);
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
