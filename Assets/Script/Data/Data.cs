using System.Collections;
using UnityEngine;

namespace Data
{
    public enum SceneType
    {
        None = -1,

        /// <summary>
        /// Menu, accueil.
        /// </summary>
        Menu,
        /// <summary>
        /// Lobby, salle d'attente, accueil gameshow.
        /// </summary>
        Lobby,

        /// <summary>
        /// Plateformes qui tombent comme mini-jeux champignon de Mario Party.
        /// </summary>
        MarioParty,
        /// <summary>
        /// Duel style cowboy.
        /// </summary>
        Duel,
        /// <summary>
        /// Trouver le trou sur la plateforme.
        /// </summary>
        Crusher,
        /// <summary>
        /// Ça passe ou ça casse.
        /// </summary>
        CPOCC
    }


    public enum Interaction
    {
        None = 0,
        BoutonPlateforme
    }

    public class Question
    {
        public string question;
        public string[] reponses;
        public int indexBonneReponse;

        public Question(string question, string[] reponses, int indexBonneReponse)
        {
            this.question = question;
            this.reponses = reponses;
            this.indexBonneReponse = indexBonneReponse;
        }
    }
}