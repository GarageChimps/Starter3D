using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OpenTK;
using Starter3D.API.geometry.factories;

namespace Starter3D.API.geometry.loaders
{
  public class ObjMeshLoader : IMeshLoader
  {
    private readonly char[] _splitCharacters = { ' ' };
    private readonly char[] _faceParamaterSplitter = { '/' };

    private readonly IVertexFactory _vertexFactory;
    private readonly IFaceFactory _faceFactory;

    public ObjMeshLoader(IVertexFactory vertexFactory, IFaceFactory faceFactory)
    {
      _vertexFactory = vertexFactory;
      _faceFactory = faceFactory;
    }

    public void Load(IMesh mesh, string filePath)
    {
      using (var streamReader = new StreamReader(filePath))
      {
        Load(streamReader, mesh);
      }
    }

    private void Load(TextReader textReader, IMesh mesh)
    {
      var vertices = new List<IVertex>();
      var faces = new List<IFace>();
      var verticesIndexMap = new Dictionary<IVertex, int>();
      int currentVertexIndex = 0;

      var positions = new List<Vector3>();
      var normals = new List<Vector3>();
      var texCoords = new List<Vector3>();
      
      string line;
      while ((line = textReader.ReadLine()) != null)
      {
        line = line.Trim(_splitCharacters);
        line = line.Replace("  ", " ");

        var parameters = line.Split(_splitCharacters);

        switch (parameters[0])
        {
          case "p": // Point
            break;

          case "v": // Vertex
            var x = float.Parse(parameters[1], CultureInfo.InvariantCulture);
            var y = float.Parse(parameters[2], CultureInfo.InvariantCulture);
            var z = float.Parse(parameters[3], CultureInfo.InvariantCulture);
            positions.Add(new Vector3(x, y, z));
            break;

          case "vt": // TexCoord
            var u = float.Parse(parameters[1], CultureInfo.InvariantCulture);
            var v = float.Parse(parameters[2], CultureInfo.InvariantCulture);
            texCoords.Add(new Vector3(u, v, 0));
            break;

          case "vn": // Normal
            var nx = float.Parse(parameters[1], CultureInfo.InvariantCulture);
            var ny = float.Parse(parameters[2], CultureInfo.InvariantCulture);
            var nz = float.Parse(parameters[3], CultureInfo.InvariantCulture);
            normals.Add(new Vector3(nx, ny, nz).Normalized());
            break;

          case "f":
            var faceVertexIndices = new List<int>();
            for (int i = 0; i < parameters.Length - 1; i++)
            {
              //The definition of what is a vertex in an OBJ file only exists appears when the vertex of each face is being defined
              //We only want to add distinctive vertices to our list of vertices, so we test here if we already have a vertex with the same
              //parameters, in which case we will use the existing vertex, if not we will add the new vertex to our list and increase the vertex
              //index counter
              var candidateVertex = GetVertex(parameters[i + 1], positions, normals, texCoords);
              if (!verticesIndexMap.ContainsKey(candidateVertex))
              {
                verticesIndexMap.Add(candidateVertex, currentVertexIndex);
                vertices.Add(candidateVertex);
                currentVertexIndex++;
              }
              var index = verticesIndexMap[candidateVertex];
              faceVertexIndices.Add(index);
            }
            var face = _faceFactory.CreateFace(faceVertexIndices);
            faces.Add(face);
            break;
        }
      }
      foreach (var vertex in vertices)
      {
        mesh.AddVertex(vertex);
      }
      foreach (var face in faces)
      {
        mesh.AddFace(face);
      }
    }
    
    private IVertex GetVertex(string faceParameter, List<Vector3> vertices, List<Vector3> normals, List<Vector3> texCoords)
    {
      var position = new Vector3();
      var texCoord = new Vector3();
      var normal = new Vector3();

      var parameters = faceParameter.Split(_faceParamaterSplitter);

      var vertexIndex = int.Parse(parameters[0]);
      if (vertexIndex < 0) vertexIndex = vertices.Count + vertexIndex;
      else vertexIndex = vertexIndex - 1;
      position = vertices[vertexIndex];

      if (parameters.Length > 1 && texCoords.Count > 0)
      {
        int texCoordIndex = 0;
        if (int.TryParse(parameters[1], out texCoordIndex))
        {
          if (texCoordIndex < 0) texCoordIndex = texCoords.Count + texCoordIndex;
          else texCoordIndex = texCoordIndex - 1;
          texCoord = texCoords[texCoordIndex];
        }
      }

      if (parameters.Length > 2 && normals.Count > 0)
      {
        int normalIndex = 0;
        if (int.TryParse(parameters[2], out normalIndex))
        {
          if (normalIndex < 0) normalIndex = normals.Count + normalIndex;
          else normalIndex = normalIndex - 1;
          normal = normals[normalIndex];
        }
      }
      var vertex = _vertexFactory.CreateVertex(position, normal, texCoord);
      return vertex;
    }

  }
}