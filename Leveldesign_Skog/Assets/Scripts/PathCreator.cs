using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{

    public bool generatePath;
    public Transform[] pointPath;
    public float pathWhith;

    List<Transform> pointPathList = new List<Transform>();

    void Awake()
    {
        AddItemToList();
        MeasureDistanse();
    }

    void Update()
    {
        Debug.Log("Editor causes this Update");
        Mesh updateMesh;
        Vector3[] verticesUpdate;

        if (generatePath)
        {
            for (int j = 0; j <= pointPathList.Count; j++) {
            updateMesh = pointPathList[j].gameObject.GetComponent<MeshFilter>().mesh;

            verticesUpdate = updateMesh.vertices;

            for (var i = 0; i < verticesUpdate.Length; i++)
            {
                //verticesUpdate[i] -= ;
            }
            updateMesh.vertices = verticesUpdate;
            updateMesh.RecalculateBounds();
        }

            generatePath = false;
        }
    }

   


    private void AddItemToList()
    {
        pointPathList.AddRange(pointPath);
    }

   
    private void MeasureDistanse()
    {
        float dist = 0; 
       
            for (int i = 0; i <= pointPathList.Count; i++)
        {
            Vector3 firstPos = transform.InverseTransformPoint(pointPathList[i].position);
            Vector3 secondPos = transform.InverseTransformPoint(pointPathList[1 + i].position);
            
            CreateQuad(ReturnValue(secondPos.z, firstPos.z, pathWhith), ReturnValue(firstPos.x, secondPos.x,  pathWhith), firstPos, secondPos, pointPathList[i].gameObject);
                         
            
          
        }

    }

   

    private float ReturnValue(float positionZ, float positionX, float value)
    {
        if(positionZ < positionX)
        {
           
            return value;

        }
        value = -value;
        return value;
    }

    private void CreateQuad(float width, float height, Vector3 pos1, Vector3 pos2, GameObject listIndex) //Add a list)
    {
        

        MeshRenderer meshRenderer = listIndex.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = listIndex.AddComponent<MeshFilter>();
        MeshCollider meshCollider = listIndex.AddComponent(typeof(MeshCollider)) as MeshCollider; 

        Mesh mesh = new Mesh();
        //Bottom-left Bottom-right Top-left Top-right
        Vector3[] vertices = new Vector3[4]
        {
            //INPUT THE VECTORS HERE TO 
            new Vector3(pos1.x, 0, pos1.z),
            new Vector3(pos1. x - width,0, pos1.z - height),
            new Vector3(pos2.x, 0, pos2.z),
            new Vector3(pos2.x - width, 0, pos2.z - height)
        };
        mesh.vertices = vertices;
        
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] -= pos1; 
        }

        // assign the local vertices array into the vertices array of the Mesh.
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = meshFilter.sharedMesh; 
    }
}

