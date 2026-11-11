using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class MarioParty : MonoBehaviour
{
    [Min(1f)]
    public float cooldownDuration = 3, downtimeDuration = 3, dureeMvt = 2.5f;
    public Material[] materiauxCouleur;

    [SerializeField, Tooltip("Mettre le conteneur et non les éléments")]
    GameObject[] plateformes, boutonsSelectionPlateformes;
    [SerializeField, ReadOnly] float[] limitesMvtPlateformes = new float[2] { 2, -3 };
    [SerializeField, ReadOnly] int targetPlateform = -1;

    float
        cooldown = 0,
        downtime = 0;
    int targetY = 0;
    bool
        isDown = false,
        isGoingDown = false,
        isUp = true,
        isGoingUp = false;

    private void Awake()
    {
        if (plateformes != null && plateformes.Length == 1)
        {
            GameObject parent = plateformes[0];
            plateformes = new GameObject[parent.transform.childCount];
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                plateformes[i] = parent.transform.GetChild(i).gameObject;
                //Debug.Log(plateformes[i].transform.position);
            }
        }
        if (boutonsSelectionPlateformes != null && boutonsSelectionPlateformes.Length == 1)
        {
            GameObject parent = boutonsSelectionPlateformes[0];
            boutonsSelectionPlateformes = new GameObject[parent.transform.childCount];
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                boutonsSelectionPlateformes[i] = parent.transform.GetChild(i).gameObject;
                //Debug.Log(plateformes[i].transform.position);
            }
        }

        List<Material> listMatsColor = new List<Material>(materiauxCouleur);
        //Debug.Log(listMatsColor.Count);
        for (int i = 0; i < 9; i++)
        {
            int randomIndex = Random.Range(0, listMatsColor.Count);

            plateformes[i].GetComponent<MeshRenderer>().material = listMatsColor[randomIndex];
            boutonsSelectionPlateformes[i].transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().material = listMatsColor[randomIndex];

            listMatsColor.RemoveAt(randomIndex);
        }

        cooldown = cooldownDuration;
        downtime = downtimeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp && cooldown > 0)
        {
            if (cooldown > 0) cooldown = Mathf.Max(cooldown - Time.deltaTime, 0);
            if (cooldown == 0)
            {
                //Debug.Log("Cooldown elapsed");

                isUp = false;
                isGoingDown = true;
                cooldown = cooldownDuration;

                targetY = 1;
                //targetPlateform = Random.Range(0, plateformes.Length);
                //Debug.Log(plateformes[targetPlateform]);
            }
        }
        else if (targetPlateform >= 0 && (isGoingDown || isGoingUp))
        {
            bool isAllDown = true;
            bool isAllUp = true;

            for (int i = 0; i < plateformes.Length; i++)
            {
                if (i == targetPlateform) continue;

                plateformes[i].transform.position = Vector3.MoveTowards(
                    plateformes[i].transform.position,
                    new Vector3(plateformes[i].transform.position.x, limitesMvtPlateformes[targetY], plateformes[i].transform.position.z),
                    dureeMvt * Time.deltaTime);

                if (plateformes[i].transform.position.y != limitesMvtPlateformes[targetY])
                {
                    if (isGoingDown) isAllDown = false;
                    else if (isGoingUp) isAllUp = false;
                }
            }

            if (isGoingDown && isAllDown)
            {
                //Debug.Log("Finished going down now");

                isGoingDown = false;
                isDown = true;
            }
            else if (isGoingUp && isAllUp)
            {
                //Debug.Log("Finished going up now");

                isGoingUp = false;
                isUp = true;
            }
        }
        else if (isDown && downtime > 0)
        {
            if (downtime > 0) downtime = Mathf.Max(downtime - Time.deltaTime, 0);
            if (downtime == 0)
            {
                //Debug.Log("Downtime elapsed");

                isDown = false;
                isGoingUp = true;
                downtime = downtimeDuration;

                targetY = 0;
                //Debug.Log(plateformes[targetPlateform]);
            }
        }
    }
}
