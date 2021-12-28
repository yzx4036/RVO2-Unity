using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;
using Vector2 = RVO.Vector2;

public class ObstacleCollect : MonoBehaviour
{
    private static IList<Vector2> ComputesBoxCollider2Vector2(BoxCollider boxCollider)
    {
        var transform1 = boxCollider.transform;
        var position = transform1.position;
        var size = boxCollider.size;
        var lossyScale = transform1.lossyScale;
        float minX = position.x -
                     size.x * lossyScale.x * 0.5f;
        float minZ = position.z -
                     size.z * lossyScale.z * 0.5f;
        float maxX = position.x +
                     size.x * lossyScale.x * 0.5f;
        float maxZ = position.z +
                     size.z * lossyScale.z * 0.5f;

        IList<Vector2> obstacle = new List<Vector2>();
        obstacle.Add(new Vector2(maxX, maxZ));
        obstacle.Add(new Vector2(minX, maxZ));
        obstacle.Add(new Vector2(minX, minZ));
        obstacle.Add(new Vector2(maxX, minZ));
        return obstacle;
    }

    void Awake()
    {
        BoxCollider[] boxColliders = GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < boxColliders.Length; i++)
        {
            IList<Vector2> obstacle = ComputesBoxCollider2Vector2(boxColliders[i]);
            Simulator.Instance.addObstacle(obstacle);
        }
    }

    public static IList<Vector2> GetAgent2Obstacle(GameAgent gameAgent)
    {
        IList<Vector2> obstacleVector2List = new List<Vector2>();
        BoxCollider boxColliderComp = gameAgent.gameObject.GetComponent<BoxCollider>();
        if (boxColliderComp)
        { 
            obstacleVector2List = ComputesBoxCollider2Vector2(boxColliderComp);
        }

        return obstacleVector2List;
    }

    // Update is called once per frame
    void Update()
    {
    }
}