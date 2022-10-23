using System.Collections.Generic;

namespace Data
{
public class LevelsDataManager: GameDataManager<List<LevelData>>
{
    protected override string _fileName => "LevelsData";
}
}