using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example : MonoBehaviour
{
    public Image image;
    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void updateImage()
    {
        gameObject.SetActive(true);
        Sprite new_sprite = Resources.Load("Graphics/PuzzleCat/Verbs/Pic6") as Sprite;
        image.sprite = new_sprite;
    }
}
