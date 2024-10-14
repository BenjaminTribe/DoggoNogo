using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Diagnostics; // Stopwatch included here
using System.Collections;
using System.Collections.Generic; //for List

public class TrialManager : MonoBehaviour
{
    public float[] ISIRange = { 1f, 4f }; // Input floats to Random.Range to return a float
    
    // ISI global vars
    double trialISI; // Stores each trial's trialISI for speed of access
    // Timer - https://stackoverflow.com/questions/394020/how-accurate-is-system-diagnostics-stopwatch
    Stopwatch timer = new Stopwatch(); // High precision timer: https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch?view=net-8.0&redirectedfrom=MSDN#remarks

    // Feedback/score
    public ScoreManager scoreManager; // Get score based on RT
    public Bone bone; // Show/hide bone depending on timer
    public Feedback feedback; // For prompting new trial states (and missed trial for now)

    // ******************* FUNCTIONS *******************
    // Coroutine to delay the start of a new trial and show feedback
    IEnumerator DelayBeforeNextTrial(float delay = 1f) {
        yield return new WaitForSeconds(delay); // Wait for 1 second
        NewTrial(); // Start new trial after delay
    }

    void NewTrial() { //function to reset variables and set-up for a new trials
        bone.Hide();
        // Get new ISI
        trialISI = UnityEngine.Random.Range(ISIRange[0], ISIRange[1]); // New trialISI
        // Create new trial in data structure
        DataManager.Instance.data.NewTrial(trialISI); // Create an instance of a Trial
        // Hide Feedback
        feedback.Hide();
        // Start timer
        RestartTimer();
    }

    void EndTrial(){ // Don't forget missed trials here
        timer.Stop(); // Immediately stop timer
        double rt = timer.Elapsed.TotalSeconds - trialISI; // subtract ISI from time elapsed during press
        scoreManager.ProcessTrialResult(rt); // Probs don't need score anywhere
        // feedback.giveFeedback("missed", scoreManager.totalScore, 0);
        // SaveTrial(null, "missed", 0, scoreManager.totalScore, medianRT, false, scoreManager.validTrialCount);
        /////// ---------------
        StartCoroutine(DelayBeforeNextTrial());
        // presentText(feedback); // Prompt for new trial starting
    }   


    void RestartTimer(){
        timer.Reset();
        timer.Start();
    }

    // Consider moving this to trial manager
    public void EndTask(){ // called from feedback
        // Load next scene
        SceneManager.LoadScene("End");
    }

    // ******************* UNITY *******************
    void Start(){
        DataManager.Instance.data.ClearTrials(); // Incase of retry
    }

    // Update is called once per frame - maybe use FixedUpdate for inputs?
    void Update(){
        if(trialISI == 0 && Input.GetKeyDown(KeyCode.DownArrow)){
            NewTrial();
        } else if(timer.IsRunning){
            // If ISI ended show bone
            if(timer.Elapsed.TotalSeconds > trialISI && bone.Hidden()){
                bone.Show(); // End ISI
            }
            // On down arrow (early or valid press)
            if(Input.GetKeyDown(KeyCode.DownArrow) || timer.Elapsed.TotalSeconds > (trialISI + scoreManager.maxRT)){
                EndTrial();
            }
        }
    }
}
