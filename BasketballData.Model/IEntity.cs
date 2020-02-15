using System;
using System.Collections.Generic;
using System.Text;

namespace BasketballData.Model
{
	public interface IEntity
	{
		DateTime DateLastModifiedUtc { get; set; }
		DateTime DateCreatedUtc { get; set; }
	}
}
