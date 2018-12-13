using System.Collections;
using System.Collections.Generic;
using AstroWorld.Utils;
using UnityEngine;

namespace AstroWorld.Scenes.Home
{
    public class MenuManager : MonoBehaviour
    {
        #region Singleton

        public static MenuManager instance;

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

        public GameObject mainMenu;
        public GameObject helpMenu;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start() => helpMenu.SetActive(false);

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(Controls.CloseKey))
                CloseHelp();
        }

        public void OpenHelp()
        {
            mainMenu.SetActive(false);
            helpMenu.SetActive(true);

        }

        public void CloseHelp()
        {
            mainMenu.SetActive(true);
            helpMenu.SetActive(false);
        }
    }
}