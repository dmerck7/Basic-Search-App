namespace Scheduler.TSQL {
    class ScheduledTaskTSQL {

        public static string ResetAllProcessingTasks()
        {
            return @"
UPDATE CommunicationCenter.dbo.ScheduledTaskQueue
    SET active=1
WHERE status=2 and active=0
";
        }

        public static string GetReadyTasks() {
            return @"
SELECT stq.taskId
    ,stq.companyCode
    ,stq.taskType
    ,stq.active
    ,stq.status
FROM CommunicationCenter.dbo.ScheduledTaskQueue stq
WHERE (stq.active = 1
AND stq.scheduledTime <= GETDATE())
";
        }

        public static string UpdateActiveBit() {
            return @"
UPDATE CommunicationCenter.dbo.ScheduledTaskQueue
    SET active = @bitfield
WHERE taskId = @taskId
";
        }

        public static string UpdateTaskStatus()
        {
            return @"
UPDATE CommunicationCenter.dbo.ScheduledTaskQueue
    SET status = @status
WHERE taskId = @taskId
";
        }

        public static string UpdateScheduledTask() {
            return @"
UPDATE CommunicationCenter.dbo.ScheduledTaskQueue
	SET scheduledTime = @scheduledTime
		,lastStartTime = @lastStartTime
		,lastEndTime = @lastEndTime
WHERE taskId = @taskId AND companyCode = @companyCode
";
        }

//        public static string SetToHistory()
//        {
//            return @"
//INSERT INTO CommunicationCenter.dbo.ScheduledTaskQueueHist
//	(id, taskId, notificationCompanyId, companyCode, scheduledTime, taskType, [status], startTime, endTime)
//SELECT @id
//	,@taskId
//    ,@notificationCompanyId
//    ,@companyCode
//	,@scheduledTime
//	,@taskType
//	,@status
//	,@startTime
//	,@endTime
//";
//        }
    }
}