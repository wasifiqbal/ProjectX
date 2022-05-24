using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Core.Commons.Abstraction
{
	public abstract class BaseEntity<T> where T : struct
	{
		[Key]
		public T Id { get; set; }
	}
}
