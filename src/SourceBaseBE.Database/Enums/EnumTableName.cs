namespace SourceBaseBE.Database.Enums
{
	public enum TableName
	{
		//TraceDatas,
		Environments,
		DeviceConnections,
		TraceDataISendVarCounts,
		TraceDataISendVarDateTimes,
		TraceDataISendVarSafetyPoints,
		TraceDataISendVarStatusEntitys,
		TraceDataThirdPartyEntitys,
		TraceDataProcessEntitys,
		Parameter
	}
	//public enum TraceDatas
	//{
	//  Id,
	//  MessageId,
	//  ConnectionId,
	//  ExecuteAt,
	//  CreatedAt,
	//}
	public enum TraceDataISendVarCounts
	{
		Id,
		MessageId,
		ConnectionId,
		ExecuteAt,
		CreatedAt,
	}
	public enum TraceDataISendVarDateTimes
	{
		Id,
		MessageId,
		ConnectionId,
		ExecuteAt,
		CreatedAt,
	}
	public enum TraceDataISendVarSafetyPoints
	{
		Id,
		MessageId,
		ConnectionId,
		ExecuteAt,
		CreatedAt,
	}
	public enum TraceDataISendVarStatusEntitys
	{
		Id,
		MessageId,
		ConnectionId,
		ExecuteAt,
		CreatedAt,
	}
	public enum TraceDataThirdPartyEntitys
	{
		Id,
		MessageId,
		ConnectionId,
		ExecuteAt,
		CreatedAt,
	}
	public enum TraceDataProcessEntitys
	{
		Id,
		MessageId,
		ConnectionId,
		ExecuteAt,
		CreatedAt,
	}
	public enum Environments
	{
		Id,
		ConnectionId,
		EnvKey,
		TraceColumnId,
		TraceColumnDataType,
		ExecuteAt,
		CreatedAt,
		CreatedBy,
		UpdateBy,
	}
	public enum DeviceConnections
	{
		Id,
		ConnectionKey,
		ExecuteAt,
		CreatedAt,
		CreatedBy,
		UpdateBy,
	}
	public enum business
	{
	}
}
