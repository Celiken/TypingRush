using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance;

    [SerializeField] private TextMeshProUGUI _scoreTMP;

    private int score = 0;

    [SerializeField] private List<Enemy> _mainWordEnemyList = new();
    [SerializeField] private List<Enemy> _secondaryWordEnemyList = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
        UpdateScore();
    }

    public void UpdateWord(char l)
    {
        CheckWordCompletion(l);
    }

    private void CheckWordCompletion(char l)
    {
        if (_mainWordEnemyList.Count == 0)
            _mainWordEnemyList.AddRange(CheckAllWords(l));
        else
        {
            (bool progMain, List<Enemy> tmpListMain) = CheckMainWord(l);
            if (progMain)
            {
                UpdateMainWordEnemyList(tmpListMain);
                UpdateSecondaryWordEnemyList(new());
            }
            if (_secondaryWordEnemyList.Count == 0)
                _secondaryWordEnemyList.AddRange(CheckAllWords(l, true));
            else
            {
                (bool progSecond, List<Enemy> tmpListSec) = CheckSecondWord(l);
                if (progSecond)
                {
                    UpdateSecondaryWordEnemyList(tmpListSec);
                    _secondaryWordEnemyList.Clear();
                    UpdateMainWordEnemyList(tmpListSec);
                }
                else
                {
                    UpdateSecondaryWordEnemyList(new());
                    _secondaryWordEnemyList.AddRange(CheckAllWords(l, true));
                }
            }
        }
        UpdateWordVisual();
    }

    private List<Enemy> CheckAllWords(char l, bool avoidMain = false)
    {
        List<Enemy> list = new();
        foreach (Enemy enemy in SpawnManager.Instance._enemyList)
        {
            if (avoidMain && _mainWordEnemyList.Contains(enemy)) continue;
            if (enemy.CheckNextLetter(l).progress) list.Add(enemy);
        }
        return list;
    }

    private (bool progress, List<Enemy> newList) CheckMainWord(char l)
    {
        List<Enemy> list = new();

        foreach (Enemy enemy in _mainWordEnemyList)
            if (enemy.CheckNextLetter(l).progress) list.Add(enemy);

        return (list.Count != 0, list);
    }
    private (bool progress, List<Enemy> newList) CheckSecondWord(char l)
    {
        List<Enemy> list = new();

        foreach (Enemy enemy in _secondaryWordEnemyList)
            if (enemy.CheckNextLetter(l).progress) list.Add(enemy);

        return (list.Count != 0, list);
    }

    private void UpdateWordVisual()
    {
        foreach (Enemy enemy in SpawnManager.Instance._enemyList)
            enemy.UpdateWordVisual();
    }

    public void UpdateMainWordEnemyList(List<Enemy> newList)
    {
        if (_mainWordEnemyList != null)
            foreach (Enemy enemy in _mainWordEnemyList)
            {
                if (newList != null && newList.Contains(enemy)) continue;
                enemy.ResetWordCompletion();
            }
        _mainWordEnemyList.Clear();
        _mainWordEnemyList.AddRange(newList);
    }

    public void UpdateSecondaryWordEnemyList(List<Enemy> newList)
    {
        if (_secondaryWordEnemyList != null)
            foreach (Enemy enemy in _secondaryWordEnemyList)
            {
                if (newList != null && newList.Contains(enemy)) continue;
                enemy.ResetWordCompletion();
            }
        _secondaryWordEnemyList.Clear();
        _secondaryWordEnemyList.AddRange(newList);
    }

    public void ValidateWord()
    {
        UpdateMainWordEnemyList(new());
        UpdateSecondaryWordEnemyList(new());
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
