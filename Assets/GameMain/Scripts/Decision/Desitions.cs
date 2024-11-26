public interface IDecision
{
    bool Evaluate(); // 判断是否满足触发条件
    void Execute();  // 执行对应操作
    void ClearStat();
}

public abstract class DecisionBase : IDecision
{
    protected Player player;

    public DecisionBase(Player player)
    {
        this.player = player;
    }

    public abstract bool Evaluate();
    public abstract void Execute();
    public abstract void ClearStat();
}