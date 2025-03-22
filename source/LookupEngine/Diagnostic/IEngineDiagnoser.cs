namespace LookupEngine.Diagnostic;

/// <summary>
///     Engine diagnostic provider
/// </summary>
internal interface IEngineDiagnoser
{
    /// <summary>
    ///     Start composer monitoring
    /// </summary>
    void StartMonitoring();

    /// <summary>
    ///     Stop composer monitoring    
    /// </summary>
    void StopMonitoring();
}