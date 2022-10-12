using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour
{
    public Color pathColor = Color.white;
    public Transform[] pathPoints;

    public float speed;
    public float rotationSpeed;

    public bool loop = false;

    [Tooltip("O quão perto o objeto precisa chegar perto do waypoint para considerar que ele chegou naquele ponto")]
    private float reachDistance = 0.2f;

    private int currentPathPoint = 0;
    private bool complete = false;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void OnEnable()
    {
        GetComponent<Rigidbody>().useGravity = false;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    public void FixedUpdate()
    {
        if (currentPathPoint < pathPoints.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, pathPoints[currentPathPoint].position, Time.deltaTime * speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, pathPoints[currentPathPoint].rotation, Time.deltaTime * rotationSpeed);

            float distance = Vector3.Distance(transform.position, pathPoints[currentPathPoint].position);
            if (distance <= reachDistance)
            {
                currentPathPoint++;
            }
        }
        else
        {
            if (loop)
            {
                currentPathPoint = 0;
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }
            else
            {
                complete = true;
                GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (pathPoints.Length == 0)
            return;

        for (int i = 0; i < pathPoints.Length; i++)
        {
            Gizmos.color = pathColor;
            Gizmos.DrawWireSphere(pathPoints[i].position, 0.1f);

            if (i < pathPoints.Length - 1)
            {
                Vector3 currentPosition = pathPoints[i].position;
                Vector3 nextPosition = pathPoints[i + 1].position;
                Gizmos.DrawLine(currentPosition, nextPosition);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(pathPoints[i].position, pathPoints[i].forward);

            //float arrowHeadLength = 0.05f;
            //float arrowHeadAngle = 20.0f;
            //Vector3 right = Quaternion.LookRotation(pathPoints[i].forward) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            //Vector3 left = Quaternion.LookRotation(pathPoints[i].forward) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            //Gizmos.DrawRay(pathPoints[i].position + pathPoints[i].forward, right * arrowHeadLength);
            //Gizmos.DrawRay(pathPoints[i].position + pathPoints[i].forward, left * arrowHeadLength);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(pathPoints[i].position, pathPoints[i].up);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(pathPoints[i].position, pathPoints[i].right);
        }
    }

    public bool isComplete()
    {
        return complete;
    }
}
