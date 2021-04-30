using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int[] borders;
    public Vector3[] enemyTrajectories;
    // Start is called before the first frame update
    void Awake()
    {
        int i = 0;
        foreach (var b in borders)
        {
            var enemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            var enemyController = enemy.GetComponent<EnemyController>();
            enemyController.SetVectorsArray(SubArray(enemyTrajectories, i, b));
            enemyController.Prepare();
            i += b;
        }
    }
    private Vector3[] SubArray(Vector3[] data, int index, int length)
    {
        Vector3[] result = new Vector3[length];
        System.Array.Copy(data, index, result, 0, length);
        return result;
    }
}
