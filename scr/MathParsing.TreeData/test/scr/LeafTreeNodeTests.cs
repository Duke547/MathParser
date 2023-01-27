﻿using MathParsing.TreeData;
using NUnit.Framework;

namespace MathParsing.Testing.TreeData;

public class LeafTreeNodeTests
{
    [Test]
    public void AddChild_Test()
    {
        var parent = new LeafTreeNode<string>("parent");
        var child  = new TreeNode    <string>("child1");

        Assert.That(() => parent.AddChild(child), Throws.InvalidOperationException);
    }
}