[System.Serializable]
public class Enemy
{
    public string enemyName;
    public int maxHp;
    public int hp;
    public bool isSpecialEnemy;

    public Enemy(string name, int hp, bool special = false)
    {
        this.enemyName = name;
        this.maxHp = hp;
        this.hp = hp;
        this.isSpecialEnemy = special;
    }

    // (Option) Reset HP ได้
    public void Reset()
    {
        this.hp = this.maxHp;
    }
}
