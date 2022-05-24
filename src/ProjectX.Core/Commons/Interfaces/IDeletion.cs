using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Core.Commons.Interfaces
{
	public interface IDeletion
	{
		bool IsDeleted { get; set; }
		DateTime? DeletionTime { get; set; }
	}
}
