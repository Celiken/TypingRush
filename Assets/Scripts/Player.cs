using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HealthComponent _health;

    private KeyCode[] _listKeycode =
        {
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
            KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N,
            KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U,
            KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
        };

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            WordManager.Instance.UpdateMainWordEnemyList(new());
            WordManager.Instance.UpdateSecondaryWordEnemyList(new());
        }
        foreach (var key in _listKeycode)
            if (Input.GetKeyDown(key))
                WordManager.Instance.UpdateWord(key.ToString()[0]);
    }

    public void Hit()
    {
        if (_health.GetHit())
            Debug.Log("Player died");

    }
}
