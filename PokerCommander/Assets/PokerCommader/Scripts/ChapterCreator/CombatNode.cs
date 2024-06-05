
public class CombatNode : BaseChapterNode
{
	public BattleData BattleData;
	
	[Input(ShowBackingValue.Never, connectionType = ConnectionType.Override)]
	public BaseChapterNode Input;
	[Output(connectionType = ConnectionType.Override)]
	public BaseChapterNode Output;
}