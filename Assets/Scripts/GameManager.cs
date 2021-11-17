using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    int enemies;
    int enemiesT;
    int enemiesV;
    int enemiesVo;
    string texto = "Enemigos restantes: ";
    [SerializeField] TextMeshProUGUI textEne;
    /*private void Awake()
    {
        int managers = GameObject.FindObjectsOfType<GameManager>().Length;
        if (managers > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(this);
    }*/
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemiesT = GameObject.FindObjectsOfType<EnemyTank>().Length;
        enemiesV = GameObject.FindObjectsOfType<EnemyVapor>().Length;
        enemiesVo = GameObject.FindObjectsOfType<Enemy>().Length;
        enemies = enemiesT + enemiesV + enemiesVo;
        textEne.text = texto+enemies.ToString();
        Debug.Log(enemies);
        if (enemies<=0)
            SceneManager.LoadScene("Victoria");
    }
}
