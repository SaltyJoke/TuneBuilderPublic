using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FragmentType
{
    LEFT, RIGHT, COMBINED
};

public class TuneCollection : MonoBehaviour
{
    enum MenuState
    {
        CLOSED, FRAGMENT, TUNE
    };
    MenuState state;
    SpriteRenderer spriteRenderer;
    Vector3 closedPosition;
    Vector3 openPosition;
    Sprite closedSprite;
    Sprite fragmentMenuSprite;
    Sprite tuneMenuSprite;
    GameObject itemGet;
    GameObject setBGMPrompt;

    AudioSource audioSource;
    AudioClip bgm;

    List<int> fragments;
    int currentFragment = -1;
    int[] fragmentSlots;
    Vector3[] slotPositions;
    int[,] recipe;
    List<int> tunes;

    bool allowSetBGM;

    public static TuneCollection Instance;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fragments = new List<int>();
        fragmentSlots = new int[] { -1, -1, -1 };
        slotPositions = new Vector3[]
        {
            new Vector3(3.82f, 3.19f, -1f),
            new Vector3(3.82f, 0.42f, -1f),
            new Vector3(3.82f, -2.48f, -1f)
        };
        recipe = new int[,]
        {
            { -1, -1, 12, 11, -1, 7 },
            { -1, -1, 10, 6, -1, 9 },
            { 12, 10, -1, -1, 8, -1 },
            { 11, 6, -1, -1, 13, -1 },
            { -1, -1, 8, 13, -1, 14 },
            { 7, 9, -1, -1, 14, -1 }
        };
        tunes = new List<int>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        closedPosition = new Vector3(-8.34f, 4.13f, 0.5f);
        openPosition = new Vector3(0, 0, 0.5f);
        closedSprite = Resources.Load<Sprite>("Visuals/minimized-menu");
        fragmentMenuSprite = Resources.Load<Sprite>("Visuals/tune-fragment-menu");
        tuneMenuSprite = Resources.Load<Sprite>("Visuals/tune-menu");
        itemGet = transform.Find("ItemGet").gameObject;
        setBGMPrompt = transform.Find("SetBGMPrompt").gameObject;

