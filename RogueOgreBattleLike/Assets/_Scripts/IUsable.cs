public interface IUsable {
    bool TargetsEnemy { get; set; }
    bool TargetsAll { get; set; }
    void Use(BattleEntity target);
}
