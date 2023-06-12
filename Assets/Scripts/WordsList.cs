using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordsList : SerializedMonoBehaviour
{
    public static WordsList Instance;

    [SerializeField] private TextAsset _wordsListFile;

    [SerializeField]
    private Dictionary<int, List<string>> _wordsArrays = new();
    private int _minLength = int.MaxValue, _maxLength = 0;

    void Awake()
    {
        Instance = this;
        string[] _wordsArray = _wordsListFile.text.Split("\r\n");
        int length;
        foreach (string word in _wordsArray)
        {
            length = word.Length;
            if (_minLength > length) _minLength = length;
            if (_maxLength < length) _maxLength = length;
            if (!_wordsArrays.ContainsKey(length))
                _wordsArrays.Add(length, new List<string>());
            _wordsArrays[length].Add(word.ToUpper());
        }
    }

    public string GetRandomWord(int length)
    {
        var listRef = _wordsArrays[length];
        return listRef[Random.Range(0, listRef.Count)];
    }

    public string GetRandomWord()
    {
        var keys = _wordsArrays.Keys.ToList();
        return GetRandomWord(keys[Random.Range(0, keys.Count)]);
    }
}