        audioSource = GetComponent<AudioSource>();

        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MenuState.CLOSED:
                if (Input.GetKeyUp(KeyCode.E))
                {
                    OpenFragmentMenu();
                }
                break;
            case MenuState.FRAGMENT:
                if (Input.GetKeyUp(KeyCode.D))
                {
                    SelectIngredient();
                }
                if (Input.GetKeyUp(KeyCode.A))
                {
                    RemoveIngredient();
                }
                if (Input.GetKeyUp(KeyCode.C))
                {
                    AddTune();
                }
                if (Input.GetKeyUp(KeyCode.E))
                {
                    CloseMenu();
                }
                break;
            case MenuState.TUNE:
                if (Input.GetKeyUp(KeyCode.C) && allowSetBGM)
                {
                    SetBGM();
                }
                if (Input.GetKeyUp(KeyCode.E))
                {
                    CloseMenu();
                }
                break;
        }
    }

    void CloseMenu()
    {
        if (currentFragment != -1)
        {
            TuneFragment frag = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
            frag.ToggleInteraction();
            if (audioSource.clip != bgm)
            {
                audioSource.Stop();
            }
            currentFragment = -1;
        }
        if (state == MenuState.FRAGMENT)
        {
            ClearIngredients();
        }
        for (int i = 0; i < transform.childCount - 2; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if (bgm != null)
        {
            audioSource.clip = bgm;
            audioSource.Play();
        }
        allowSetBGM = false;
        spriteRenderer.sprite = closedSprite;
        transform.position = closedPosition;
        state = MenuState.CLOSED;
        EventManager.Mode = GameMode.PLAY;
    }

    public void OpenFragmentMenu()
    {
        if (state == MenuState.TUNE)
        {
            if (currentFragment != -1)
            {
                TuneFragment frag = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
                frag.ToggleInteraction();
                ToggleInteraction(currentFragment);
            }
            for (int i = 0; i < tunes.Count; i++)
            {
                transform.GetChild(tunes[i]).gameObject.SetActive(false);
            }
            setBGMPrompt.SetActive(false);
        }
        audioSource.Stop();
        spriteRenderer.sprite = fragmentMenuSprite;
        transform.position = openPosition;
        itemGet.SetActive(false);
        ListFragments();
        state = MenuState.FRAGMENT;
        EventManager.Mode = GameMode.PAUSE;
    }

    public void OpenTuneMenu(bool cdPlayer = false)
    {
        switch (state)
        {
            case MenuState.CLOSED: // only possible from CD player
                allowSetBGM = true;
                setBGMPrompt.SetActive(true);
                itemGet.SetActive(false);
                break;
            case MenuState.FRAGMENT:
                ClearIngredients();
                if (currentFragment != -1)
                {
                    TuneFragment frag = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
                    frag.ToggleInteraction();
                    ToggleInteraction(currentFragment);
                }
                for (int i = 0; i < fragments.Count; i++)
                {
                    transform.GetChild(fragments[i]).gameObject.SetActive(false);
                }
                setBGMPrompt.SetActive(allowSetBGM);
                break;
            default:
                break;
        }
        audioSource.Stop();
        spriteRenderer.sprite = tuneMenuSprite;
        transform.position = openPosition;
        ListTunes();
        state = MenuState.TUNE;
        EventManager.Mode = GameMode.PAUSE;
    }

    private void ListFragments()
    {
        for (int i = 0; i < fragments.Count; i++)
        {
            Transform frag = transform.GetChild(fragments[i]);
            frag.Translate(new Vector3(-4.5f, i * -1.4f + 3.5f, -1) - frag.position);
            frag.gameObject.SetActive(true);
        }
    }

    private void ListTunes()
    {
        for (int i = 0; i < Math.Min(6, tunes.Count); i++)
        {
            Transform frag = transform.GetChild(tunes[i]);
            frag.Translate(new Vector3(-4.5f, i * -1.4f + 3.5f, -1) - frag.position);
            frag.gameObject.SetActive(true);
        }
        for (int i = 6; i < tunes.Count; i++)
        {
            Transform frag = transform.GetChild(tunes[i]);
            frag.Translate(new Vector3(0f, (i - 6) * -1.4f + 3.5f, -1) - frag.position);
            frag.gameObject.SetActive(true);
        }
    }

    private void SetBGM()
    {
        if (currentFragment != -1)
        {
            TuneFragment frag = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
            bgm = frag.audioClip;
        }
    }

    public void AddFragment(int frag)
    {
        if (fragments.Contains(frag))
        {
            return;
        }
        fragments.Add(frag);
        itemGet.SetActive(true);
    }

    void SelectIngredient()
    {
        if (currentFragment == -1)
        {
            return;
        }
        TuneFragment frag = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
        if (fragmentSlots[0] == -1)
        {
            InsertInSlot(0, currentFragment);
        } else if (fragmentSlots[1] == -1)
        {
            TuneFragment firstSlot = (TuneFragment)(transform.GetChild(fragmentSlots[0]).gameObject.GetComponent<MonoBehaviour>());
            if (firstSlot.fragmentType != frag.fragmentType)
            {
                InsertInSlot(1, currentFragment);
                InsertInSlot(2, recipe[fragmentSlots[0], fragmentSlots[1]]);
            }
        }
    }

    void InsertInSlot(int slot, int index)
    {
        fragmentSlots[slot] = index;
        Transform frag = transform.GetChild(index);
        frag.position = slotPositions[slot];
        frag.gameObject.SetActive(true);
        if (fragments.Contains(index))
        {
            fragments.Remove(index);
            ListFragments();
        }
    }

    void RemoveIngredient()
    {
        if (fragmentSlots[0] == currentFragment)
        {
            fragments.Add(fragmentSlots[0]);
            fragmentSlots[0] = -1;
            ListFragments();
            if (fragmentSlots[1] != -1)
            {
                InsertInSlot(0, fragmentSlots[1]);
                fragmentSlots[1] = -1;
                Transform frag = transform.GetChild(fragmentSlots[2]);
                frag.gameObject.SetActive(false);
                fragmentSlots[2] = -1;
            }
        } else if (fragmentSlots[1] == currentFragment)
        {
            fragments.Add(fragmentSlots[1]);
            fragmentSlots[1] = -1;
            ListFragments();
            Transform frag = transform.GetChild(fragmentSlots[2]);
            frag.gameObject.SetActive(false);
            fragmentSlots[2] = -1;
        }
    }

    void AddTune()
    {
        if (fragmentSlots[2] != -1 && !tunes.Contains(fragmentSlots[2]))
        {
            tunes.Add(fragmentSlots[2]);
        }
    }

    void ClearIngredients()
    {
        int temp = currentFragment;
        currentFragment = 1;
        if (fragmentSlots[1] != -1)
        {
            currentFragment = fragmentSlots[1];
            RemoveIngredient();
        }
        if (fragmentSlots[0] != -1)
        {
            currentFragment = fragmentSlots[0];
            RemoveIngredient();
        }
        currentFragment = temp;
    }

    public void ToggleInteraction(int index)
    {
        if (currentFragment == index)
        {
            currentFragment = -1;
            audioSource.Stop();
        } else
        {
            if (currentFragment != -1)
            {
                TuneFragment frag = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
                frag.ToggleInteraction();
                audioSource.Stop();
            }
            currentFragment = index;
            TuneFragment fragment = (TuneFragment)(transform.GetChild(currentFragment).gameObject.GetComponent<MonoBehaviour>());
            audioSource.clip = fragment.audioClip;
            audioSource.Play();
        }
    }
}
