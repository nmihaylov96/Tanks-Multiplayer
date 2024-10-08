using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerScript : MonoBehaviour
{

    public int enemies = 5;
    public Text enemiesText;

    private void Awake()
    {
        enemiesText.text = enemies.ToString();

        Enemy.OnEnemyKilled += OnEnemyKilledAction;
    }

    private void OnEnemyKilledAction()
    {
        enemies--;
        enemiesText.text = enemies.ToString();
    }
}
