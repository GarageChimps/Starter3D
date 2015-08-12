struct vertexAttributes {
  float3 inPosition : POSITION;
  float3 inNormal : NORMAL;
  float3 inTextureCoords: TEXCOORD0;
};

struct fragmentAttributes {
  float4 position : SV_POSITION;
  float2 pixelCoords : TEXCOORD0;
};


uniform float3 mouse;
uniform float time;

static const float infinity = 1. / 0.;


struct Sphere {
  float3 position;
  float radius;
  int materialIndex;
};

struct Material {
  float4 diffuse;
  float4 specular;
  float shininess;
  float reflectivity;
};

struct Light {
  float3 position;
  float4 color;
};

struct Ray {
  float3 position;
  float3 direction;
};

struct Camera {
  float3 position;
};

struct Scene {
  float4 ambient;
  float4 background;
};

static const int numberOfSpheres = 8;
static const int numLights = 2;
static const int numberOfMaterials = 5;
static const int maxNumberOfReflections = 2;

static  Sphere spheres[numberOfSpheres];
static  Material materials[numberOfMaterials];
static  Light lights[numLights];
static  Camera camera;
static  Scene scene;


//ray-sphere intersection
float intersect(Ray ray, Sphere sphere)
{
  float a = dot(ray.direction, ray.direction);
  float b = dot(ray.position - sphere.position, ray.direction);
  float c = dot(ray.position - sphere.position, ray.position - sphere.position) - sphere.radius*sphere.radius;

  float discr = b*b - a*c;
  if (discr < 0.0)
    return infinity;

  discr = sqrt(discr);
  float t0 = (-b - discr) / a;
  float t1 = (-b + discr) / a;

  float tMin = min(t0, t1);
  if (tMin < 0.0)
    return infinity;

  return tMin;

}

//shadows
bool isInShadow(float3 p, Sphere sphere, Light light)
{
  float lightDistance = distance(light.position, p);
  float3 shadowDir = normalize(light.position - p);
    Ray shadowRay;
  shadowRay.position = p + 0.1 * shadowDir;
  shadowRay.direction = shadowDir;
  float tShadow = intersect(shadowRay, sphere);
  if (!isinf(tShadow) && tShadow < lightDistance)
    return true;

  return false;
}

bool isInOtherSphereShadow(float3 p, Sphere thisSphere, Light light)
{
  for (int i = 0; i < numberOfSpheres; i++)
  {
    if (isInShadow(p, spheres[i], light))
      return true;
  }
  return false;
}

//blinn-phong shading
float4 blinnPhongShading(float3 p, float3 n, Sphere sphere)
{
  Material material = materials[sphere.materialIndex];
  float3 v = camera.position - p;
    v = normalize(v);

  float4 shadedColor = scene.ambient * material.diffuse;
    for (int i = 0; i < numLights; i++)
    {
      float4 lightColor = lights[i].color;
        if (isInOtherSphereShadow(p, sphere, lights[i]))
          lightColor = float4(0, 0, 0, 1);
      float3 l = lights[i].position - p;
        l = normalize(l);
      float3 h = v + l;
        h = normalize(h);

      shadedColor = shadedColor + lightColor * (max(0, dot(n, l)) * material.diffuse + pow(max(0, dot(n, h)), material.shininess) * material.specular);
    }
  return shadedColor;
}


float4 rayTrace(Ray ray)
{
  float4 accumulatedColor = float4(0, 0, 0, 1);
    float frac = 1.0;
  for (int i = 0; i < maxNumberOfReflections + 1; i++)
  {
    float tMin = infinity;
    int sphereMin = -1;
    for (int i = 0; i < numberOfSpheres; i++)
    {
      float t = intersect(ray, spheres[i]);
      if (t < tMin)
      {
        tMin = t;
        sphereMin = i;
      }
    }

    if (!isinf(tMin))
    {
      float3 p = ray.position + tMin*ray.direction;
        float3 n = normalize(p - spheres[sphereMin].position);
        Material mat = materials[spheres[sphereMin].materialIndex];
      float4 localColor = blinnPhongShading(p, n, spheres[sphereMin]);
        accumulatedColor += localColor * frac;
      if (mat.reflectivity > 0)
      {
        ray.position = p;
        ray.direction = normalize(reflect(ray.direction, n));
        frac *= mat.reflectivity;
      }
      else
      {
        break;
      }
    }

    accumulatedColor += scene.background * frac;
  }
  return accumulatedColor;
}

