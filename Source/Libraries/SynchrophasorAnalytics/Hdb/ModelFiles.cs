using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Synchrophasor.Hdb
{
    [Serializable()]
    public class ModelFiles
    {
        private string m_areaFile;
        private string m_circuitBreakerFile;
        private string m_companyFile;
        private string m_divisionFile;
        private string m_lineSegmentFile;
        private string m_nodeFile;
        private string m_shuntFile;
        private string m_stationFile;
        private string m_transformerFile;
        private string m_transformerTapFile;
        private string m_transmissionLineFile;

        public string AreaFile
        {
            get
            {
                return m_areaFile;
            }
            set
            {
                m_areaFile = value;
            }
        }

        public string CircuitBreakerFile
        {
            get
            {
                return m_circuitBreakerFile;
            }
            set
            {
                m_circuitBreakerFile = value;
            }
        }

        public string CompanyFile
        {
            get
            {
                return m_companyFile;
            }
            set
            {
                m_companyFile = value;
            }
        }

        public string DivisionFile
        {
            get
            {
                return m_divisionFile;
            }
            set
            {
                m_divisionFile = value;
            }
        }

        public string LineSegmentFile
        {
            get
            {
                return m_lineSegmentFile;
            }
            set
            {
                m_lineSegmentFile = value;
            }
        }

        public string NodeFile
        {
            get
            {
                return m_nodeFile;
            }
            set
            {
                m_nodeFile = value;
            }
        }

        public string ShuntFile
        {
            get
            {
                return m_shuntFile;
            }
            set
            {
                m_shuntFile = value;
            }
        }

        public string StationFile
        {
            get
            {
                return m_stationFile;
            }
            set
            {
                m_stationFile = value;
            }
        }

        public string TransformerFile
        {
            get
            {
                return m_transformerFile;
            }
            set
            {
                m_transformerFile = value;
            }
        }

        public string TransformerTapFile
        {
            get
            {
                return m_transformerTapFile;
            }
            set
            {
                m_transformerTapFile = value;
            }
        }

        public string TransmissionLineFile
        {
            get
            {
                return m_transmissionLineFile;
            }
            set
            {
                m_transmissionLineFile = value;
            }
        }

        public ModelFiles()
        {
        }


    }
}
