using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public PuzzleBlock currentBlock;
    private Puzzle puzzle;
    public List<PuzzleBlock> gridBlocks = new();
    public static BlockManager instance = null;

    //need to make this a list
    private Quaternion targetRotation = Quaternion.Euler(270, 0, 0);
    public float delayAfterPuzzleEnd = 2.5f;

    private Dictionary<int, int[]> blockDisplayMapping = new Dictionary<int, int[]>()
    {
        { 0, new int[] {0, 1, 2} },
        { 1, new int[] {0, 1} },
        { 2, new int[] {0} }
    };

    private void Awake()
    {
        //singleton
        if (instance == null)
        {
            instance = this;
        }
        puzzle = GetComponentInParent<Puzzle>();
        DisplayNextBlock();
    }

    //set this as the current block
    public void SetCurrentBlock(PuzzleBlock block)
    {
        currentBlock = block;
    }

    //make the blocks clickable
    public void ActivateBlocks()
    {
        gridBlocks.ForEach(block => block.interactable = true);
    }

    //make the blocks unclickable
    public void DeactivateBlocks()
    {
        gridBlocks.ForEach(block => block.interactable = false);
    }

    //display the next block if we collected a block
    public void DisplayNextBlock()
    {
        //respective key with respective objects that need to be displayed
        if (blockDisplayMapping.ContainsKey(Player.instance.missingBlocks))
        {
            ItemUIManager.Instance.ToggleItem(6 - Player.instance.missingBlocks);
            //block array that has the map with the key
            int[] blockIndexes = blockDisplayMapping[Player.instance.missingBlocks];
            foreach (int index in blockIndexes)
            {
                //set active
                gridBlocks[index].gameObject.SetActive(true);
            }
        }
    }

    //check if the block has the right position/rotation
    private bool CheckWinCondition(BlockFace blockFace)
    {
        foreach (PuzzleBlock block in gridBlocks)
        {
            if (block.gameObject.activeSelf)
            {
                if (block.CurrentFace != blockFace || !CheckRotation(block))
                    return false;
            }
        }
        return true;
    }

    //check wincon and finish up the puzzle
    public void CallCheck()
    {
        if (CheckWinCondition(BlockFace.Top))
        {
            //disable block interaction
            DeactivateBlocks();
            //recall memory
            Player.instance.RecallMemory(puzzle.associatedMemory);
            //move the last block in place
            currentBlock.transform.DOMove(currentBlock.defaultBlockPos.position, 1);
            //call the function after the delay
            Invoke(nameof(OnPuzzleFinishedMove), delayAfterPuzzleEnd);
        }
    }

    //checks if we have the right rotation of the image
    public bool CheckRotation(PuzzleBlock puzzleBlock)
    {
        float difference = Quaternion.Angle(puzzleBlock.transform.localRotation, targetRotation);
        return Mathf.Abs(difference) < 0.1f;
    }

    //move the puzzle away
    private void OnPuzzleFinishedMove()
    {
        gameObject.transform.DOMove(UIManager.instance.puzzleUI.blockPuzzleInstantiatePos.position, UIManager.instance.puzzleUI.blockPuzzleMoveDur)
            .SetEase(UIManager.instance.puzzleUI.blockPuzzleCurve)
            .OnComplete(() => Destroy(gameObject));
    }

    //rotate the block with the given index in the list
    public void RotateBlockAt(int index, RotationDirection direction)
    {
        if (index >= 0 && index < gridBlocks.Count)
        {
            gridBlocks[index].RotateBlock(direction);
        }
    }
}
