using System;
using System.Collections.Generic; // Dictionaries
using UnityEngine;

[System.Serializable]
public class Trial {
    public int trialNumber;
    public double isi;
    public double rt;
    public string datetime;
    public int trialScore;
    public int totalScore;
    public string responseType;
    public double threshold;
    public bool validTrial;
    public int validTrialCount;
    // Stim spec
    [System.NonSerialized]
    public float screenWidth;     // Current display width
    [System.NonSerialized]
    public float screenHeight;    // Current display height
    public float canvasWidth;
    public float canvasHeight;
    public float canvasScaleFactor;
    public float stimulusX;
    public float stimulusY;
    public float stimulusOrientation;
    public float stimulusScale;

    public Trial(int trialN, double isi) {
        trialNumber = trialN;
        this.isi = isi;
        this.screenWidth = Screen.width; // Directly accessing the screen width
        this.screenHeight = Screen.height;
    }

    public void SaveTrial(double rt, string type, int score, int total, double threshold, bool validTrial, int validTrialCount) {
        this.rt = rt;
        this.datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.responseType = type;
        this.trialScore = score;
        this.totalScore = total;
        this.threshold = threshold;
        this.validTrial = validTrial;
        this.validTrialCount = validTrialCount;
        Debug.Log(JsonUtility.ToJson(this));
    }

    public void SaveStimulus(Dictionary<string, float> stimSpec){
        this.canvasWidth = stimSpec["canvasWidth"];
        this.canvasHeight = stimSpec["canvasHeight"];
        this.canvasScaleFactor = stimSpec["canvasScale"];
        this.stimulusX = stimSpec["x"];
        this.stimulusY = stimSpec["y"];
        this.stimulusOrientation = stimSpec["rotation"];
        this.stimulusScale = stimSpec["scale"];
    }
}
