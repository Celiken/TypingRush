using UnityEngine;
using TMPro;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance;

    [SerializeField] private TextMeshProUGUI _typingWordTMP;
    [SerializeField] private TextMeshProUGUI _scoreTMP;

    private int score = 0;
    private string _typing = "";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
        ClearWord();
        UpdateScore();
    }

    public void UpdateWord(string l)
    {
        _typing += l;
        UpdateWordCompletion();
    }

    private void UpdateWordCompletion()
    {
        bool hasRightStart = false;
        _typingWordTMP.text = _typing;
        foreach (Enemy enemy in SpawnManager.Instance._enemyList)
        {
            if (enemy.UpdateWordCompletion(_typing)) hasRightStart = true;
        }
        if (!hasRightStart && _typing != "") ClearWord();
    }

    public void ClearWord()
    {
        _typing = "";
        _typingWordTMP.text = "";
        UpdateWordCompletion();
    }

    public void ValidateWord()
    {
        score++;
        UpdateScore();
    }

    private void UpdateScore() => _scoreTMP.text = $"Score: {score}";

    public bool IsWordUsed(string word)
    {
        foreach (Enemy enemy in SpawnManager.Instance._enemyList)
            if (enemy.GetWord() == word) return true;
        return false;
    }

}
