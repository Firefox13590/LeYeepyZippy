using Data;
using TMPro;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.UI;

public class RandomPorte : MonoBehaviour
{
    //public GameObject questions;
    public TMP_Text question;
    public TMP_Text rep1;
    public TMP_Text rep2;

    public GameObject GAquestions;

    Question[] questions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        question = GetComponent<TMP_Text>();
        rep1 = GetComponent<TMP_Text>();
        rep2 = GetComponent<TMP_Text>();

        questions = new Question[10]
        {
            new Question("Pourquoi Mario est cool?", new string[] {"Il est rouge", "Il est vert"}, 0),
            new Question("Qui est Steve?", new string[] {"The guy from Fortnite", "Le gars de Minecraft"}, 0),
            new Question("Spider-Man:...", new string[] {"Never Home", "No Way Home"}, 1),
            new Question("Quel est la plus grande montagne?", new string[] {"Burj Kahlifa", "Mont Everest"}, 1),
            new Question("Quelle est la plus grande planète?", new string[] {"Jupiter", "Ta mère"}, 0),
            new Question("Le plus grand océan?", new string[] {"La fuite d'eau de Maisonneuve", "Océan Pacifique"}, 1),
            new Question("Quelle est la capitale du Canada?", new string[] {"Québéec", "Ottawa"}, 1),
            new Question("Quand était les Olympiques de Montréal?", new string[] {"1969", "1967"}, 1),
            new Question("Le nom de la plateforme la plus populaire de streaming?", new string[] {"Twitch", "Bilibili"}, 0),
            new Question("Pourquoi Mario est cool", new string[] {"Il est rouge", "Il est vert"}, 0)
        };
    }

    // Update is called once per frame
    void Update()
    {
        //Invoke("spawnPorte", 1f);
    }

    

    void question1()
    {
        string question1 = "Pourquoi Mario est cool?";
        string[] rep1, rep2;
        int indexbonnereponse;

        Debug.Log(questions[0].question);
        Debug.Log(questions[0].reponses[0]);
        Debug.Log(questions[0].indexBonneReponse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("trigger"))
        {
            Instantiate(GAquestions, new Vector3(1.99f, 0, 3f), Quaternion.identity);
        }
    }
}
