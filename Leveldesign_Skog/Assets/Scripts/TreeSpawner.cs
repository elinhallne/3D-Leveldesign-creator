using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public bool generateEnverment;

    [SerializeField]
    private float radius;

    [SerializeField]
    private int amountPlant, amountTree, amountDeadTree, amountStubb, amountStone, amountMushroom, amountFireflies;
 

    public GameObject[] plantPrefab, treePrefab, deadTreePrefab, stubbPrefab, stonePrefab, mushroomPrefab, fireFliesPrefab;




    public void Start()
    {
        if (generateEnverment == true) {

            InstantGameObject(amountTree, treePrefab, 8f, 12f);
            InstantGameObject(amountPlant, plantPrefab, 8f, 20f);
            InstantGameObject(amountDeadTree, deadTreePrefab, 8f, 12f);
            InstantGameObject(amountStubb, stubbPrefab, 8f, 12f);
            InstantGameObject(amountStone, stonePrefab, 8f, 10f);
            InstantGameObject(amountMushroom, mushroomPrefab, 8f, 10f);
            InstantGameObject(amountFireflies, fireFliesPrefab, 1f, 1f);

        }
    }

     public void InstantGameObject(int amountObjects, GameObject[] gameObjects, float minSize, float maxSize)
    {
        for (int i = 0; i < amountObjects; i++)
        {
            /* int layerMask = 1 << 9;
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(TreePosition().x, 9999f, TreePosition().z), Vector3.down, out hit, Mathf.Infinity, layerMask)) ;
            {
                TreePosition();
                Debug.Log("hit");

            } */

            GameObject _prefab = RandomGameObject(gameObjects);

            ManipulateScale(_prefab, minSize, maxSize);

            var newTreePrefab = Instantiate(_prefab, TreePosition(), ManipulateRotation(_prefab));

            newTreePrefab.transform.parent = gameObject.transform;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1 * radius);

       
    }

  
    private GameObject RandomGameObject(GameObject[] gameObjects)
    {
       float instanNr = Random.Range(0f, gameObjects.Length);

        
       int i = (int) instanNr;

       return gameObjects[i];

    }

    private void ManipulateScale(GameObject gameObject, float _minSize, float _maxSize)
    {
        float _randomScale = Random.Range(_minSize, _maxSize);
        Vector3 _scale = new Vector3(_randomScale, _randomScale, _randomScale);
        gameObject.transform.localScale = _scale;
    }

    private Quaternion ManipulateRotation(GameObject gameObject)
    {
        float _randomRotation = Random.Range(0f, 360f);
        Vector3 _rotation = new Vector3(0f, _randomRotation, 0f);
        Quaternion _quaternion = Quaternion.Euler(_rotation);

        return _quaternion;        
    }

    private Vector3 TreePosition()
    {

        var circleSize = Random.insideUnitCircle * radius;

        Vector3 worldPos = transform.TransformPoint(circleSize.x, 0, circleSize.y);

        RaycastHit hit, hita, hitb, hitc, hitd;
        float terrainHeight = 0f;
        int layerMask = 1 << 8;
               

       if (Physics.Raycast(new Vector3(worldPos.x, 9999f, worldPos.z), Vector3.down, out hit, Mathf.Infinity, layerMask)) ;
        {
            terrainHeight = hit.point.y;
           //ray cast
            CheckHeightDifference( out hita, out hitb, out hitc, out hitd, terrainHeight, worldPos);
            
        }

        float _pointSpawn = terrainHeight;

        var treePosition = new Vector3(circleSize.x, _pointSpawn, circleSize.y) + transform.position;

        return treePosition;
    }

   
    private void CheckHeightDifference(out RaycastHit hit1, out RaycastHit hit2, out RaycastHit hit3, out RaycastHit hit4, float objectHeight, Vector3 _position)
    {
        int layerMask = 1 << 8;
        float raycastX = 3f;
        float raycastZ = 3f;
        List<RaycastHit> castArray = new List<RaycastHit>();

        Physics.Raycast(new Vector3(_position.x + raycastX, 9999f, _position.z), Vector3.down, out hit1, Mathf.Infinity, layerMask);
        castArray.Add(hit1);

        Physics.Raycast(new Vector3(_position.x - raycastX, 9999f, _position.z), Vector3.down, out hit2, Mathf.Infinity, layerMask);
        castArray.Add(hit2);

        Physics.Raycast(new Vector3(_position.x, 9999f, _position.z + raycastZ), Vector3.down, out hit3, Mathf.Infinity, layerMask);
        castArray.Add(hit3);

        Physics.Raycast(new Vector3(_position.x, 9999f, _position.z - raycastZ), Vector3.down, out hit4, Mathf.Infinity, layerMask);
        castArray.Add(hit4);
        
        for (int i = 0; i < castArray.Count; i++)
        {
            float heightDiffrens = objectHeight - castArray[i].point.y;

            float heightLimit = 0.5f;
            if (heightLimit < heightDiffrens)
            {
                objectHeight = objectHeight - 3;
                //Debug.Log("out of bounds" + " " + heightDiffrens);
                castArray.Clear();
            }
        }

    }
  



}
