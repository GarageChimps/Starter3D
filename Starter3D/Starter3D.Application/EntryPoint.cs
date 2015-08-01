using System;
using Autofac;
using Starter3D.Application.controllers;
using Starter3D.Application.windows;
using Starter3D.API.controller;
using Starter3D.API.geometry.factories;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes.factories;
using Starter3D.API.scene.persistence;
using Starter3D.API.scene.persistence.factories;
using Starter3D.API.utils;
using Starter3D.Renderers;

namespace Starter3D.Application
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
      builder.RegisterType<VertexFactory>().As<IVertexFactory>().SingleInstance();
      builder.RegisterType<FaceFactory>().As<IFaceFactory>().SingleInstance();
      builder.RegisterType<MeshLoaderFactory>().As<IMeshLoaderFactory>().SingleInstance();
      builder.RegisterType<MeshFactory>().As<IMeshFactory>().SingleInstance();
      builder.RegisterType<SceneNodeFactory>().As<ISceneNodeFactory>().SingleInstance();
      builder.RegisterType<SceneDataNodeFactory>().As<ISceneDataNodeFactory>().SingleInstance();
      builder.RegisterType<XmlSceneReader>().As<ISceneReader>().SingleInstance();
      builder.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();
      builder.RegisterType<Configuration>().As<IConfiguration>().SingleInstance();
      builder.RegisterType<ShaderFactory>().As<IShaderFactory>().SingleInstance();
      builder.RegisterType<TextureFactory>().As<ITextureFactory>().SingleInstance();
      builder.RegisterType<RendererFactory>().As<IRendererFactory>().SingleInstance();
      builder.RegisterType<ControllerFactory>().As<IControllerFactory>().SingleInstance();
      
      Container = builder.Build();
      
    }

    [STAThread]
    public static void Main()
    {
      InitDependencyContainer();
      using (var scope = Container.BeginLifetimeScope())
      {
        var gameWindowFactory = scope.Resolve<IWindowFactory>();
        var configuration = scope.Resolve<IConfiguration>();
        using (var window = gameWindowFactory.CreateWindow(WindowWidth, WindowHeight, configuration))
        {
          window.Run(FrameRate);
        }
      }
    }
  }
}

