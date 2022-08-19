using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Wave_Function_Collapse_1.Wfc;

namespace Wave_Function_Collapse_1.Config;

public class WfcConfig
{
    [JsonPropertyName("CharsSet")]
    public char[] CharsSet = null!;
    
    [JsonPropertyName("CharsRule")]
    public List<WfcAlgorithm.Tile> CharsRule = null!;
    
    public (int, int) GridSize = (25, 50);
    
    [JsonPropertyName("ShowSteps")]
    public bool ShowSteps = false;
    
    [JsonPropertyName("StepsSpeed")]
    [Description("Time in ms")]
    public int StepsSpeed = 100;

    public void LoadConfigFromFile(string configJsonPath)
    {
        var configData = JsonConvert.DeserializeObject<WfcConfig>(File.ReadAllText(configJsonPath));
        if (configData is null) throw new NullReferenceException("The JSON config not founded");
        CharsSet = configData.CharsSet;
        CharsRule = configData.CharsRule;
        ShowSteps = configData.ShowSteps;
        StepsSpeed = configData.StepsSpeed;
    }
}