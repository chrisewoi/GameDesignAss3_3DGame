using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ScoreTracking : MonoBehaviour
{
    public float[] highscores;
    public string[] fileNames = new string[] { "highscores.txt", "highscores2.txt" };
    string scoreFilePath = Directory.GetCurrentDirectory() + "/Assets/Saves/highscores.txt";

    public ScoreTracking() {

    }
    public ScoreTracking(string[] fileNameList) {
        fileNames = fileNameList;
    }

    public ScoreTracking(string[] fileNameList, string filePath) {
        fileNames = fileNameList;
        scoreFilePath = filePath;
    }


    // Start is called before the first frame update
    void Start()
    {
        LoadScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadScores() {
        //Read current highscores from text file  
        highscores = ReadFileToArray(scoreFilePath);
    }

    private void LoadScores(string fileName) {
        //Read current highscores from text file
        string filePath = Directory.GetCurrentDirectory() + "/Assets/Saves/" + fileName;
        highscores = ReadFileToArray(filePath);
    }

    private void LoadScores(string fileName, string directoryName) {
        //Read current highscores from text file
        string filePath = directoryName + fileName;
        highscores = ReadFileToArray(filePath);
    }

    private void loadScoresMulti() {
        List<float> multiFileScores = new List<float>();
       
        foreach(string curFileName in fileNames) {
            //Get the path of the current file
            string highscoresText = Directory.GetCurrentDirectory() + "/Assets/Saves/" + curFileName;
            //Read the score values and convert to a list of floats
            List<float> fileScores = ReadFileToArray(highscoresText).ToList();
            //Add this to the total list of scores
            multiFileScores.AddRange(fileScores);
        }
        //Set the total list of scores to highscores
        highscores = multiFileScores.ToArray();
    }

    private float[] ReadFileToArray(string path) {
        //Read the file
        string scoresText = File.ReadAllText(path);

        // Split up the string from the file, convert the string results to floats, and return the results as an array
        return Array.ConvertAll(scoresText.Split(','), float.Parse);
    }

    public void SaveScores(float[] newScores) {
        string saveString = "";
        for (int i = 0; i < newScores.Length; i++) {
            if(i != 0) {
                saveString += ",";
            }
            saveString += newScores[i];
        }
        File.WriteAllText(scoreFilePath, saveString);
    }

    public float[] GetNewScoreList(float newScore) {
        List<float> newScoreList = highscores.ToList();

        //Append new score to list
        newScoreList.Add(newScore);

        //Sort the array high to low 
        newScoreList.Sort(new Comparison<float>(
                            (i1, i2) => i2.CompareTo(i1)
                            ));

        //Remove lowest score if more than 5 are saved
        if(newScoreList.Count > 5) {
            newScoreList.RemoveAt(newScoreList.Count - 1);
        }

        return newScoreList.ToArray();
    }
}
