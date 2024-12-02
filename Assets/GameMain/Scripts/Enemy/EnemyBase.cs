using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class EnemyBase : Character
{
    #region Values

    public enum EnemyType
    {
        InactiveEndorphin,
        Glycine,
        Endorphin,
        Acetylcholine,
        EmptyCoffeeCup,
        CloudCoffee,
        SparklingSugarCube,
        SugarCubeBox,
        FlowingColorCoffeeMachine,
        BigOrange,
        AngryBigOrange,
        Dopamine,
        Serotonin,
        Melatonin
    }
    protected Player player;
    private bool findPlayer;
    private int enemyBox = 5;
    public bool canPatrol;
    //这是表示是否会有仇恨
    protected bool canHate;
    //这个表示当前是否进入仇恨状态
    protected bool isHating;
    private int enemyScore;

    public bool needClear;
    public bool mainTurn;

    public EnemyType enemyType;
    //游走路线
    private List<Vector2> pathDir = new List<Vector2>();
    private int currentPath = 0;

    //敌人被忽略的回合数
    public int isIgnore = 0;
    //敌人死亡
    public bool isDead = false;

    protected GameObject hateGameObject;
    #endregion
    protected override void Start()
    {
        base.Start();
        SetBaseStat(3, 0, 0);

        //确定是敌人
        characterType = CharacterType.Enemy;
        //获取当前瓦片
        currentGrid = GridManager.Instance.GetGridByPos(transform.position);
        GridManager.Instance.ChangeGridInfo(transform.position, characterType);

        player = PlayerManager.Instance.player;
        hateGameObject = transform.GetChild(1).gameObject;
        
        InitEnemy();
    }
    protected void SetBaseStat(int targetHealth, int targetStrength, int targetScore)
    {
        health = targetHealth;
        strength = targetStrength;
        enemyScore = targetScore;
    }

    public void SetupBorn(Vector2 pos, List<Vector2> pathDir, bool canPatrol, bool canHating, bool needClear)
    {
        transform.position = pos;
        this.pathDir = pathDir;
        this.canPatrol = canPatrol;
        this.canHate = canHating;
        currentPatrol = pos;
        this.needClear = needClear;
        UpdateGridInfo();
    }
    
    protected abstract void InitEnemy();
    #region ForAble

    private void OnEnable()
    {
        Player.UpdatePlayerPos += CheckPlayer;
    }

    private void OnDisable()
    {
        Player.UpdatePlayerPos -= CheckPlayer;
    }

    #endregion

    #region EnemyState
    
    public override void HandleMethod()
    {
        if (isDead)
            return;
        //敌人本回合决策失效
        if (isIgnore > 0)
        {
            isIgnore--;
            return;
        }
        CheckPlayer();
        // 如果识别到玩家、是带仇恨值的并且 是当前回合的
        // Debug.Log(findPlayer+" "+isHating);
        if (findPlayer && isHating)
        {
            //判定是否可以攻击
            if (CanAttack())
            {
                Attack();
            }
            else
            {
                //带仇恨锁敌的寻路
                HatingPatrol();
            }
            
        }
        else
        {
            //可以游行
            if(canPatrol)
                Patrol();
        }

        //回合结束，状态清除
        ClearStat();
        //回合结束的时候，判断是否下一个回合是否会攻击玩家
        if (CanAttack())
        {
            //让玩家可以闪避，并且更新当前闪避对象为自己
            Debug.Log("Dodge咯");
            player.canDodge = true;
            player.toDodge = this;
        }
    }
    
    protected virtual void CheckPlayer()
    {
        //失效不决策
        if (isIgnore > 0 || isDead)
            return;
        Vector2 playerPos = player.transform.position;
        Vector2 enemyPos = transform.position;
        //TODO：先写死，后面可以给怪物升级
        if(Math.Abs(playerPos.x-enemyPos.x)+Math.Abs(playerPos.y-enemyPos.y) < 20)
        {
            findPlayer = true;
        }
        else
        {
            findPlayer = false;
        }
        
        CheckHate();
    }

    private Vector2 currentPatrol;
    protected virtual void Patrol()
    {
        Vector2 theDir = pathDir[currentPath];
        Vector2 targetPos = new Vector2(currentPatrol.x + theDir.x, currentPatrol.y + theDir.y);
        //如果目标路径不为空，则不动
        if (GridManager.Instance.GetGridByPos(targetPos)!=null && GridManager.Instance.GetGridByPos(targetPos).characterType != CharacterType.None)
            return;
        transform.DOMove(targetPos, 0.5f).OnComplete(UpdateGridInfo);
        //从出生地算起的位置
        currentPatrol += theDir;
        currentPath++;
        if (currentPath == pathDir.Count)
            currentPath = 0;
    }

    protected abstract void CheckHate();
    //默认都是false
    protected abstract bool CanAttack();

    protected virtual void Attack()
    {
        DoDamage(strength, player);
        Vector2 dir = player.currentGrid.position - currentGrid.position;
        if (GridManager.Instance.GetGridByPos(player.currentGrid.position+dir) == null)
        {
            dir = Random.Range(0, 2) == 0 ? Vector2.Perpendicular(dir) : -Vector2.Perpendicular(dir);
        }

        if (GridManager.Instance.GetGridByPos(player.currentGrid.position + dir) == null)
        {
            DoDamage(strength, player);
            return;
        }
        transform.DOMove(player.transform.position, .5f).OnComplete(UpdateGridInfo);
        player.BeMove(player.currentGrid.position + dir);
    }
    
    protected List<GridInfo> starHatingPath;
    protected virtual void HatingPatrol()
    {
        starHatingPath = GridManager.Instance.FindPath(currentGrid.position, player.transform.position);
        GridManager.Instance.SetEnemyRoute(starHatingPath);
        if (starHatingPath is { Count: > 0 } && starHatingPath[0].characterType == CharacterType.None)
        {
            UpdateGridInfoNow(starHatingPath[0].position);
            transform.DOMove(starHatingPath[0].position, 0.5f).OnComplete(()=>
            {
                //移动结束之后，判断是否会攻击
                if (CanAttack())
                {
                    //让玩家可以闪避，并且更新当前闪避对象为自己
                    // Debug.Log("Dodge咯");
                    player.canDodge = true;
                    player.toDodge = this;
                }
            });
        }
    }

    protected override void Die()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        EventManager.CallSendScore(enemyScore);
        isDead = true;
        if (player.toDodge == this)
        {
            player.toDodge = null;
            player.canDodge = false;
            GridManager.Instance.ClearHatingRoute();
        }
        base.Die();
    }
    #endregion
    
    private void ClearStat()
    {
        
    }
    //
    // protected virtual bool WillAttack()
    // {
    //     return false;
    // }
}
