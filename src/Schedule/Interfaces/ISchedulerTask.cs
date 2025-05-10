using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Interfaces;
public interface ISchedulerTask
{
	string Name { get; }
	string CronTime { get; }
	Task ExecuteJob();
	string GetQueueName()
	{
		return "default";
	}
}
