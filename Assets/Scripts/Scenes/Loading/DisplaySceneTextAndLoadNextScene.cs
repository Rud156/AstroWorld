using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AstroWorld.Scenes.Loading
{
    public class DisplaySceneTextAndLoadNextScene : MonoBehaviour
    {
        public float waitForDisplayTime = 3;
        public GameObject infoText;
        public Slider loadingSlider;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            if (NextSceneData.displayInfo)
            {
                infoText.SetActive(true);
                if (NextSceneData.missionComplete)
                    infoText.GetComponent<Text>().text = "Safely Returned To Base ...";
                else
                    infoText.GetComponent<Text>().text = "You Died !!!";
            }
            else
            {
                infoText.SetActive(false);
                infoText.GetComponent<Text>().text = "";
            }

            StartCoroutine(LoadNextSceneAsync());
        }

        IEnumerator LoadNextSceneAsync()
        {
            loadingSlider.value = 0;
            if (NextSceneData.displayInfo)
                yield return new WaitForSeconds(waitForDisplayTime);

            int sceneIndex = NextSceneData.sceneToLoad;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                loadingSlider.value = operation.progress;
                yield return null;
            }
        }
    }
}