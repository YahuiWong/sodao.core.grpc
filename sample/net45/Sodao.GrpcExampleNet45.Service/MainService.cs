﻿using Autofac;
using Sodao.Core.Grpc;
using Sodao.GrpcExample.Service.Grpc;
using Sodao.GrpcExampleNet45.Service.Tracer;
using System;

namespace Sodao.GrpcExampleNet45.Service
{
    public class MainService
    {
        public MainService()
        {
            System.Threading.ThreadPool.SetMinThreads(30, 30);
            AppDomain.CurrentDomain.UnhandledException += (obj, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                if (ex != null)
                {
                    //LogHelper.LogError(ex.Message, ex);
                }
            };
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (obj, e) =>
            {
                e.SetObserved();
                e.Exception.Flatten().Handle(c =>
                {
                    //LogHelper.LogError(c.Message, c);
                    return true;
                });
            };
        }
        public void Start(string serviceName)
        {
            // LogHelper.LogInfo($"{serviceName}服务启动");
            // autofac
            var container = AutofacContainer.Register();
            // grpc
            GrpcServiceManager.Start(GrpcExampleService.BindService(new GrpcExampleServiceImpl(container)), new ConsoleTracer(), whenException: (ex) =>
            {

            });
        }

        public void Stop(string serviceName)
        {
            GrpcServiceManager.Stop();
            // LogHelper.LogInfo($"{serviceName}服务停止");
        }
        public void ShutDown(string serviceName)
        {
            GrpcServiceManager.Stop();
            // LogHelper.LogInfo($"{serviceName}服务停止");
        }
    }
}
