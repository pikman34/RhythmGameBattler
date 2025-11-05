using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool canBePressed;
    public KeyCode keyToPress1, keyToPress2, keyToPress3, keyToPress4;

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    private bool obtained, missed;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress1) || Input.GetKeyDown(keyToPress2) || Input.GetKeyDown(keyToPress3) || Input.GetKeyDown(keyToPress4))
        {
            if (canBePressed)
            {
                if (Mathf.Abs(transform.position.y) > 0.25)
                {
                    GameManager.instance.NormalHit();
                    Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                }
                else if (Mathf.Abs(transform.position.y) > 0.05f)
                {
                    GameManager.instance.GoodHit();
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                }
                else
                {
                    GameManager.instance.PerfectHit();
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                }
                
                obtained = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canBePressed = false;
            if (!obtained && !missed)
            {
                GameManager.instance.NoteMissed();
                missed = true;
                Instantiate(missEffect, transform.position, missEffect.transform.rotation);
            }
        }
    }
}
