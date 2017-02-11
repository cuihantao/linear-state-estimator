using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynchrophasorAnalytics.Hdb.Records;

namespace SynchrophasorAnalytics.Hdb
{
    public class HdbContext
    {
        private ModelFiles m_modelFiles;
        private List<Area> m_areas;
        private List<CircuitBreaker> m_circuitBreakers;
        private List<Company> m_companies;
        private List<Division> m_divisions;
        private List<LineSegment> m_lineSegments;
        private List<Node> m_nodes;
        private List<Shunt> m_shunts;
        private List<Station> m_stations;
        private List<Transformer> m_transformers;
        private List<ParentTransformer> m_parentTransformers;
        private List<TransformerTap> m_transformerTaps;
        private List<TransmissionLine> m_transmissionLines;

        public ModelFiles ModelFiles
        {
            get
            {
                return m_modelFiles;
            }
            set
            {
                m_modelFiles = value;
            }
        }

        public List<Area> Areas
        {
            get
            {
                return m_areas;
            }
            set
            {
                m_areas = value;
            }
        }

        public List<CircuitBreaker> CircuitBreakers
        {
            get
            {
                return m_circuitBreakers;
            }
            set
            {
                m_circuitBreakers = value;
            }
        }

        public List<Company> Companies
        {
            get
            {
                return m_companies;
            }
            set
            {
                m_companies = value;
            }
        }

        public List<Division> Divisions
        {
            get
            {
                return m_divisions;
            }
            set
            {
                m_divisions = value;
            }
        }

        public List<LineSegment> LineSegments
        {
            get
            {
                return m_lineSegments;
            }
            set
            {
                m_lineSegments = value;
            }
        }

        public List<Node> Nodes
        {
            get
            {
                return m_nodes;
            }
            set
            {
                m_nodes = value;
            }
        }

        public List<Shunt> Shunts
        {
            get
            {
                return m_shunts;
            }
            set
            {
                m_shunts = value;
            }
        }

        public List<Station> Stations
        {
            get
            {
                return m_stations;
            }
            set
            {
                m_stations = value;
            }
        }

        public List<Transformer> Transformers
        {
            get
            {
                return m_transformers;
            }
            set
            {
                m_transformers = value;
            }
        }

        public List<ParentTransformer> ParentTransformers
        {
            get
            {
                return m_parentTransformers;
            }
            set
            {
                m_parentTransformers = value;
            }
        }

        public List<TransformerTap> TransformerTaps
        {
            get
            {
                return m_transformerTaps;
            }
            set
            {
                m_transformerTaps = value;
            }
        }

        public List<TransmissionLine> TransmissionLines
        {
            get
            {
                return m_transmissionLines;
            }
            set
            {
                m_transmissionLines = value;
            }
        }

        public HdbContext(ModelFiles modelFiles)
        {
            m_modelFiles = modelFiles;
            SyncContext();
        }

        public void SyncContext()
        {
            m_areas = HdbReader.ReadAreaFile(m_modelFiles.AreaFile);
            m_circuitBreakers = HdbReader.ReadCircuitBreakerFile(m_modelFiles.CircuitBreakerFile);
            m_companies = HdbReader.ReadCompanyFile(m_modelFiles.CompanyFile);
            m_divisions = HdbReader.ReadDivisionFile(m_modelFiles.DivisionFile);
            m_lineSegments = HdbReader.ReadLineSegmentFile(m_modelFiles.LineSegmentFile);
            m_nodes = HdbReader.ReadNodeFile(m_modelFiles.NodeFile);
            m_shunts = HdbReader.ReadShuntFile(m_modelFiles.ShuntFile);
            m_stations = HdbReader.ReadStationFile(m_modelFiles.StationFile);
            m_transformers = HdbReader.ReadTransformerFile(m_modelFiles.TransformerFile);
            m_parentTransformers = HdbReader.ReadParentTransformerFile(m_modelFiles.ParentTransformerFile);
            m_transformerTaps = HdbReader.ReadTransformerTapFile(m_modelFiles.TransformerTapFile);
            m_transmissionLines = HdbReader.ReadTransmissionLineFile(m_modelFiles.TransmissionLineFile);
        }
    }
}
