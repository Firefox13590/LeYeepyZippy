using UnityEngine;

public class CameraStream : MonoBehaviour
{
    [Tooltip("Si false, utilise le render texture en permanence. Sinon affiche le render texture quand le joueur est proche avec un trigger event.")]
    public bool onlyRenderWhenTrigger = true;
    public Material activeStreamMat, disabledStreamMat;


    Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = GameManager.Instance.Cameras.Find(cam => cam.CompareTag("MainCamera"));
        //Debug.Log(camera, camera);

        if (camera)
        {
            if (onlyRenderWhenTrigger) SetCameraStream(false);
            else
            {
                SetCameraStream(true);
                Destroy(GetComponent<Rigidbody>());
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).GetComponent<Collider>());
                }
            }
        }
    }
    private void OnTriggerEnter(Collider _)
    {
        if (onlyRenderWhenTrigger && camera) SetCameraStream(true);
    }
    private void OnTriggerExit(Collider _)
    {
        if (onlyRenderWhenTrigger && camera) SetCameraStream(false);
    }


    void SetCameraStream(bool active)
    {
        camera.gameObject.SetActive(active);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material = (active) ? activeStreamMat : disabledStreamMat;
        }
    }
}
