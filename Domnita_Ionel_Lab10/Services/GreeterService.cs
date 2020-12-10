using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Domnita_Ionel_Lab10
{
    public class GreeterService : Greeter.GreeterBase
    {
        public override async Task SendStatusBD(IAsyncStreamReader<SRequest> requestStream, IServerStreamWriter<SResponse> responseStream, ServerCallContext context)
        {
            List<StatusInfo> statusList = StatusRepo();
            SResponse sRes;
            await foreach (var message in requestStream.ReadAllAsync())
            {
                sRes = new SResponse();
                sRes.StatusInfo.Add(statusList.Skip(message.No - 1).Take(1));
                await responseStream.WriteAsync(sRes);
            }
        }
        public override async Task<SResponse> SendStatusCS(IAsyncStreamReader<SRequest> requestStream, ServerCallContext context)
        {
            List<StatusInfo> statusList = StatusRepo();
            SResponse sRes = new SResponse();
            await foreach (var message in requestStream.ReadAllAsync())
            {
                sRes.StatusInfo.Add(statusList.Skip(message.No - 1).Take(1));
            }
            return sRes;
        }
        public override async Task SendStatusSS(SRequest request, IServerStreamWriter<SResponse> responseStream, ServerCallContext context)
        {
            List<StatusInfo> statusList = StatusRepo();
            SResponse sRes;
            var i = 0;
            while (!context.CancellationToken.IsCancellationRequested)
            {
                sRes = new SResponse();
                sRes.StatusInfo.Add(statusList.Skip(i).Take(request.No));
                await responseStream.WriteAsync(sRes);
                i++;
                await Task.Delay(1000);
            }
        }
        public List<StatusInfo> StatusRepo()
        {
            List<StatusInfo> statusList = new List<StatusInfo> {
new StatusInfo { Author = "Randy", Description = "Task 1 in progess"},
new StatusInfo { Author = "John", Description = "Task 1 just started"},
new StatusInfo { Author = "Miriam", Description = "Finished all tasks"},
new StatusInfo { Author = "Petra", Description = "Task 2 finished"},
new StatusInfo { Author = "Steve", Description = "Task 2 in progress"}
};
            return statusList;
        }
    }
}
