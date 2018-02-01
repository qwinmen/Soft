using System;
using System.Text;
using Common.Logging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Monolit.Facade.Common
{
	public class WcfErrorHandler : IErrorHandler
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(WcfErrorHandler));


		public static ILog Log => log;

		private readonly bool _includeInnerException;

		public WcfErrorHandler()
		{
			_includeInnerException = true;
		}

		public bool IncludeInnerException => _includeInnerException;

		#region Implementation of IErrorHandler

		void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
		{
			if (error == null)
				throw new ArgumentNullException("error");

			string errorId = GenerateErrorCode();
			error.HelpLink = errorId;

			var errorMessage = new StringBuilder();
			errorMessage.AppendFormat("Application server - {0} - error with code: {1}",
				Environment.MachineName, errorId);
			if (IncludeInnerException)
				errorMessage.Append(Environment.NewLine).Append(ExceptionToString(error));

			var fe = new FaultException(errorMessage.ToString(), new FaultCode(errorId));
			MessageFault mf = fe.CreateMessageFault();
			fault = Message.CreateMessage(version, mf, String.Empty);
		}

		bool IErrorHandler.HandleError(Exception error)
		{
			if (error == null)
				return false;

			string errorId = string.IsNullOrEmpty(error.HelpLink) ? GenerateErrorCode() : error.HelpLink;

			LogError(error, errorId);
			return true;
		}

		#endregion

		private static string GenerateErrorCode()
		{
			return Guid.NewGuid().ToString().ToUpper();
		}

		private static string ExceptionToString(Exception ex)
		{
			return ex is FaultException<ExceptionDetail>
				? String.Format("{0}{2}Server Stack trace:{2}{1}", ex, ex.StackTrace, Environment.NewLine)
				: ex.ToString();
		}

		private static void LogError(Exception error, string errorId)
		{
			//Logger.Error(() => string.Format("Error number: {0}{1}{2}",
			Log.Error(string.Format("Error number: {0}",
				errorId /*,
				Environment.NewLine,
				ExceptionToString(error)*/), error);
		}
	}

}
