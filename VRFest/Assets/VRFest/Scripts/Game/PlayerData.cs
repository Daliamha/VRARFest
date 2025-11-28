// PlayerData.cs
using System;
using System.Collections.Generic;

[Serializable]
public class ScoreEntry
{
    public string date;
    public int score;

    public ScoreEntry(string date, int score)
    {
        this.date = date;
        this.score = score;
    }
}

[Serializable]
public class Player
{
    public string name;
    public string gender;
    public int age;
    public List<ScoreEntry> scores;

    public Player(string name, string gender, int age, List<ScoreEntry> scores)
    {
        this.name = name;
        this.gender = gender;
        this.age = age;
        this.scores = scores;
    }
}