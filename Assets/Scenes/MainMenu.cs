using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string BattleScene;

    public bool ToggleMusic = true;
    public Image image;
    public Sprite onSprite;
    public Sprite offSprite;

    public AudioMixerSnapshot snapshotOn;
    public AudioMixerSnapshot snapshotOff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(BattleScene);
    }

    public void Toggle()
    {
        ToggleMusic = !ToggleMusic;
        image.sprite = ToggleMusic ? onSprite : offSprite;
        if (ToggleMusic)
        {
            snapshotOn.TransitionTo(.1f);
        }
        else
        {
            snapshotOff.TransitionTo(.1f);
        }
    }
}
