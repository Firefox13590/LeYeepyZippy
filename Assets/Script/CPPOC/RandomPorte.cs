using TMPro;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.UI;

public class RandomPorte : MonoBehaviour
{
    public GameObject questions;
    public TMP_Text question;
    public TMP_Text rep1;
    public TMP_Text rep2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        question = GetComponent<TMP_Text>();
        rep1 = GetComponent<TMP_Text>();
        rep2 = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    void question1()
    {

    }
}
