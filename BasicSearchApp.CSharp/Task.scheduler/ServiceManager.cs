using System.ServiceProcess;

namespace Scheduler {
    public class ServiceManager {
        // Start the Scheduler as a Service
        public static void Main(string[] args) {
            ServiceBase.Run(new Scheduler());
        }
    }
}
