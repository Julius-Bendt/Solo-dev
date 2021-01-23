using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed;

    private Transform point;
    public Transform[] points;

    private float idleTime = 5;
    private float timer;

    Quaternion startRot;

    public AnimationCurve curve;

    private Animator anim;


    public void Goto(Transform _point)
    {
        point = _point;
        timer = 0;
        anim.SetBool("desk", false);
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(GetPoint());
    }

    private void Update()
    {
        if(point != null)
        {

            transform.Translate(Vector2.right * speed * Time.deltaTime);
            anim.SetFloat("speed", 1);

            if (Vector2.Distance(transform.position, point.position) < 0.25f)
            {
                StartCoroutine(Rotate());
            }
            else
            {
                LookAt(point.position);
            }
        }
    }

    public void LookAt(Vector2 lookAt)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 lookDir = lookAt - pos;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D o)
    {
        if(o.tag == "desk")
        {
            anim.SetBool("desk", true);

        }
    }

    IEnumerator GetPoint()
    {
        while(true)
        {
            if(point == null)
            {

                float max = (anim.GetBool("desk")) ? 15f : 5f;

                idleTime = Random.Range(2.5f, max);
                anim.SetFloat("speed", 0);
                yield return new WaitForSeconds(idleTime);

                Goto(points[Random.Range(0, points.Length - 1)]);
            }

            yield return null;
        }
    }

    IEnumerator Rotate()
    {
        anim.SetFloat("speed", 0);
        float ElapsedTime = 0.0f;
        float time = 0.5f;
        Quaternion startRot = transform.rotation, to = point.rotation;

        point = null;
        while (ElapsedTime < time)
        {

            ElapsedTime += Time.deltaTime;
            float value = (curve == null) ? (ElapsedTime / time) : curve.Evaluate((ElapsedTime / time));
            transform.rotation = Quaternion.Lerp(startRot, to, value);
            yield return null;
        }
    }
}
