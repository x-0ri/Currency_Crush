using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitcher : MonoBehaviour
{
    public void SelectScene(string scene)   //utworz klase z wymaganym parametrem typu string
    {
        SceneManager.LoadScene(scene);      //wywolaj funkcje zmieniajaca scene na taka o nazwie podanej wczesniej "scene"
        Debug.Log("Player switched scene to " + scene);
    }

 }
