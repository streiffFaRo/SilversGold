using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TreeNode
{
    public List<TreeNode> children = new();
    public TreeNode parent;
    public int actionIndex;
    public float score;
    public string name;

}
