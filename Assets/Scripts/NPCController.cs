using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameManager gm;
    AStar aStar = new AStar();
    List<Node> path = new List<Node>();
    public float moveSpeed = 3f;
    private Animator animator;

    void Start()
    {
        if (gm == null)
            gm = FindFirstObjectByType<GameManager>();

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gm.destination != null)
        { 
            aStar.RunAStar(gm.start, gm.destination, gm.tiles);
            gm.destination = null;
        }

        if (!aStar.isRunning && aStar.path.Count > 0)
        {
            path = new List<Node>(aStar.path);
            aStar.path.Clear();
        }

        if (path.Count > 0)
        {
            Node targetNode = path[0];
            Vector3 targetPos = targetNode.gridPosition;

            targetPos.y = transform.position.y;

            Vector3 direction = (targetPos - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            targetNode.isStart = true;
            for (int i = 1; i < targetNode.neighbours.Count; i++)
            {
                targetNode.neighbours[i].isStart = false;
            }
            gm.SetNewStart(targetNode);
            Debug.Log("Moving to: " + targetPos);

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

                if (animator != null)
                    animator.SetBool("IsMoving", true);
            }

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                path.RemoveAt(0);
                if (path.Count == 0)
                {
                    gm.SetNewStart(targetNode);

                    if (animator != null)
                        animator.SetBool("IsMoving", false);
                }
            }
        }
    }
}
