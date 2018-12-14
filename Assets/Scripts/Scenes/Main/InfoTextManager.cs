using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AstroWorld.Scenes.Main
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Text))]
    public class InfoTextManager : MonoBehaviour
    {
        #region Singleton

        public static InfoTextManager instance;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (instance == null)
                instance = this;

            if (instance != this)
                Destroy(gameObject);
        }

        #endregion Singleton

        private Animator infoTextAnimator;
        private Text infoText;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            infoTextAnimator = GetComponent<Animator>();
            infoText = GetComponent<Text>();
        }

        private const string FadeInOut = "Display Text";

        public void DisplayText(string text, Color textColor)
        {
            infoText.text = text;
            infoText.color = textColor;

            infoTextAnimator.SetTrigger(FadeInOut);
        }
    }
}