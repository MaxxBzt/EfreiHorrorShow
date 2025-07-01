using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject options;
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
            EnableOption();
        }

        // --- About (X on left controller)
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            EnableAbout();
        }

        // --- Quit game (Y on left controller OR Menu button)
        if (OVRInput.GetDown(OVRInput.Button.Four) || OVRInput.GetDown(OVRInput.Button.Start))
        {
            QuitGame();
        }

        // --- Return to main menu: you can pick a button (here, B or Y as examples)
        // (Comment this out if you want to control page return in a different way)
        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four))
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
        // Replace with your scene transition logic
        SceneTransitionManager.singleton.GoToSceneAsync(0);
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        about.SetActive(false);
    }

    public void EnableOption()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
        about.SetActive(false);
    }

    public void EnableAbout()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(true);
    }
}
