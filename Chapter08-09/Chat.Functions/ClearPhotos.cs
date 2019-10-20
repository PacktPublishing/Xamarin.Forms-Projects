using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chat.Functions
{
    public static class ClearPhotos
    {
        [FunctionName("ClearPhotos")]
        public static async Task Run([TimerTrigger("0 */60 * * * *")]TimerInfo myTimer, ILogger log)
        {
            await StorageHelper.Clear();
        }
    }
}
