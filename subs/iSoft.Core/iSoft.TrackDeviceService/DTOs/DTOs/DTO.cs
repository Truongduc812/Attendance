namespace UDF.TrackDeviceService.Models.DTOs
{
	public record BaseDTO(bool isEnable,bool isDelete);
	public record ConfigDTO(ulong? id, string name,bool isEnable, bool isDelete, string type = "string") : BaseDTO(isEnable, isDelete);
	public record ConnectionDTO(ulong? id, string name, ConnectionParamDTO[] paras, ConnectionConfigDTO[] configs, bool isConnected, bool isEnable, bool isDelete) : BaseDTO(isEnable, isDelete);

	public record ParamDTO(ulong? id, string name, bool isEnable, bool isDelete, string type = "string") : BaseDTO(isEnable, isDelete);
	public record ConnectionParamDTO(ulong? id, string value, ulong? connId, ulong? paramId, bool isEnable, bool isDelete) : BaseDTO(isEnable, isDelete);
	public record ConnectionConfigDTO(ulong? id, string value, ulong? connId, ulong? configId, bool isEnable, bool isDelete) : BaseDTO(isEnable, isDelete);
}
