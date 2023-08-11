using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private GameObject target;
    Rigidbody2D rb;
    private float _thrustspeed = 5f;
    private float _rotationspeed = 75f;
    [SerializeField]
    private GameObject _explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Enemy");

        StartCoroutine(FindNewTarget());
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            var dir = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            var rotateToTarget = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * _rotationspeed);
            rb.velocity = new Vector2(dir.x * _thrustspeed, dir.y * _thrustspeed);

        }
        else
        {
            transform.Translate(Vector3.forward * _thrustspeed * Time.deltaTime);
        }

        BoundaryCheck();

    }

    void BoundaryCheck()
    {
        if (transform.position.z > 1 || transform.position.z < -1)
        {
            Explosion();
        }
    }

    public void Explosion()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    IEnumerator FindNewTarget()
    {
        while (target == null)
        {
            yield return new WaitForSeconds(.2f);
            target = GameObject.FindWithTag("Enemy");
        }
    }
}
