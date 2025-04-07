using iSoft.Common.Enums;
using iSoft.Common.ExtensionMethods;
using Prometheus;
using System.Diagnostics.Metrics;

namespace iSoft.Common
{
	public static class Messages
	{
		private static readonly Counter ExceptionCount = Metrics.CreateCounter(
															"app_exception_count",
															"Number of exception",
															labelNames: new[] {
														  EnumMetricsLabel.Controller.ToString().ToLower(),
														  EnumMetricsLabel.Method.ToString().ToLower()
															}
														 );

		// Info messages
		public static Message IFuncStart_0 { get { return new InfoMessage("I0010001", "{0} Func Start"); } }
		public static Message IFuncEnd_0 { get { return new InfoMessage("I0010002", "{0} Func End"); } }
		public static Message IBegin_0_1 { get { return new InfoMessage("I0010003", "{0} Begin, {1}"); } }
		public static Message ISuccess_0_1 { get { return new InfoMessage("I0010004", "{0} Success, {1}"); } }
		public static Message IConfigChanged { get { return new InfoMessage("I0010005", "{0} Config changed, {1}"); } }
		public static Message IInfo { get { return new InfoMessage("I0010006", "{0}"); } }

		// Warning messages
		public static Message Warning { get { return new WarningMessage("W0010001", "W0010001"); } }

		// TODO: bị lỗi multi thread khi dùng chung biến bên dưới
		// Error messages
		public static Message ErrException { get { return new ErrorExceptionMessage("E0010099", "Unknown exception occurred, ex: {0}, {1}"); } }
		public static Message ErrBaseException { get { return new ErrorExceptionMessage("E0010010", "Base exception occurred, ex: {0}, {1}"); } }
		public static Message ErrDBException { get { return new ErrorExceptionMessage("E0010011", "Database exception occurred, ex: {0}, {1}"); } }
		public static Message ErrPermission_0 { get { return new ErrorMessage("E0010012", "Permission error, role: {0}"); } }
		public static Message ErrInputInvalid_0_1 { get { return new ErrorMessage("E0010013", "{0} Input invalid, data: {1}"); } }
		public static Message ErrDuplicateItem_0_1 { get { return new ErrorMessage("E0010014", "{0} Duplicate item, data: {1}"); } }
		public static Message ErrUpdateItem_0_1 { get { return new ErrorMessage("E0010015", "Update {0} fail, data: {1}"); } }
		public static Message ErrInsertItem_0_1 { get { return new ErrorMessage("E0010016", "Insert {0} fail, data: {1}"); } }
		public static Message ErrDeleteItem_0_1 { get { return new ErrorMessage("E0010017", "Delete {0} fail, data: {1}"); } }
		public static Message ErrGetItem_0_1 { get { return new ErrorMessage("E0010018", "Get {0} fail, data: {1}"); } }
		public static Message ErrAlreadyExists_0_1 { get { return new ErrorMessage("E0010019", "{0} Already exists, data: {1}"); } }
		public static Message ErrNotFound_0_1 { get { return new ErrorMessage("E0010020", "{0} Not found, data: {1}"); } }
		public static Message ErrAbnormalData_0_1 { get { return new ErrorMessage("E0010021", "{0} Data abnormal, data: {1}"); } }
		public static Message ErrOverMax_0_1 { get { return new ErrorMessage("E0010022", "{0} Value is over the maximum limit, data: {1}"); } }
		public static Message ErrLogin { get { return new ErrorMessage("E0010023", "Username or password is incorrect"); } }
		public static Message ErrExportExcel { get { return new ErrorMessage("E0010024", "Excel export error"); } }
		public static Message ErrInternal { get { return new ErrorMessage("E0010025", "Internal Server Error"); } }
		public static Message ErrImportExcelDataFormat { get { return new ErrorMessage("E0010025", "Import File Format Incorrect."); } }
		public static Message ErrImportExcelProcessing { get { return new ErrorMessage("E0010026", "Processing Import Data Error."); } }
		public static Message ErrDifferrentChangePassword { get { return new ErrorMessage("E0010027", "New Password and Confirm Password Are Different"); } }
		public static Message ErrWrongPassword { get { return new ErrorMessage("E0010028", "Wrong Password"); } }
		public static Message ErrRolePassword { get { return new ErrorMessage("E0010029", "Wrong Password Rule, Password length must greater than 6 and lower than 40"); } }
		public static Message ErrChangePasswordFailded { get { return new ErrorMessage("E0010030", "Change Password Failded"); } }
		public class Message
		{
			internal EnumMessageType type { get; set; }
			internal string code { get; set; }
			internal string text { get; set; }
			internal List<object> parameters { get; set; } = new List<object>();
			public Message()
			{
			}
			public Message(EnumMessageType type, string code, string text)
			{
				this.type = type;
				this.code = code;
				this.text = text;
			}

