using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TreeNode
{
    public List<Node> children = new();
    public Node parent;
    public int actionIndex;
    public float score;
}
