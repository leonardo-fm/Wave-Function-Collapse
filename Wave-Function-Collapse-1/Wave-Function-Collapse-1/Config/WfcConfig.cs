

using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Wave_Function_Collapse_1.Config;

public class WfcConfig
{
    public WfcConfig(string configJsonPath)
    {
        var configData = JsonConvert.DeserializeObject<JsonConfigStructure>(File.ReadAllText(configJsonPath));
        if (configData is null) throw new NullReferenceException("The JSON config not founded");
        CharsSet = configData.CharsSet;
        CharsRule = configData.CharsRule;
        ShowSteps = configData.ShowSteps;
        StepsSpeed = configData.StepsSpeed;
    }
    
    public readonly char[] CharsSet;
    
    public readonly List<Wfc.Tile> CharsRule;
    
    public readonly (int, int) GridSize = (30, 100);
    
    public readonly bool ShowSteps = false;
    
    public readonly int StepsSpeed = 100;

    internal class JsonConfigStructure
    {
        [JsonPropertyName("CharsSet")]
        public char[] CharsSet;
    
        [JsonPropertyName("CharsRule")]
        public List<Wfc.Tile> CharsRule;
        
        [JsonPropertyName("ShowSteps")]
        public bool ShowSteps;
        
        [JsonPropertyName("StepsSpeed")]
        [Description("Time in ms")]
        public int StepsSpeed;
    }
}