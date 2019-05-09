using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Toolbox;

public enum HeroStatus { None, HeroIdle, HeroRun, HeroAttack, HeroDie };

public class HeroController : MonoBehaviour
{
    HeroStatus status = HeroStatus.None;
    readonly float maxDistanceDelta = 0.1f;
    List<Vector3> path = new List<Vector3>();
    Vector3 nextPoint;

    new SpriteRenderer renderer;
    new Rigidbody2D rigidbody;
    Animator animator;
    Tilemap wall;

    HeroStatus Status
    {
        get => status;
        set
        {
            status = value;
            print(status);
            animator.SetInteger("HeroStatus", (int)status);
        }
    }

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        wall = GameObject.Find("Wall").GetComponent<Tilemap>();
    }

    void Start()
    {
        Status = HeroStatus.HeroIdle;
        nextPoint = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Status == HeroStatus.HeroIdle)
                FindPath();
            else
                Stop();
        }

        Move();
    }

    void FindPath()
    {
        // 경로를 설정한다.
        Vector3 from = transform.position;
        Vector3 to = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        path = AStar.FindPath(wall, from, to);
        print("FindPath from " + from + " to " + to + ": " + string.Join(", ", path));

        if (path != null && path.Count > 1)
        {
            // 경로의 첫번째 값은 현재 위치이므로 제외한다.
            path.RemoveAt(0);
            Status = HeroStatus.HeroRun;
        }
    }

    void Stop()
    {
        // 셀의 중심으로 옮긴다.
        Vector3Int cellPosition = wall.WorldToCell(transform.position);
        rigidbody.MovePosition(wall.GetCellCenterWorld(cellPosition));

        // 이동을 멈춘다.
        Status = HeroStatus.HeroIdle;
        nextPoint = transform.position;
        path.Clear();
    }

    void SetNext()
    {
        if (path.Count > 0)
        {
            nextPoint = path[0];
            path.RemoveAt(0);
            renderer.flipX = (nextPoint - transform.position).x < 0;
        }
        else
            Stop();
    }

    void Move()
    {
        if (Status == HeroStatus.HeroRun)
        {
            if (Vector3.Distance(nextPoint, transform.position) < float.Epsilon)
            {
                SetNext();
            }
            else
            {
                Vector3 position = Vector3.MoveTowards(transform.position, nextPoint, maxDistanceDelta);
                rigidbody.MovePosition(position);
            }
        }
    }
}
