using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneralSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] spawnObjects;
    [SerializeField] GameObject[] spawnAreas;

    //private void Start()
    //{
    //    if (spawnAreas.Length > 0)
    //    {
    //        Debug.Log(spawnAreas[0].GetComponent<MeshFilter>().mesh.vertexCount);
    //        for (int i = 0; i < spawnAreas[0].GetComponent<MeshFilter>().mesh.vertexCount; i++)
    //        {
    //            Debug.Log(spawnAreas[0].GetComponent<MeshFilter>().mesh.vertices[i]);
    //            Debug.Log(spawnAreas[0].transform.TransformPoint(spawnAreas[0].GetComponent<MeshFilter>().mesh.vertices[i]));
    //        }
    //    }
    //}



    Vector3 SelectRandomMeshVertex(GameObject obj)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        //Debug.Log(mesh.vertices[Random.Range(0, mesh.vertexCount)]);
        //Debug.Log(obj.transform.TransformPoint(mesh.vertices[Random.Range(0, mesh.vertexCount)]));
        return obj.transform.TransformPoint(mesh.vertices[Random.Range(0, mesh.vertexCount)]);
    }

    public void SpawnObject(int indexObject, int indexArea, Vector3 position, Transform parent, bool spawnEachObject, bool spawnOnEachArea, bool spawnRandomObject, bool spawnOnRandomArea, bool justPlaceInstance)
    {
        if (
            indexObject >= spawnObjects.Length |
            indexObject < 0 |
            indexArea >= spawnAreas.Length |
            indexArea < 0
            )
            throw new IndexOutOfRangeException("La position demandée doit être valide pour les objects et les areas");
        if (spawnObjects[indexObject] == null | spawnAreas[indexArea] == null) throw new NullReferenceException();

        int
            finalPosObj = indexObject,
            finalPosArea = indexArea;

        if (justPlaceInstance)
        {
            Instantiate(spawnObjects[finalPosObj], position, Quaternion.identity, parent);
        }
        else if (spawnEachObject && spawnOnEachArea)
        {
            if (spawnObjects.Length != spawnAreas.Length) throw new Exception("spawnObjects et spawnAreas ne sont pas de même taille.");
            for (int i = 0; i < spawnObjects.Length; i++)
            {
                finalPosObj = (spawnRandomObject) ? Random.Range(0, spawnObjects.Length) : i;
                finalPosArea = (spawnOnRandomArea) ? Random.Range(0, spawnAreas.Length) : i;
                Instantiate(spawnObjects[finalPosObj], SelectRandomMeshVertex(spawnAreas[finalPosArea]), Quaternion.identity, parent);
            }
        }
        else if (spawnEachObject)
        {
            for (int i = 0; i < spawnObjects.Length; i++)
            {
                finalPosObj = (spawnRandomObject) ? Random.Range(0, spawnObjects.Length) : i;
                Instantiate(spawnObjects[finalPosObj], SelectRandomMeshVertex(spawnAreas[finalPosArea]), Quaternion.identity, parent);
            }
        }
        else if (spawnOnEachArea)
        {
            for (int i = 0; i < spawnObjects.Length; i++)
            {
                finalPosArea = (spawnOnRandomArea) ? Random.Range(0, spawnAreas.Length) : i;
                Instantiate(spawnObjects[finalPosObj], SelectRandomMeshVertex(spawnAreas[finalPosArea]), Quaternion.identity, parent);
            }
        }
        else
        {
            finalPosObj = (spawnRandomObject) ? Random.Range(0, spawnObjects.Length) : indexObject;
            finalPosArea = (spawnOnRandomArea) ? Random.Range(0, spawnAreas.Length) : indexArea;
            Instantiate(spawnObjects[finalPosObj], SelectRandomMeshVertex(spawnAreas[finalPosArea]), Quaternion.identity, parent);
        }
    }
    public void SpawnObject(int index) => SpawnObject(index, index, new Vector3(), null, false, false, false, false, false);
    public void SpawnObject(Vector3 position) => SpawnObject(0, 0, position, null, false, false, false, false, true);
    public void SpawnObject(int index, Vector3 position) => SpawnObject(index, index, position, null, false, false, false, false, true);
    public void SpawnObject(int index, Transform parent) => SpawnObject(index, index, new Vector3(), parent, false, false, false, false, false);
    public void SpawnObject(Vector3 position, Transform parent) => SpawnObject(0, 0, position, parent, false, false, false, false, true);
    public void SpawnObject(int index, bool spawnEachObject, bool spawnOnEachArea, bool spawnRandomObject, bool spawnOnRandomArea) =>
        SpawnObject(index, index, new Vector3(), null, spawnEachObject, spawnOnEachArea, spawnRandomObject, spawnOnRandomArea, false);
    public void SpawnObject(int index, Transform parent, bool spawnEachObject, bool spawnOnEachArea, bool spawnRandomObject, bool spawnOnRandomArea) =>
        SpawnObject(index, index, new Vector3(), parent, spawnEachObject, spawnOnEachArea, spawnRandomObject, spawnOnRandomArea, false);
    public void SpawnObject(int indexObject, int indexArea) => SpawnObject(indexObject, indexArea, new Vector3(), null, false, false, false, false, false);
    public void SpawnObject(int indexObject, int indexArea, Transform parent) => SpawnObject(indexObject, indexArea, new Vector3(), parent, false, false, false, false, false);
    public void SpawnObject(int indexObject, int indexArea, bool spawnEachObject, bool spawnOnEachArea, bool spawnRandomObject, bool spawnOnRandomArea) =>
        SpawnObject(indexObject, indexArea, new Vector3(), null, spawnEachObject, spawnOnEachArea, spawnRandomObject, spawnOnRandomArea, false);
    public void SpawnObject(int indexObject, int indexArea, Transform parent, bool spawnEachObject, bool spawnOnEachArea, bool spawnRandomObject, bool spawnOnRandomArea) =>
        SpawnObject(indexObject, indexArea, new Vector3(), parent, spawnEachObject, spawnOnEachArea, spawnRandomObject, spawnOnRandomArea, false);
}
