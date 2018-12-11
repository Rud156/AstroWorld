using System.Collections;
using System.Collections.Generic;
using AstroWorld.Scenes.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AstroWorld.Scenes.Home
{
    public class PlayAndQuit : MonoBehaviour
    {
        public void PlayGame()
        {
            NextSceneData.sceneToLoad = 2;
            NextSceneData.displayInfo = false;
            SceneManager.LoadScene(1);
        }

        public void QuitGame() => Application.Quit();
    }
}