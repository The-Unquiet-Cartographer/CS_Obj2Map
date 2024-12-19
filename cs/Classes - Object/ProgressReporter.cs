public class ProgressReporter {

/// <summary>
/// E.g. each stage can be a file, and the stepCount can be the number of lines to be processed within that file.
/// </summary>
    private List<int> _stagesByStepCount = new List<int>();
    private int _currentStage = -1;
    private int _currentStep_overall = 0;



    private int completedStages {get{return _currentStage;}}
    private int totalSteps {get{
            int total = 0;
            for (int i = 0; i < _stagesByStepCount.Count; i++) {
                total += _stagesByStepCount[i];
            }
            return total;
    }}
    private int currentStep_thisStage {get{
            int linesProcessed = _currentStep_overall;
            for (int i = 0; i < _currentStage; i++) linesProcessed -=  _stagesByStepCount[i];
            return linesProcessed;
    }}
    private float currentStagePctProgress {get{
            if (_currentStage == _stagesByStepCount.Count) return 0;
            return (float)currentStep_thisStage/_stagesByStepCount[_currentStage] * 100;
    }}
    public float totalPctProgress {get{return (completedStages*100f + currentStagePctProgress) / _stagesByStepCount.Count;}}



    public void AddStage (int noOfSteps) {
        _stagesByStepCount.Add(noOfSteps);
    }
    public void IncrementProgress () {
        if (_currentStage == -1) {
            _currentStage++;
        }
        if (currentStep_thisStage < _stagesByStepCount[_currentStage]) {
            _currentStep_overall++;
        }
        if (currentStep_thisStage == _stagesByStepCount[_currentStage]) {
            _currentStage++;
        }
    }
    public void Reset () {
        _stagesByStepCount = new List<int>();
        _currentStage = -1;
        _currentStep_overall = 0;
        _progressBarProgress = 0;
    }



    public void WriteProgressGuide () {
        Console.WriteLine("|.........|.........|.........|.........|.........|.........|.........|.........|.........|.........|");
    }
    private int _progressBarProgress = 0;
    public void WriteProgressPerStep () {
        if (currentStep_thisStage == 1) {
            Console.Write('|');
            return;
        }
        int currentPctProgress_int = (int)currentStagePctProgress;
        if (currentPctProgress_int > _progressBarProgress) {
            if (currentPctProgress_int%10 > 0) WriteColored(ConsoleColor.White, "+");
            else WriteColored(ConsoleColor.Yellow, "0");
            _progressBarProgress++;
        }
        else if (currentPctProgress_int == 0 && _progressBarProgress == 99){        //<== currentStagePctProgress automatically resets to zero when it reaches 100, so do this instead.
            _progressBarProgress = 0;
            WriteColored(ConsoleColor.Cyan, "0\r\n");
        }
    }
    private static void WriteColored (ConsoleColor c, string s) {
        Console.ForegroundColor = c;
        Console.Write(s);
        Console.ResetColor();
    }
/*
    public void WriteTotalProgress () {
        string s = "|";
        float _totalPctProgress = totalPctProgress;
        for (int i = 0; i < 100; i+=10) {
            for (int j = 1; j < 10; j++) {
                if (i+j <= _totalPctProgress) s += '+';
                else s += '.';
            }
            if (i+10 <= _totalPctProgress) s += '0';
            else s += '|';
        }
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        WriteProgressGuide();
        Console.WriteLine(s);
        Console.ForegroundColor = ConsoleColor.White;
    }
*/



    private string LogEverything () {
        string s = "";
        s += "Stage "+(_currentStage+1)+"/"+_stagesByStepCount.Count;
        s += "; Step "+currentStep_thisStage+"/"+(_currentStage == _stagesByStepCount.Count ? "?" : _stagesByStepCount[_currentStage]);
        s += "; Pct"+currentStagePctProgress+" -> "+totalPctProgress;
        return s;
    }


}