using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource Menu_Click;
    public void Play_Menu_Click()
    {
        //Menu_Click.Play();
        Menu_Click.PlayOneShot(Menu_Click.clip);
    }

}
