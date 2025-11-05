using UnityEngine;
using UnityEngine.Animations;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress1, keyToPress2, keyToPress3, keyToPress4;

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (Input.GetKeyDown(keyToPress1) || Input.GetKeyDown(keyToPress2) || Input.GetKeyDown(keyToPress3) || Input.GetKeyDown(keyToPress4))
        {
            theSR.sprite = pressedImage;
        }

        if (Input.GetKeyUp(keyToPress1) || Input.GetKeyUp(keyToPress2) || Input.GetKeyUp(keyToPress3) || Input.GetKeyUp(keyToPress4))
        {
            theSR.sprite = defaultImage;    
        }
    }
}
