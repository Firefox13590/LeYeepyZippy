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

    bool peutUpdateAuto;
    TextMeshProUGUI text;

    private void Awake()
    {
        peutUpdateAuto = estCompteurAuto;
        valeur = depart;
        text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        text.text = depart.ToString();
        if (estCompteurAuto) InvokeRepeating(nameof(UpdateCompteur), delaiPremiereVariation, delaiEntreVariations);
    }



    public void UpdateCompteur()
    {
        valeur += variation;
        text.text = valeur.ToString();
        if (valeur == fin)
        {
            CancelInvoke(nameof(UpdateCompteur));
            GameManager.Instance.MinigameEnd();
        }
    }
}
