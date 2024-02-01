using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] GameObject m_Enemy;
    [SerializeField] [Range(0f,10f)] float movementSpeed = 1f;
    [SerializeField] [Range(0f,10f)] float rotationSpeed = 1f;

    List<Node> path = new List<Node>();

    Enemy enemy;
    WaveHandler waveHandler;
    GridManager gridManager;
    Pathfinder pathfinder;

    void OnEnable()
    {
        SpawnPoint();
        RecalculatePath(true);
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        waveHandler = FindObjectOfType<WaveHandler>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if (resetPath)
        {
            coordinates = pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();

        path.Clear();
        path = pathfinder.GetNewPath(coordinates);

        StartCoroutine(FollowPath());
    }

    private void SpawnPoint()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    IEnumerator FollowPath()
    {
        for(int i = 1; i < path.Count; i++)
        {
            Vector3 currentPos = transform.position;
            Vector3 nextPos = gridManager.GetPositionFromCoordinates(path[i].coordinates);

            float travelAmount = 0f;
            float rotationDegrees = degreesToRotate(m_Enemy.transform.forward, currentPos, nextPos);

            while (travelAmount < 1f)
            {
                if (travelAmount < 0.25f)
                {
                    float rotationAngle = 4 * rotationDegrees * Time.deltaTime * rotationSpeed;
                    m_Enemy.transform.Rotate(new Vector3(0, rotationAngle, 0));
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
        waveHandler.LowerAliveCount();
        gameObject.SetActive(false);
    }

    
}
