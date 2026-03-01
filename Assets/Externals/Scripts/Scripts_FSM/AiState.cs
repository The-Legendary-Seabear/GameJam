using UnityEngine;

public abstract class AiState
{
    protected StateAgent agent;

    public AiState(StateAgent agent)
    {
        this.agent = agent;
    }

    public string Name => GetType().Name;
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnUpdate();
}
