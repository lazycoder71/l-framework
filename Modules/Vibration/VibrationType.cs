namespace LFramework.Vibration
{
    [System.Serializable]   
    public enum VibrationType 
    {
        Default,

        ImpactLight,
        ImpactMedium,
        ImpactHeavy,
        
        Success,
        Failure,
        Warning,

        // iOS
        Rigid,
        Soft,

        // Android predefined patterns
        ClickSingle,
        ClickDouble,
        ClickHeavy,

        Tick,
    }
}