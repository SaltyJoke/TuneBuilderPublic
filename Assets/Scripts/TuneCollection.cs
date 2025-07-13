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
    // Start is called before the first frame update
    void Start()
    {
        fragments = new List<string>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        closedPosition = new Vector3(-8.34f, 4.13f, 0.5f);
        openPosition = new Vector3(0, 0, 0.5f);
        closedSprite = Resources.Load<Sprite>("Visuals/cd-player-placeholder");
        openSprite = Resources.Load<Sprite>("Visuals/tune-menu");
        itemGet = transform.Find("ItemGet").gameObject;
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

    public void CloseMenu()
    {
        menuOpen = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        spriteRenderer.sprite = closedSprite;
        transform.position = closedPosition;
        Time.timeScale = 1;
    }

    public void OpenMenu()
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
            frag.Translate(Vector3.up * (i * -1.5f - frag.position.y));
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
}
