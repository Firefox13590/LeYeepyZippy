using NaughtyAttributes;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Compteur : MonoBehaviour
{
    [SerializeField] bool estCompteurAuto = true;
    [SerializeField]
    int
        depart = 10,
        fin = 0,
        variation = -1;
    [SerializeField]
    float
        delaiPremiereVariation = 1,
        delaiEntreVariations = 1;

    [SerializeField, ReadOnly] int valeur;

    TextMeshProUGUI text;

    private void Awake()
    {
        valeur = depart;
        text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        text.text = depart.ToString();
        if (estCompteurAuto) StartCompteur(delaiPremiereVariation, delaiEntreVariations);
    }



    public void StartCompteur(float premierDelai = 1, float delaiRepetitif = 1)
    {
        InvokeRepeating(nameof(UpdateCompteur), premierDelai, delaiRepetitif);
    }
    public void WriteCompteur(object obj)
    {
        text.text = obj.ToString();
    }
    public void UpdateCompteur()
    {
        valeur += variation;
        WriteCompteur(valeur);
        if (valeur == fin)
        {
            CancelInvoke(nameof(UpdateCompteur));
            GameManager.Instance.MinigameEnd();
        }
    }
    public void ResetCompteur()
    {
        valeur = depart;
        CancelInvoke(nameof(UpdateCompteur));
    }
    public void RestartCompteur()
    {
        ResetCompteur();
        StartCompteur(delaiPremiereVariation, delaiEntreVariations);
    }
}
