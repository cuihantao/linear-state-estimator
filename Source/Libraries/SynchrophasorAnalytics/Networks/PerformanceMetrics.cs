using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace SynchrophasorAnalytics.Networks
{
    [Serializable()]
    public class PerformanceMetrics
    {
        #region [ Private Members ]

        private Network m_network;
        private string m_activeVoltageCountKey;
        private string m_activeCurrentFlowCountKey;
        private string m_activeCurrentInjectionCountKey;
        private string m_observedBusCountKey;

        private long m_refreshExecutionTime = 0;
        private long m_parsingExecutionTime = 0;
        private long m_measurementMappingExecutionTime = 0;
        private long m_observabilityAnalysisExecutionTime = 0;
        private long m_activeCurrentPhasorDetermintationExecutionTime = 0;
        private long m_stateComputationExecutionTime = 0;
        private long m_solutionRetrievalExecutionTime = 0;
        private long m_outputPreparationExecutionTime = 0;
        private long m_totalExecutionTimeInTicks = 0;
        private long m_totalExecutionTimeInMilliseconds = 0;
        private string m_refreshExecutionTimeKey;
        private string m_parsingExecutionTimeKey;
        private string m_measurementMappingExecutionTimeKey;
        private string m_observabilityAnalysisExecutionTimeKey;
        private string m_activeCurrentPhasorDetermintationExecutionTimeKey;
        private string m_stateComputationExecutionTimeKey;
        private string m_solutionRetrievalExecutionTimeKey;
        private string m_outputPreparationExecutionTimeKey;
        private string m_totalExecutionTimeInTicksKey;
        private string m_totalExecutionTimeInMillisecondsKey;

        #endregion

        #region [ Public Properties ]

        [XmlIgnore()]
        public int ActiveVoltageCount
        {
            get
            {
                return m_network.Model.ActiveVoltages.Count;
            }
        }

        [XmlElement("ActiveVoltageCountKey")]
        public string ActiveVoltageCountKey
        {
            get
            {
                return m_activeVoltageCountKey;
            }
            set
            {
                m_activeVoltageCountKey = value;
            }
        }

        [XmlIgnore()]
        public int ActiveCurrentFlowCount
        {
            get
            {
                return m_network.Model.ActiveCurrentFlows.Count;
            }
        }

        [XmlElement("ActiveCurrentFlowCountKey")]
        public string ActiveCurrentFlowCountKey
        {
            get
            {
                return m_activeCurrentFlowCountKey;
            }
            set
            {
                m_activeCurrentFlowCountKey = value;
            }
        }

        [XmlIgnore()]
        public int ActiveCurrentInjectionCount
        {
            get
            {
                return m_network.Model.ActiveCurrentInjections.Count;
            }
        }

        [XmlElement("ActiveCurrentInjectionCountKey")]
        public string ActiveCurrentInjectionCountKey
        {
            get
            {
                return m_activeCurrentInjectionCountKey;
            }
            set
            {
                m_activeCurrentInjectionCountKey = value;
            }
        }

        [XmlIgnore()]
        public int ObservedBusCount
        {
            get
            {
                return m_network.Model.ObservedBusses.Count;
            }
        }

        [XmlElement("ObservedBusCountKey")]
        public string ObservedBusCountKey
        {
            get
            {
                return m_observedBusCountKey;
            }
            set
            {
                m_observedBusCountKey = value;
            }
        }

        [XmlIgnore()]
        public long RefreshExecutionTime
        {
            get
            {
                return m_refreshExecutionTime;
            }
            set
            {
                m_refreshExecutionTime = value;
            }
        }

        [XmlIgnore()]
        public long ParsingExecutionTime
        {
            get
            {
                return m_parsingExecutionTime;
            }
            set
            {
                m_parsingExecutionTime = value;
            }
        }

        [XmlIgnore()]
        public long MeasurementMappingExecutionTime
        {
            get
            {
                return m_measurementMappingExecutionTime;
            }
            set
            {
                m_measurementMappingExecutionTime = value;
            }
        }

        [XmlIgnore()]
        public long ObservabilityAnalysisExecutionTime
        {
            get
            {
                return m_observabilityAnalysisExecutionTime;
            }
            set
            {
                m_observabilityAnalysisExecutionTime = value;
            }
        }

        [XmlIgnore()]
        public long ActiveCurrentPhasorDeterminationExecutionTime
        {
            get
            {
                return m_activeCurrentPhasorDetermintationExecutionTime;
            }
            set
            {
                m_activeCurrentPhasorDetermintationExecutionTime = value;
            }
        }

        [XmlIgnore()]
        public long StateComputationExecutionTime
        {
            get
            {
                return m_stateComputationExecutionTime;
            }
            set
            {
                m_stateComputationExecutionTime = value;
            }
        }

        [XmlIgnore()]
        public long SolutionRetrievalExecutionTime
        {
            get
            {
                return m_solutionRetrievalExecutionTime;
            }
            set
            {
                m_solutionRetrievalExecutionTime = value;
            }
        }

        [XmlIgnore()]
        public long OutputPreparationExecutionTime
        {
            get
            {
                return m_outputPreparationExecutionTime;
            }
            set
            {
                m_outputPreparationExecutionTime = value;
            }
        }

        [XmlIgnore()]
        public long TotalExecutionTimeInTicks
        {
            get
            {
                return m_totalExecutionTimeInTicks;
            }
            set
            {
                m_totalExecutionTimeInTicks = value;
            }
        }

        [XmlIgnore()]
        public long TotalExecutionTimeInMilliseconds
        {
            get
            {
                return m_totalExecutionTimeInMilliseconds;
            }
            set
            {
                m_totalExecutionTimeInMilliseconds = value;
            }
        }

        [XmlElement("RefreshExecutionTimeKey")]
        public string RefreshExecutionTimeKey
        {
            get
            {
                return m_refreshExecutionTimeKey;
            }
            set
            {
                m_refreshExecutionTimeKey = value;
            }
        }

        [XmlElement("ParsingExecutionTimeKey")]
        public string ParsingExecutionTimeKey
        {
            get
            {
                return m_parsingExecutionTimeKey;
            }
            set
            {
                m_parsingExecutionTimeKey = value;
            }
        }

        [XmlElement("MeasurementMappingExecutionTimeKey")]
        public string MeasurementMappingExecutionTimeKey
        {
            get
            {
                return m_measurementMappingExecutionTimeKey;
            }
            set
            {
                m_measurementMappingExecutionTimeKey = value;
            }
        }

        [XmlElement("ObservabilityAnalysisExecutionTimeKey")]
        public string ObservabilityAnalysisExecutionTimeKey
        {
            get
            {
                return m_observabilityAnalysisExecutionTimeKey;
            }
            set
            {
                m_observabilityAnalysisExecutionTimeKey = value;
            }
        }

        [XmlElement("ActiveCurrentPhasorDetermintationExecutionTimeKey")]
        public string ActiveCurrentPhasorDeterminationExecutionTimeKey
        {
            get
            {
                return m_activeCurrentPhasorDetermintationExecutionTimeKey;
            }
            set
            {
                m_activeCurrentPhasorDetermintationExecutionTimeKey = value;
            }
        }

        [XmlElement("StateComputationExecutionTimeKey")]
        public string StateComputationExecutionTimeKey
        {
            get
            {
                return m_stateComputationExecutionTimeKey;
            }
            set
            {
                m_stateComputationExecutionTimeKey = value;
            }
        }

        [XmlElement("SolutionRetrievalExecutionTimeKey")]
        public string SolutionRetrievalExecutionTimeKey
        {
            get
            {
                return m_solutionRetrievalExecutionTimeKey;
            }
            set
            {
                m_solutionRetrievalExecutionTimeKey = value;
            }
        }

        [XmlElement("OutputPreparationExecutionTimeKey")]
        public string OutputPreparationExecutionTimeKey
        {
            get
            {
                return m_outputPreparationExecutionTimeKey;
            }
            set
            {
                m_outputPreparationExecutionTimeKey = value;
            }
        }

        [XmlElement("TotalExecutionTimeInTicksKey")]
        public string TotalExecutionTimeInTicksKey
        {
            get
            {
                return m_totalExecutionTimeInTicksKey;
            }
            set
            {
                m_totalExecutionTimeInTicksKey = value;
            }
        }

        [XmlElement("TotalExecutionTimeInMillisecondsKey")]
        public string TotalExecutionTimeInMillisecondsKey
        {
            get
            {
                return m_totalExecutionTimeInMillisecondsKey;
            }
            set
            {
                m_totalExecutionTimeInMillisecondsKey = value;
            }
        }

        #endregion

        #region [ Constructors ]
        public PerformanceMetrics()
            :this(new Network())
        {
        }

        public PerformanceMetrics(Network network)
        {
            m_network = network;
            m_activeVoltageCountKey = "Undefined";
            m_activeCurrentFlowCountKey = "Undefined";
            m_activeCurrentInjectionCountKey = "Undefined";
            m_observedBusCountKey = "Undefined";
            m_refreshExecutionTimeKey = "Undefined";
            m_parsingExecutionTimeKey = "Undefined";
            m_measurementMappingExecutionTimeKey = "Undefined";
            m_observabilityAnalysisExecutionTimeKey = "Undefined";
            m_activeCurrentPhasorDetermintationExecutionTimeKey = "Undefined";
            m_stateComputationExecutionTimeKey = "Undefined";
            m_solutionRetrievalExecutionTimeKey = "Undefined";
            m_outputPreparationExecutionTimeKey = "Undefined";
            m_totalExecutionTimeInTicksKey = "Undefined";
            m_totalExecutionTimeInMillisecondsKey = "Undefined";
    }

        #endregion
    }
}
