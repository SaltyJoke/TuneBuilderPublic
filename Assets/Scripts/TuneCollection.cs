using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuneCollection : MonoBehaviour
{
    List<string> fragments;
    SpriteRenderer spriteRenderer;
    bool menuOpen;
    Vector3 closedPosition;
    Vector3 openPosition;
    Sprite closedSprite;
    Sprite openSprite;
    GameObject itemGet;

    AudioSource audioSource;
    AudioClip[,] audioClips = new AudioClip[7, 7];
    int currentFragment;

    // Start is called before the first frame update
    void Start()
    {
        fragments = new List<string>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        closedPosition = new Vector3(-8.34f, 4.13f, 0.5f);
        openPosition = new Vector3(0, 0, 0.5f);
        closedSprite = Resources.Load<Sprite>("Visuals/minimized-menu");
        openSprite = Resources.Load<Sprite>("Visuals/tune-menu");
        itemGet = transform.Find("ItemGet").gameObject;

        audioSource = GetComponent<AudioSource>();
        audioClips[1, 0] = Resources.Load<AudioClip>("Audio/var1lh-placeholder");
        audioClips[1, 3] = Resources.Load<AudioClip>("Audio/v1-v2-placeholder");
        audioClips[1, 4] = Resources.Load<AudioClip>("Audio/v1-t-placeholder");
        audioClips[1, 6] = Resources.Load<AudioClip>("Audio/var1-placeholder");
        audioClips[2, 0] = Resources.Load<AudioClip>("Audio/themelh-placeholder");
        audioClips[2, 3] = Resources.Load<AudioClip>("Audio/t-v2-placeholder");
        audioClips[2, 4] = Resources.Load<AudioClip>("Audio/theme-placeholder");
        audioClips[2, 6] = Resources.Load<AudioClip>("Audio/t-v1-placeholder");
        audioClips[3, 0] = Resources.Load<AudioClip>("Audio/var2rh-placeholder");
        audioClips[3, 5] = Resources.Load<AudioClip>("Audio/var2-placeholder");
        audioClips[4, 0] = Resources.Load<AudioClip>("Audio/themerh-placeholder");
        audioClips[4, 5] = Resources.Load<AudioClip>("Audio/v2-t-placeholder");
        audioClips[5, 0] = Resources.Load<AudioClip>("Audio/var2lh-placeholder");
        audioClips[5, 6] = Resources.Load<AudioClip>("Audio/v2-v1-placeholder");
        audioClips[6, 0] = Resources.Load<AudioClip>("Audio/var1rh-placeholder");

        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (menuOpen)
            {
                CloseMenu();
            } else
            {
                OpenMenu();
            }
        }
    }

    void CloseMenu()
    {
        menuOpen = false;
        if (currentFragment != 0)
        {
            TuneFragment frag = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
            frag.ToggleInteraction();
            audioSource.Stop();
            currentFragment = 0;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        spriteRenderer.sprite = closedSprite;
        transform.position = closedPosition;
        Time.timeScale = 1;
    }

    void OpenMenu()
    {
        menuOpen = true;
        spriteRenderer.sprite = openSprite;
        transform.position = openPosition;
        itemGet.SetActive(false);
        ListFragments();
        Time.timeScale = 0;
    }

    private void ListFragments()
    {
        for (int i = 0; i < fragments.Count; i++)
        {
            Transform frag = transform.Find(fragments[i]);
            frag.Translate(Vector3.up * (i * -1.4f - frag.position.y + 3.5f));
            frag.gameObject.SetActive(true);
        }
    }

    public void AddFragment(string frag)
    {
        if (fragments.Contains(frag))
        {
            return;
        }
        fragments.Add(frag);
        itemGet.SetActive(true);
    }

    public void ToggleInteraction(int index)
    {
        if (currentFragment == index)
        {
            currentFragment = 0;
            audioSource.Stop();
        } else
        {
            if (currentFragment != 0)
            {
                TuneFragment frag = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
                frag.ToggleInteraction();
                audioSource.Stop();
            }
            currentFragment = index;
            audioSource.clip = audioClips[index, 0];
            audioSource.Play();
        }
    }
}
