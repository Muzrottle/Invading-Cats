using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f,10f)] float movementSpeed = 1f;
    [SerializeField] [Range(0f,10f)] float rotationSpeed = 1f;

    Enemy enemy;

    void OnEnable()
    {
        FindPath();
        SpawnPoint();
        StartCoroutine(FollowPath());
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void FindPath()
    {
        path.Clear();

        GameObject parent = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform child in parent.transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();

            if (waypoint != null)
            {
                path.Add(child.GetComponent<Waypoint>());
            }
        }
    }

    private void SpawnPoint()
    {
        transform.position = path[0].transform.position;
    }

    IEnumerator FollowPath()
    {
        foreach (Waypoint waypoint in path)
        {
            Vector3 currentPos = transform.position;
            Vector3 nextPos = waypoint.transform.position;

            float travelAmount = 0f;
            float rotationDegrees = degreesToRotate(transform.forward, currentPos, nextPos);

            while (travelAmount < 1f)
            {
                if (travelAmount < 0.25f)
                {
                    float rotationAngle = 4 * rotationDegrees * Time.deltaTime * rotationSpeed;
                    transform.Rotate(new Vector3(0, rotationAngle, 0));
                }

                travelAmount += Time.deltaTime * movementSpeed;
                transform.position = Vector3.Lerp(currentPos, nextPos, travelAmount);

                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    float degreesToRotate(Vector3 currentDir, Vector3 currentPosition, Vector3 endPosition)
    {
        Vector3 targetDir = endPosition - currentPosition;
        return Vector3.SignedAngle(currentDir, targetDir, Vector3.up);
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false);
    }

    
}
