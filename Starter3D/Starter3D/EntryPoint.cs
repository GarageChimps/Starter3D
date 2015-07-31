using System;
using Autofac;
using Starter3D.API.controller;
using Starter3D.API.geometry.factories;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes.factories;
using Starter3D.API.scene.persistence;
using Starter3D.API.scene.persistence.factories;
using Starter3D.API.utils;
using Starter3D.Controllers;

namespace Starter3D.OpenGL
{
  public class EntryPoint
  {
    private static readonly int WindowWidth = 512;
    private static readonly int WindowHeight = 512;
    private static readonly float FrameRate = 60;

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
      builder.RegisterType<SceneDataNodeFactory>().As<ISceneDataNodeFactory>().SingleInstance();
      builder.RegisterType<XmlSceneReader>().As<ISceneReader>().SingleInstance();
      builder.RegisterType<GameWindowFactory>().As<IGameWindowFactory>().SingleInstance();
      builder.RegisterType<Configuration>().As<IConfiguration>().SingleInstance();
      builder.RegisterType<ShaderFactory>().As<IShaderFactory>().SingleInstance();


      //AE July 2015: Current controller is registered here. This could be extended by using a factory or reading current controller from config file
      builder.RegisterType<MaterialEditorController>().As<IController>().SingleInstance();

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

