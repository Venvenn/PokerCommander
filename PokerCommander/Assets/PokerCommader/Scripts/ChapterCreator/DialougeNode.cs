using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using Yarn.Unity;

public class DialogueNode : BaseChapterNode
{
	public YarnProject YarnProject;

	[Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
	public BaseChapterNode Input;
	[Output(connectionType = ConnectionType.Override)]
	public BaseChapterNode Output;
}