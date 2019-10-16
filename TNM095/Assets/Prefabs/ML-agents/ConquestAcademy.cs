using MLAgents;
using System.Collections.Generic;
using System.Linq;

public class ConquestAcademy : Academy {
    public List<GameBoard> games;

    public override void AcademyReset() {
        base.AcademyReset();
        foreach (GameBoard game in games) {
            game.Reset();
        }
    }
}
