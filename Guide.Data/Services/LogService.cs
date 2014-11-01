//using System;
//using System.Globalization;
//using Guide.Data.Contracts;
//using Guide.Model.Entities;
//using Guide.Model.Model;
//using Guide.Services.Contracts;

//namespace Guide.Data.Services
//{
//    public class LogService: ILogService
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public LogService(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }

//        public void Log(Exception exception, string message = "")
//        {
//            _unitOfWork.LogMessages.Insert(new LogMessage()
//            {
//                Type = LogMessageTypes.Error.ToString(),
//                Message = exception.Message,
//                Source = string.IsNullOrWhiteSpace(message)? message: exception.Source,
//                StackTrace = exception.StackTrace,
//                Date = DateTime.Now
//            });
//            _unitOfWork.Save();
//        }

//        public void Log(LogMessageModel model)
//        {
//            if (model.Type.ToString(CultureInfo.InvariantCulture).Contains(","))
//            {
//                return;
//            }
//            var type = (LogMessageTypes)Enum.Parse(typeof(LogMessageTypes), model.Type);
//            if (!Enum.IsDefined(typeof(LogMessageTypes), type))
//            {
//                return;
//            }

//            _unitOfWork.LogMessages.Insert(new LogMessage()
//            {
//                Type = type.ToString(),
//                Message = model.Message,
//                Date = DateTime.Now
//            });
//            _unitOfWork.Save();
//        }

//        public void Log(LogMessageTypes type, string message = "")
//        {
//            _unitOfWork.LogMessages.Insert(new LogMessage()
//            {
//                Type = type.ToString(),
//                Message = message,
//                Date = DateTime.Now
//            });
//            _unitOfWork.Save();
//        }

//        public void LogError(string message)
//        {
//            _unitOfWork.LogMessages.Insert(new LogMessage()
//            {
//                Type = LogMessageTypes.Error.ToString(),
//                Message = message,
//                Date = DateTime.Now
//            });
//            _unitOfWork.Save();
//        }

//        public void LogWarning(string message)
//        {
//            _unitOfWork.LogMessages.Insert(new LogMessage()
//            {
//                Type = LogMessageTypes.Warning.ToString(),
//                Message = message,
//                Date = DateTime.Now
//            });
//            _unitOfWork.Save();
//        }

//        public void LogEvent(string message)
//        {
//            _unitOfWork.LogMessages.Insert(new LogMessage()
//            {
//                Type = LogMessageTypes.Event.ToString(),
//                Message = message,
//                Date = DateTime.Now
//            });
//            _unitOfWork.Save();
//        }
//    }
//}
