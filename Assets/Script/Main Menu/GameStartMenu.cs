using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject about;

    void Start()
    {
        EnableMainMenu();
    }

    void Update()
    {
        // --- Start game (A on right controller)
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            StartGame();
        }

        // --- Option menu (B on right controller)
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            EnableAbout();
        }

        // --- About (X on left controller)
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            QuitGame();
        }

        // --- Quit game (Y on left controller OR Menu button)
        if (OVRInput.GetDown(OVRInput.Button.Four) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            EnableMainMenu();
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        SceneTransitionManager.singleton.StartCoroutine(
        SceneTransitionManager.singleton.LoadSceneRoutine(2)
        );

    }

    public void HideAll()
    {
        mainMenu.SetActive(false);

        about.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        about.SetActive(false);
    }



    public void EnableAbout()
    {
        mainMenu.SetActive(false);
        about.SetActive(true);
    }
}
