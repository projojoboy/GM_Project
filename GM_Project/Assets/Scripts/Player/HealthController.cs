using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _currentHealth = 3;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject _heart;

    private GameObject[] _hp;

    private void Awake()
    {
        if (GameObject.FindObjectOfType<Canvas>())
            _canvas = GameObject.FindObjectOfType<Canvas>();
        else
            _canvas = Instantiate(_canvas);

        _healthBar = Instantiate(_healthBar, _canvas.transform);

        _hp = new GameObject[3];

        for (int i = 0; i < _maxHealth; i++)
        {
            GameObject heart = _hp[i] = Instantiate(_heart, _healthBar.transform);
            heart.transform.position = new Vector2(_healthBar.transform.position.x + (60 * i), _healthBar.transform.position.y);
        }
    }

    private void Update()
    {
        if (_currentHealth <= 0)
            Dead();
    }

    private void Dead()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void LoseHP(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_currentHealth > 0)
            {
                _currentHealth--;
                _hp[_currentHealth].SetActive(false);
            }
        }
    }

    public void GainHP(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (_currentHealth < _maxHealth)
            {
                _hp[_currentHealth].SetActive(true);
                _currentHealth++;
            }
        }
    }
}
