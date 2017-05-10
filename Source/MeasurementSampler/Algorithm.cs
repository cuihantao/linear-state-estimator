using System;
using ECAClientFramework;
using MeasurementSampler.Model.ECA;
using MeasurementSampler.Model.LSE;

namespace MeasurementSampler
{
    public static class Algorithm
    {
        internal class Output
        {
            public NullOutput OutputData = new NullOutput();
            public _NullOutputMeta OutputMeta = new _NullOutputMeta();
            public static Func<Output> CreateNew { get; set; } = () => new Output();
        }

        public static void UpdateSystemSettings()
        {
            SystemSettings.InputMapping = "Input";
            SystemSettings.OutputMapping = "NullOutput";
            SystemSettings.ConnectionString = @"server=localhost:6190; interface=0.0.0.0";
            SystemSettings.FramesPerSecond = 30;
            SystemSettings.LagTime = 3;
            SystemSettings.LeadTime = 1;

        }
         


        internal static Output Execute(Input inputData, _InputMeta inputMeta)
        {
            Output output = Output.CreateNew();


            return output;
        }
    }
}
