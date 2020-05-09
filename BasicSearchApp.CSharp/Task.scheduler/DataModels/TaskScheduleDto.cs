using System;

namespace Scheduler.DataModels {

    enum Status {
        New = 1,
        Processing = 2,
        Complete = 3
    };

    class ScheduledTaskQueueDto {
        public Guid TaskId { get; set; }
        public string CompanyCode { get; set; }
        public string TaskType { get; set; }
        public bool Active { get; set; }
        public Status Status { get; set; }
    }

}
