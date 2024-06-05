using UnityEngine;
using XNode;

[CreateAssetMenu(fileName = "New Chapter Graph", menuName = "DataGraphs/Chapter Graph")]
public class ChapterGraph : NodeGraph
{
	public BaseChapterNode NextNode(BaseChapterNode currentNode)
	{
		if (currentNode == null)
		{
			currentNode = StartChapter();
		}

		currentNode = currentNode.GetPort("Output").Connection.node as BaseChapterNode;

		return currentNode;
	}
	
	private BaseChapterNode StartChapter()
	{
		return nodes.Find(x => x is StartNode) as BaseChapterNode;
	}
}