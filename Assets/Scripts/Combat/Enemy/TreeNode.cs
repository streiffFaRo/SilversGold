using System.Collections.Generic;


public class TreeNode
{
    public List<TreeNode> children = new();
    public TreeNode parent;
    public int actionIndex;
    public float score;
    public string name;

}
