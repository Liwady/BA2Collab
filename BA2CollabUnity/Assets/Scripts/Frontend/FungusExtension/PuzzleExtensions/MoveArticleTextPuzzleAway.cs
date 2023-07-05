using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CommandInfo("puzzle", "moveArticleTextPuzzleAway", "moves puzzle out of screen")]
public class MoveArticleTextPuzzleAway : Command
{
    public TheatreArticleItem theatreItem;
    public override void OnEnter()
    {
        theatreItem.MoveItemAway();
        Continue();
    }
}
