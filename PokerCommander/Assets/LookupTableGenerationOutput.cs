
public class LookupTableGenerationOutput
{
    public string Output;
    public uint Step;
    public uint Max;
    public int TotalPhases;

    public LookupTableGenerationOutput(int totalPhases)
    {
        TotalPhases = totalPhases;
    }
}
