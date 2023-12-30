public enum Orientation
{
    north,
    east,
    south,
    west,
    none
}

public enum AimDirection
{
    Up,
    UpRight,
    UpLeft,
    Right,
    Left,
    Down
}

public enum GameState
{
    gameStarted,
    playingLevel,
    engagingEnemies,
    engagingBoss,
    bossStage,
    levelCompleted,
    gameWon,
    gameLost,
    gamePaused,
    dungeonOverviewMap,
    restartGame
}

