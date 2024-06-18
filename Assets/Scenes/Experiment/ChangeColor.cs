using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    CircleCollider2D cc;
    List<GameObject> victims = new List<GameObject>();


    [SerializeField]
    float power;

    private void Start()
    {
        cc = GetComponent<CircleCollider2D>();
        cc.enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;    // Z 座標がカメラと同じになっているので、リセットする]
            this.transform.position = mousePosition;
            cc.enabled = true;
            StartCoroutine(WaitAndExplode());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        victims.Remove(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        victims.Add(collision.gameObject);
    }


    IEnumerator WaitAndExplode()
    {
        yield return new WaitForSeconds(0.1f);
        if (victims.Count > 0)
        {
            foreach (GameObject obj in victims)
            {
                Vector2 facing = (obj.transform.position - transform.position).normalized;
                if (obj.GetComponent<Rigidbody2D>() != null)
                {
                    Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                    rb.velocity = facing * power;
                    Debug.Log(facing * power);
                }
            }
        }
        cc.enabled = false;
    }
}
