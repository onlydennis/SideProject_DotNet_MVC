using System.Collections;
using System.Collections.Generic;

namespace Shared.Core.Data
{
	public class ApplicationMessage
	{
		public bool IsOk { get; set; }

		public object Data { get; set; }

		public IEnumerable Errors { get; set; }

		public string Message { get; set; }

		public string ReportName { get; set; }

		public string ReportId { get; set; }

		public string RightsToSearch { get; set; }

		public string Parameter { get; set; }

        public object Data2 { get; set; }
    }
}
