using System;

namespace ServiceResult.AspectOriented.ServiceProxy
{
    /// <summary>
    /// The data we want to store so that we can create a ServiceResult.
    /// </summary>
    public interface IResultPatternProxy
    {
        public bool IsNull { get; set; }
        public ResultKinds Kind { get; set; }
        public string WarningDescription { get; set; }
        public ExceptionDescriptions ExceptionDescriptions { get; set; }
    }
}
