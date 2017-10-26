﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using PSMS.Host.WMI.Contracts;
using PSMS.Host.WMI.Common;
using PSMS.Host.WMI.ClassFactory;
using PSMS.Host.WMI.RequestProcessing;

namespace PSMS.Host.FileServerConnectionManager
{
    public partial class Service : ServiceBase
    {
        Timer _Timer;
        Int32 _Interval = 1;

        public Service()
        {
            InitializeComponent();
            try
            {
                String strInterval = ConfigurationManager.AppSettings["TimerInterval"];
                if (!Int32.TryParse(strInterval, out _Interval))
                    _Interval = 1000;
                _Timer = new Timer(ProcessNext, null, Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

        }

        protected override void OnStart(string[] args)
        {
            _Timer.Change(_Interval,_Interval);
        }

        protected override void OnStop()
        {
            _Timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        void ProcessNext(Object data)
        {
            try
            {
                IRequestProcessorFactory factory = new AFactory();
                RequestProcessor processor = new RequestProcessor(factory);
                processor.ProcessNext();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        void HandleException(Exception ex)
        {
            EventLogErrorLogger el = new EventLogErrorLogger();
            el.LogError(ex);
        }
    }
}