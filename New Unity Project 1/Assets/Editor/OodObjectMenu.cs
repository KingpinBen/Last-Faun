using UnityEngine;
using UnityEditor;
using System.Collections;

public static class OodObjectMenu
{
    private static Mesh _sPlaneMesh;

    [MenuItem("PT Tools/Create Directional Light")]
    public static void CreateDirectionalLight()
    {
        var go = new GameObject("New Directional Light");
        var light = go.AddComponent<Light>();

        light.intensity = .4f;
        light.color = Color.white;
        light.shadows = LightShadows.None;
        light.type = LightType.Directional;

        go.AddComponent<GlobalLightScript>();
        Debug.Log("Don't forget to set the material to the skybox.");
    }

    [MenuItem("PT Tools/Create Basic Plane")]
    public static void CreateBasicPlane()
    {
        var go = new GameObject("Basic Plane");

        go.AddComponent<MeshRenderer>();
        var meshfilter = go.AddComponent<MeshFilter>();
        var collider = go.AddComponent<MeshCollider>();


        if (_sPlaneMesh == false)
        {
            var size = 10;
            var halfSize = size*.5f;

            _sPlaneMesh = new Mesh
                              {
                                  vertices = new[]
                                                 {
                                                     new Vector3(-halfSize, 0, halfSize),
                                                     new Vector3(halfSize, 0, halfSize),
                                                     new Vector3(halfSize, 0, -halfSize),
                                                     new Vector3(-halfSize, 0, -halfSize)
                                                 },
                                  triangles = new[] {0, 1, 2, 0, 2, 3},
                                  normals = new[]
                                                {
                                                    Vector3.up, Vector3.up, Vector3.up,
                                                    Vector3.up
                                                },
                                  colors = new[]
                                               {
                                                   Color.red, Color.blue, Color.green, Color.magenta
                                               },
                                  name = "Basic Plane",
                                  uv =
                                      new[]
                                          {Vector2.zero, new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)}
                              };
        }

        meshfilter.mesh = _sPlaneMesh;
        collider.sharedMesh = _sPlaneMesh;

        Debug.Log("The plane comes with a collider by default. Remove it if you want.");
    }

    [MenuItem("PT Tools/Place New Level Pieces")]
    public static void NewPieces()
    {
        var go = new GameObject("Camera Manager");
        go.tag = "CameraManager";

        go.AddComponent<CameraManagerScript>();
        go.AddComponent<CursorManager>();

        Camera.main.transform.parent = go.transform;

        var cameraGo = new GameObject("Cameras");
        var triggersGo = new GameObject("Triggers");

        cameraGo.transform.parent = go.transform;
        triggersGo.transform.parent = go.transform;

        var lightGo = new GameObject("New Directional Light");
        var light = lightGo.AddComponent<Light>();

        light.intensity = .4f;
        light.color = Color.white;
        light.shadows = LightShadows.None;
        light.type = LightType.Directional;

        lightGo.AddComponent<GlobalLightScript>();

        lightGo.transform.parent = go.transform;
    }
}