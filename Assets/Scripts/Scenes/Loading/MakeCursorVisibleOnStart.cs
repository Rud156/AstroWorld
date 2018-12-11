using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AstroWorld.Scenes.Loading
{
    public class MakeCursorVisibleOnStart : MonoBehaviour
    {
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}