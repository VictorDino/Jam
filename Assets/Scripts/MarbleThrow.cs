using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleThrow : MonoBehaviour
{
    public float power = 10f;
    Rigidbody2D rb;

    public Vector2 minPower;
    public Vector2 maxPower;

    Camera cam;
    Vector2 force;

    bool throwed = false;

    public GameObject goalObj;
    CheckGoal checkGoal;

    Animator animator;

    LineRenderer lr;

    int throwPhase = 0;

    bool isDragging;
    Vector3 throwDir;

    void Start()
    {
        cam = Camera.main;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        animator = this.gameObject.GetComponent<Animator>();
        lr = this.gameObject.GetComponent<LineRenderer>();

        checkGoal = goalObj.GetComponent<CheckGoal>();

        throwed = false;
        throwPhase = 0;
    }

    void Update()
    {
        if (!throwed)
        {
            if (Input.GetMouseButtonDown(1))
            {
                throwPhase++;
            }

            if (throwPhase == 0 && isDragging)
            {
                Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                transform.Translate(0, mousePosition.y, 0);
            }
            if (this.transform.position.y < -150)
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x, -150, this.transform.position.z);
            }
            else if (this.transform.position.y > 50)
            {
                this.gameObject.transform.position = new Vector3(this.transform.position.x, 50, this.transform.position.z);
            }

            if (throwPhase == 1)
            {
                DrawLine();
            }
            else { lr.enabled = false; }

            if (throwPhase == 2)
            {
                force = new Vector2(Mathf.Clamp(throwDir.x, minPower.x, maxPower.x), Mathf.Clamp(throwDir.y, minPower.y, maxPower.y));
                rb.AddForce(force * power, ForceMode2D.Impulse);
                throwed = true;
            }
        }

        if (throwed && rb.velocity.x >= -1 && rb.velocity.x <= 1 && rb.velocity.y >= -1 && rb.velocity.y <= 1)
        {
            rb.velocity = new Vector2(0, 0);
            int score = checkGoal.GetScore();
            Debug.Log(score);
        }

        //ANIMATION
        if (rb.velocity.magnitude < 5)
        {
            animator.SetBool("fastSpin", false);
            animator.SetBool("slowSpin", false);
        }
        else if (rb.velocity.magnitude > 0 && rb.velocity.magnitude < 200)
        {
            animator.SetBool("fastSpin", false);
            animator.SetBool("slowSpin", true);
        }
        else if (rb.velocity.magnitude > 100)
        {
            animator.SetBool("fastSpin", true);
            animator.SetBool("slowSpin", false);
        }
    }

    private void OnMouseDown() {
        if (throwPhase == 0) { isDragging = true; }
    }
    private void OnMouseUp() {
        if (throwPhase == 0) { isDragging = false; }
    }

    void DrawLine()
    {
        lr.enabled = true;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDir = mousePos - this.gameObject.transform.position;
        mouseDir.z = 0;
        mouseDir = mouseDir.normalized;

        Vector3 startPos = gameObject.transform.position;
        startPos.z = 0;
        lr.SetPosition(0, startPos);
        Vector3 endPos = mousePos;
        endPos.z = 0;
        float capLength = Mathf.Clamp(Vector2.Distance(startPos, endPos), 0, 100);
        endPos = startPos + (mouseDir * capLength);
        lr.SetPosition(1, endPos);
        throwDir = (endPos - startPos).normalized;
    }
}