void init()
{
  camera.position = float3(0.5, 0.5, 1.5);
  scene.ambient = float4(0.1, 0.1, 0.1, 1);
  scene.background = float4(0, 0, 0, 1);

  // Blue specular
  materials[0].diffuse = float4(0.156, 0.126, 0.507, 1);
  materials[0].specular = float4(1, 1, 1, 1);
  materials[0].shininess = 100;
  materials[0].reflectivity = 0.5;

  // Yellow
  materials[1].diffuse = float4(0.656, 0.626, 0.107, 1);
  materials[1].specular = float4(0, 0, 0, 1);
  materials[1].shininess = 1;
  materials[1].reflectivity = 0.5;

  // White
  materials[2].diffuse = float4(0.739, 0.725, 0.765, 1);
  materials[2].specular = float4(0, 0, 0, 1);
  materials[2].shininess = 1;
  materials[2].reflectivity = 0.5;

  // Red
  materials[3].diffuse = float4(0.639, 0.06, 0.062, 1);
  materials[3].specular = float4(0, 0, 0, 1);
  materials[3].shininess = 1;
  materials[3].reflectivity = 0.5;

  // Green
  materials[4].diffuse = float4(0.156, 0.426, 0.107, 1);
  materials[4].specular = float4(0, 0, 0, 1);
  materials[4].shininess = 1;
  materials[4].reflectivity = 0.5;


  lights[0].position = float3(0.5, 0.99, 0.5);
  lights[0].color = float4(0.8, 0.7, 0.6, 1);
  lights[1].position = float3(0.5, 0.99, -0.5);
  lights[1].color = float4(0.2, 0.2, 0.2, 1);

  //Blue sphere
  spheres[0].position = float3(0.35, 0.24, -0.72);
  spheres[0].radius = 0.2;
  spheres[0].materialIndex = 0;

  //Yellow sphere
  spheres[1].position = float3(0.82, 0.2, -0.25);
  spheres[1].radius = 0.19;
  spheres[1].materialIndex = 1;

  //Back wall
  spheres[2].position = float3(0.5, 0.5, -1001);
  spheres[2].radius = 1000;
  spheres[2].materialIndex = 2;

  //Ceiling
  spheres[3].position = float3(0.5, 1001.1, -0.5);
  spheres[3].radius = 1000;
  spheres[3].materialIndex = 2;

  //Floor
  spheres[4].position = float3(0.5, -1000.1, -0.5);
  spheres[4].radius = 1000;
  spheres[4].materialIndex = 2;

  //Right wall
  spheres[5].position = float3(1001.1, 0.5, -0.5);
  spheres[5].radius = 1000;
  spheres[5].materialIndex = 4;

  //Left wall
  spheres[6].position = float3(-1000, 0.5, -0.5);
  spheres[6].radius = 999.9;
  spheres[6].materialIndex = 3;

  //White sphere
  spheres[7].position = float3(0.5, 0.7, -0.5);
  spheres[7].radius = 0.03;
  spheres[7].materialIndex = 2;

}

fragmentAttributes VShader(vertexAttributes input)
{
  fragmentAttributes output = (fragmentAttributes)0;
  output.position = float4(input.inPosition, 1);
  output.pixelCoords = float2((input.inPosition.x + 1.0) / 2.0, (input.inPosition.y + 1.0) / 2.0);
  return output;
}


float4 FShader(fragmentAttributes input) : SV_Target
{
  //Initialize elements
  init();

  //Light movement 
  lights[0].position = float3(mouse.x, mouse.y, 0.0);

  //Object movement
  float speed = 1;
  spheres[0].position = float3(0.5, 0.24, -0.5) + 0.3 * float3(sin(speed*(time)), 0, cos(speed*(time)));
  spheres[1].position = float3(0.5, 0.2, -0.5) + 0.23 * float3(sin(speed*(time)+10), 0, cos(speed*(time)+10));
  spheres[7].position = float3(0.5, 0.7, -0.5) + 0.2 * float3(0, sin(speed*(time)), 0);

  //Raytrace objects
  float3 pixel = float3(input.pixelCoords, 0);
    Ray ray;
  ray.position = camera.position; 
  ray.direction = normalize(pixel - camera.position);
  return rayTrace(ray);
}


technique10 Render
  {

    pass P0
    {
      SetGeometryShader(0);
      SetVertexShader(CompileShader(vs_4_0, VShader()));
      SetPixelShader(CompileShader(ps_4_0, FShader()));
    }
  }

