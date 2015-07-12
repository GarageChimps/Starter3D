using System;
using Autofac;
using Starter3D.API.OpenGLRendering;
using ThreeAPI.examples;
using ThreeAPI.geometry.factories;
using ThreeAPI.renderer;
using ThreeAPI.resources;
using ThreeAPI.scene.nodes.factories;
using ThreeAPI.scene.persistence;
using ThreeAPI.scene.persistence.factories;

namespace ThreeDU
{
  public class ShaderExample
  {
    public static int WindowWidth = 512;
    public static int WindowHeight = 512;
    public static float FrameRate = 60;

    private static IContainer Container { get; set; }

    private static void InitDependencyContainer ()
    {
      var builder = new ContainerBuilder();
      builder.RegisterType<MaterialFactory>().As<IMaterialFactory>().SingleInstance();
      builder.RegisterType<ResourceManager>().As<IResourceManager>().SingleInstance();
      builder.RegisterType<ShapeFactory>().As<IShapeFactory>().SingleInstance();
      builder.RegisterType<OpenGLRenderer>().As<IRenderer>().SingleInstance();
      builder.RegisterType<VertexFactory>().As<IVertexFactory>().SingleInstance();
      builder.RegisterType<FaceFactory>().As<IFaceFactory>().SingleInstance();
      builder.RegisterType<MeshLoaderFactory>().As<IMeshLoaderFactory>().SingleInstance();
      builder.RegisterType<MeshFactory>().As<IMeshFactory>().SingleInstance();
      builder.RegisterType<SceneNodeFactory>().As<ISceneNodeFactory>().SingleInstance();
      builder.RegisterType<DataNodeFactory>().As<IDataNodeFactory>().SingleInstance();
      builder.RegisterType<XMLDataNodeReader>().As<ISceneNodeReader>().SingleInstance();
      builder.RegisterType<GameWindowFactory>().As<IGameWindowFactory>().SingleInstance();

      Container = builder.Build();
      
    }

    [STAThread]
    public static void Main()
    {
      InitDependencyContainer();
      using (var scope = Container.BeginLifetimeScope())
      {
        var gameWindowFactory = scope.Resolve<IGameWindowFactory>();
        using (var window = gameWindowFactory.CreateGameWindow(WindowWidth, WindowHeight))
        {
          window.Run(FrameRate);
        }
      }
    }
  }
}