			public string GetCode()
			{
				return this.code;
			}

			public EnumMessageType GetMsgType()
			{
				return this.type;
			}

			public Message SetParameters(params object[] parameters)
			{
				this.parameters.Clear();
				for (int i = 0; i < parameters.Length; i++)
				{
					object param = parameters[i];
					if (param == null)
					{
						continue;
					}
					if (typeof(Exception).IsAssignableFrom(parameters[i].GetType()))
					{
						ExceptionCount.WithLabels("none", "none").Inc();
						this.parameters.Add("\r\n[EXCEPTION !!!] " + ((Exception)parameters[i]).Message + (getStackTrace((Exception)parameters[i], 1)));
					}
					else
					{
						this.parameters.Add(parameters[i]);
					}
				}
				return this;
			}

			private string getStackTrace(Exception exception, int deep)
			{
				if (exception.InnerException == null)
				{
					return $"\r\n[StackTrace {deep}] " + $"{exception.Message}\r\n{exception.StackTrace}";
				}
				return $"\r\n[StackTrace {deep}] " + $"{exception.Message}\r\n{exception.StackTrace}" + getStackTrace(exception.InnerException, deep + 1);
			}

			public virtual string GetMessage(params object[] parameters)
			{
				if (parameters != null && parameters.Length >= 1)
				{
					return $"[{this.code}] " + string.Format(correctTemplate(this.text, parameters.Length), parameters.ToArray());
				}
				return this.GetMessage();
			}

			public virtual string GetMessage()
			{
				if (this.parameters != null && this.parameters.Count >= 1)
				{
					try
					{
						return $"[{this.code}] " + string.Format(correctTemplate(this.text, this.parameters.Count), this.parameters.ToArray());
					}
					catch (Exception ex)
					{
						return $"[{this.code}] Error string.Format, " + ex.Message + ", " + correctTemplate(this.text, this.parameters.Count) + ": " + this.parameters.ToJson();
					}
				}
				return $"[{this.code}] {this.text}";
			}

			private string correctTemplate(string messageTemplate, int paramCount)
			{
				for (int i = 0; i < paramCount; i++)
				{
					if (!messageTemplate.Contains("{" + i + "}"))
					{
						messageTemplate += ", " + "{" + i + "}";
					}
				}
				return messageTemplate;
			}

			public override string ToString()
			{
				return this.GetMessage();
			}
		}
		public class ErrorExceptionMessage : ErrorMessage
		{
			public ErrorExceptionMessage(string code, string text)
				: base(code, text)
			{
			}

			public override string GetMessage(params object[] parameters)
			{
				ExceptionCount.WithLabels("none", "none").Inc();
				if (parameters.Length == 0)
				{
					return this.GetMessage();
				}
				if (parameters.Length == 1)
				{
					parameters = new object[] { parameters[0], "" };
				}
				return base.GetMessage(parameters);
			}

			public override string GetMessage()
			{
				ExceptionCount.WithLabels("none", "none").Inc();
				if (this.parameters.Count == 1)
				{
					this.parameters.Add("");
				}
				return base.GetMessage();
			}
		}
		public class ErrorMessage : Message
		{
			public ErrorMessage(string code, string text)
				: base(EnumMessageType.Error, code, text)
			{
			}
		}
		public class WarningMessage : Message
		{
			public WarningMessage(string code, string text)
				: base(EnumMessageType.Warning, code, text)
			{
			}
		}
		public class InfoMessage : Message
		{
			public InfoMessage(string code, string text)
				: base(EnumMessageType.Info, code, text)
			{
			}
		}

	}
}
