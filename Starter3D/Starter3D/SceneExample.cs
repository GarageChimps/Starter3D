using System;
using Starter3D;

namespace ThreeDU
{
  public class SceneExample
  {
    private Scene scene;
    private Camera camera;
    private Geometry geometry;
    private Mesh mesh;
    private GLRenderer renderer;

    float width, height;

    public SceneExample ()
    {
      scene = new Scene ();
      camera = new PerspectiveCamera (1, 100000, 75, 0, width / height);
      var pos = camera.Position;
      pos.Z = 1000;

      geometry = new BoxGeometry( 200, 200, 200 );
      mesh = new Mesh (geometry);

      scene.AddChild (mesh);
      renderer = new GLRenderer ();

    }

    public void Animate(){
      var rot = mesh.Rotation;
      rot.X += 0.01f;
      rot.Y += 0.02f;

      renderer.Render( scene, camera );
    }
  }
}

