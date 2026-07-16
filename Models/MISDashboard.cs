using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
	public class MISDashboardEngineStatus
	{
		[Key]
		public Int64 MISDashboardEngineStatusId { get; set; }
        public DateTime LastUpdatedOn { get; set; }
	}
}
