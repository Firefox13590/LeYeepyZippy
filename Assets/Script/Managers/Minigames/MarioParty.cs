using Data;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MarioParty : MonoBehaviour
{
    public static MarioParty Instance;

    [Header("Affectation dans l'inspecteur")]
    [SerializeField, Min(1f)]
    float cooldownDuration = 3, downtimeDuration = 3, dureeMvt = 2.5f;
    [SerializeField, Tooltip("Mettre le conteneur et non les �l�ments")]
    GameObject[] plateformes, boutonsSelectionPlateformes;
    [SerializeField] GameObject bordureCouleur;

    [Header("Visualisation des variables")]
    [ReadOnly] public GameObject boutonHighlight = null;
    [SerializeField, ReadOnly] int targetPlateform = -1;

    readonly float[] limitesMvtPlateformes = new float[2] { 2, -3 };
    float
        cooldown = 0,
        downtime = 0;
    int targetY = 0;
    bool
        isDown = false,
        isGoingDown = false,
        isUp = true,
        isGoingUp = false;
    bool canUpdateTargetPlatform = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }



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

                boutonsSelectionPlateformes[i].transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().material = plateformes[i].GetComponent<MeshRenderer>().material;
            }
        }

        cooldown = cooldownDuration;
        downtime = downtimeDuration;
    }
    private void OnEnable()
    {
        InteractionObject.OnInteract += UpdateTargetPlateform;
        GameManager.OnMinigameEnd += () => isUp = isGoingUp = isDown = isGoingDown = false;
        PlayerControls.OnPlayerDeath += AddSpectator;
    }
    private void OnDisable()
    {
        InteractionObject.OnInteract -= UpdateTargetPlateform;
        GameManager.OnMinigameEnd -= () => isUp = isGoingUp = isDown = isGoingDown = false;
        PlayerControls.OnPlayerDeath -= AddSpectator;
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

                canUpdateTargetPlatform = true;
                targetPlateform = -1;

                UpdateBordureCouleur();
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

        for (int i = 0; i < boutonsSelectionPlateformes.Length; i++)
        {
            boutonsSelectionPlateformes[i].transform.GetChild(2).GetChild(0).gameObject.layer = 0;
            if (boutonsSelectionPlateformes[i] == boutonHighlight)
            {
                boutonsSelectionPlateformes[i].transform.GetChild(2).GetChild(0).gameObject.layer = 10;
            }
        }
    }



    void UpdateTargetPlateform(Interaction interaction)
    {
        if (interaction == Interaction.BoutonPlateforme && canUpdateTargetPlatform)
        {
            targetPlateform = Array.IndexOf(boutonsSelectionPlateformes, boutonHighlight);
            boutonsSelectionPlateformes[targetPlateform].GetComponent<Animator>().SetTrigger("PlayAnimation");
            UpdateBordureCouleur();

            canUpdateTargetPlatform = false;

        }
    }

    void UpdateBordureCouleur()
    {
        for (int i = 0; i < bordureCouleur.transform.childCount; i++)
        {
            bordureCouleur.transform.GetChild(i).GetComponent<Image>().color = (targetPlateform >= 0) ?
                boutonHighlight.transform.GetChild(2).GetChild(0).GetComponent<MeshRenderer>().material.color :
                Color.white;
        }
    }

    void AddSpectator(GameObject _)
    {
        GeneralSpawner.Instance.SpawnObject(0, false, false, false, true);
    }
}
