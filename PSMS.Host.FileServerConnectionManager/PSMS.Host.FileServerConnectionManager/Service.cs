using PSMS.Host.WMI.ClassFactory;
using PSMS.Host.WMI.Common;
using PSMS.Host.WMI.Contracts;
using PSMS.Host.WMI.RequestProcessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Configuration;

namespace PSMS.Host.FileServerConnectionManager
{
    public partial class Service : ServiceBase
    {
        Timer _Timer;
        Int32 _Interval = 1;
        String _DbType;

        public Service()
        {
            InitializeComponent();
            try
            {
                _DbType = ConfigurationManager.AppSettings[Constants.AppSettingKeys.DbType];

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
            if (!String.IsNullOrEmpty(_DbType) && (_DbType.Equals(Constants.AppSettingValues.SqlDbType) || _DbType.Equals(Constants.AppSettingValues.VfpDbType)))
            {
                _Timer.Change(_Interval, _Interval);
            }
            else
            {
                HandleException(new System.Configuration.ConfigurationErrorsException(Strings.InvalidConfiguration_DbTypeMustBeVfpOrSql));
                Stop();
            }
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
