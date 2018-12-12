using System.Collections;
using System.Collections.Generic;
using AstroWorld.Scenes.Loading;
using AstroWorld.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AstroWorld.Scenes.Main
{
    public class PauseAndResume : MonoBehaviour
    {
        #region Singleton

        public static PauseAndResume instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            if (instance != this)
                Destroy(gameObject);
        }

        #endregion Singleton

        public GameObject pauseMenu;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update
        /// methods is called the first time.
        /// </summary>
        private void Start() => pauseMenu.SetActive(false);

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(Controls.CloseKey))
                PauseGame();
        }

        /// <summary>
        /// Callback sent to all game objects when the player pauses.
        /// </summary>
        /// <param name="pauseStatus">The pause state of the application.</param>
        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                PauseGame();
        }


        public void PauseGame()
        {
            pauseMenu.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            pauseMenu.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1;
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1;
            NextSceneData.sceneToLoad = 0;
            SceneManager.LoadScene(1);
        }
    }
}