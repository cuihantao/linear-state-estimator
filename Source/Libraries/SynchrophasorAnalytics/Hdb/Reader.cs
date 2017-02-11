﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynchrophasorAnalytics.Hdb.Records;

namespace SynchrophasorAnalytics.Hdb
{
    public class HdbReader
    {
        private const string m_recordDeclarative = "#record";

        private const string m_companyNumberHeader = "%SUBSCRIPT";
        private const string m_companyNameHeader = "id_co";

        private const string m_divisionNumberHeader = "%SUBSCRIPT";
        private const string m_divisionNameHeader = "id_dv";
        private const string m_divisionAreaNumberHeader = "i$area_dv";

        private const string m_stationNumberHeader = "%SUBSCRIPT";
        private const string m_stationNameHeader = "id_st";

        private const string m_areaNumberHeader = "%SUBSCRIPT";
        private const string m_areaNameHeader = "id_area";

        private const string m_nodeNumberHeader = "%SUBSCRIPT";
        private const string m_nodeIdHeader = "id_nd";
        private const string m_nodeBaseKvIdHeader = "id_kv";
        private const string m_nodeBaseKvHeader = "vl_kv";
        private const string m_nodeCompanyHeader = "id_co";
        private const string m_nodeDivisionHeader = "id_dv";
        private const string m_nodeStationHeader = "id_st";
        private const string m_nodeBusNumberHeader = "i$bs_nd";

        private const string m_transmissionLineNumberHeader = "%SUBSCRIPT";
        private const string m_transmissionLineIdHeader = "id_line";

        private const string m_shuntNumberHeader = "%SUBSCRIPT";
        private const string m_shuntIdHeader = "id_cp";
        private const string m_shuntNodeIdHeader = "nd_cp";
        private const string m_shuntStationNameHeader = "id_st";
        private const string m_shuntNominalMvarHeader = "mrnom_cp";
        private const string m_shuntIsOpenHeader = "open_cp";

        private const string m_lineSegmentNumberHeader = "%SUBSCRIPT";
        private const string m_lineSegmentIdHeader = "id_ln";
        private const string m_lineSegmentTransmissionLineIdHeader = "id_line";
        private const string m_lineSegmentDivisionIdHeader = "id_dv";
        private const string m_lineSegmentFromNodeIdHeader = "nd_ln";
        private const string m_lineSegmentFromStationNameHeader = "st_ln";
        private const string m_lineSegmentToNodeIdHeader = "znd_ln";
        private const string m_lineSegmentToStationNameHeader = "zst_ln";
        private const string m_lineSegmentResistanceHeader = "r_ln";
        private const string m_lineSegmentReactanceHeader = "x_ln";
        private const string m_lineSegmentLineChargingHeader = "bch_ln";
        private const string m_lineSegmentIsRemovedHeader = "remove_ln";

        private const string m_circuitBreakerNumberHeader = "%SUBSCRIPT";
        private const string m_circuitBreakerIdHeader = "id_cb";
        private const string m_circuitBreakerTypeHeader = "id_cbtyp";
        private const string m_circuitBreakerStationNameHeader = "id_st";
        private const string m_circuitBreakerFromNodeIdHeader = "nd_cb";
        private const string m_circuitBreakerToNodeIdHeader = "znd_cb";
        private const string m_circuitBreakerNormallyOpenHeader = "nmlopen_cb";
        private const string m_circuitBreakerIsOpenHeader = "open_cb";

        private const string m_tapNumberHeader = "%SUBSCRIPT";
        private const string m_tapIdHeader = "id_tapty";
        private const string m_tapMaximumPositionHeader = "mx_tapty";
        private const string m_tapMinimumPositionHeader = "mn_tapty";
        private const string m_tapNominalPositionHeader = "nom_tapty";
        private const string m_tapStepSizeHeader = "step_tapty";

        private const string m_transformerNumberHeader = "%SUBSCRIPT";
        private const string m_transformerIdHeader = "id_xf";
        private const string m_transformerParentHeader = "id_xfmr";
        private const string m_transformerStationNameHeader = "id_st";
        private const string m_transformerFromNodeIdHeader = "nd_xf";
        private const string m_transformerToNodeIdHeader = "znd_xf";
        private const string m_transformerRegulatedNodeIdHeader = "regnd_xf";
        private const string m_transformerFromNodeNominalKvHeader = "kvnom";
        private const string m_transformerFromNodeTapHeader = "tapty_xf";
        private const string m_transformerFromNodeTapPositionHeader = "tap_xf";
        private const string m_transformerToNodeNominalKvHeader = "zkvnom";
        private const string m_transformerToNodeTapHeader = "ztapty_xf";
        private const string m_transformerToNodeTapPositionHeader = "ztap_xf";
        private const string m_transformerIsRemovedHeader = "remove_xf";
        private const string m_transformerResistanceHeader = "r_xf";
        private const string m_transformerReactanceHeader = "x_xf";
        private const string m_transformerMagnetizingConductanceHeader = "gmag_xf";
        private const string m_transformerMagnetizingSusceptanceHeader = "bmag_xf";


        private const string m_parentTransformerNumberHeader = "%SUBSCRIPT";
        private const string m_parentTransformerIdHeader = "id_xfmr";

        public static List<ParentTransformer> ReadParentTransformerFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<ParentTransformer> parentTransformers = new List<ParentTransformer>();

            foreach (Dictionary<string, string> record in records)
            {
                parentTransformers.Add(new ParentTransformer()
                {
                    Number = Convert.ToInt32(record[m_parentTransformerNumberHeader]),
                    Id = record[m_parentTransformerIdHeader],
                });
            }
            return parentTransformers;

        }

        public static List<Transformer> ReadTransformerFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<Transformer> transformers = new List<Transformer>();

            foreach (Dictionary<string, string> record in records)
            {
                transformers.Add(new Transformer()
                {
                    Number = Convert.ToInt32(record[m_transformerNumberHeader]),
                    Id = record[m_transformerIdHeader],
                    Parent = record[m_transformerParentHeader],
                    StationName = record[m_transformerStationNameHeader],
                    FromNodeId = record[m_transformerFromNodeIdHeader],
                    ToNodeId = record[m_transformerToNodeIdHeader],
                    RegulatedNodeId = record[m_transformerRegulatedNodeIdHeader],
                    FromNodeNominalKv = Convert.ToDouble(record[m_transformerFromNodeNominalKvHeader]),
                    FromNodeTap = record[m_transformerFromNodeTapHeader],
                    FromNodeTapPosition = Convert.ToInt32(record[m_transformerFromNodeTapPositionHeader]),
                    ToNodeNominalKv = Convert.ToDouble(record[m_transformerToNodeNominalKvHeader]),
                    ToNodeTap = record[m_transformerToNodeTapHeader],
                    ToNodeTapPosition = Convert.ToInt32(record[m_transformerToNodeTapPositionHeader]),
                    IsRemoved = record[m_transformerIsRemovedHeader],
                    Resistance = Convert.ToDouble(record[m_transformerResistanceHeader]),
                    Reactance = Convert.ToDouble(record[m_transformerReactanceHeader]),
                    MagnetizingConductance = Convert.ToDouble(record[m_transformerMagnetizingConductanceHeader]),
                    MagnetizingSusceptance = Convert.ToDouble(record[m_transformerMagnetizingSusceptanceHeader]),
                });
            }
            return transformers;
        }

        public static List<TransformerTap> ReadTransformerTapFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<TransformerTap> transformerTaps = new List<TransformerTap>();

            foreach (Dictionary<string, string> record in records)
            {
                transformerTaps.Add(new TransformerTap()
                {
                    Number = Convert.ToInt32(record[m_tapNumberHeader]),
                    Id = record[m_tapIdHeader],
                    MaximumPosition = Convert.ToInt32(record[m_tapMaximumPositionHeader]),
                    MinimumPosition = Convert.ToInt32(record[m_tapMinimumPositionHeader]),
                    NominalPosition = Convert.ToInt32(record[m_tapNominalPositionHeader]),
                    StepSize = Convert.ToDouble(record[m_tapStepSizeHeader])
                });
            }
            return transformerTaps;
        }

        public static List<CircuitBreaker> ReadCircuitBreakerFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<CircuitBreaker> circuitBreakers = new List<CircuitBreaker>();

            foreach (Dictionary<string, string> record in records)
            {
                circuitBreakers.Add(new CircuitBreaker()
                {
                    Number = Convert.ToInt32(record[m_circuitBreakerNumberHeader]),
                    Id = record[m_circuitBreakerIdHeader],
                    Type = record[m_circuitBreakerTypeHeader],
                    StationName = record[m_circuitBreakerStationNameHeader],
                    FromNodeId = record[m_circuitBreakerFromNodeIdHeader],
                    ToNodeId = record[m_circuitBreakerToNodeIdHeader],
                    IsNormallyOpen = record[m_circuitBreakerNormallyOpenHeader],
                    IsOpen = record[m_circuitBreakerIsOpenHeader]
                });
            }
            return circuitBreakers;
        }

        public static List<LineSegment> ReadLineSegmentFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<LineSegment> lineSegments = new List<LineSegment>();

            foreach (Dictionary<string, string> record in records)
            {
                lineSegments.Add(new LineSegment()
                {
                    Number = Convert.ToInt32(record[m_lineSegmentNumberHeader]),
                    Id = record[m_lineSegmentIdHeader],
                    TransmissionLineId = record[m_lineSegmentTransmissionLineIdHeader],
                    DivisionName = record[m_lineSegmentDivisionIdHeader],
                    FromNodeId = record[m_lineSegmentFromNodeIdHeader],
                    FromStationName = record[m_lineSegmentFromStationNameHeader],
                    ToNodeId = record[m_lineSegmentToNodeIdHeader],
                    ToStationName = record[m_lineSegmentToStationNameHeader],
                    Resistance = Convert.ToDouble(record[m_lineSegmentResistanceHeader]),
                    Reactance = Convert.ToDouble(record[m_lineSegmentReactanceHeader]),
                    LineCharging = Convert.ToDouble(record[m_lineSegmentLineChargingHeader]),
                    IsRemoved = record[m_lineSegmentIsRemovedHeader]
                });
            }
            return lineSegments;
        }

        public static List<Shunt> ReadShuntFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<Shunt> shunts = new List<Shunt>();

            foreach (Dictionary<string, string> record in records)
            {
                shunts.Add(new Shunt()
                {
                    Number = Convert.ToInt32(record[m_shuntNumberHeader]),
                    Id = record[m_shuntIdHeader],
                    NodeId = record[m_shuntNodeIdHeader],
                    StationName = record[m_shuntStationNameHeader],
                    NominalMvar = Convert.ToDouble(record[m_shuntNominalMvarHeader]),
                    IsOpen = record[m_shuntIsOpenHeader]
                });
            }
            return shunts;
        }

        public static List<TransmissionLine> ReadTransmissionLineFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<TransmissionLine> transmissionLines = new List<TransmissionLine>();

            foreach (Dictionary<string, string> record in records)
            {
                transmissionLines.Add(new TransmissionLine()
                {
                    Number = Convert.ToInt32(record[m_transmissionLineNumberHeader]),
                    Id = record[m_transmissionLineIdHeader],
                });
            }
            return transmissionLines;
        }

        public static List<Node> ReadNodeFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<Node> nodes = new List<Node>();

            foreach (Dictionary<string, string> record in records)
            {
                nodes.Add(new Node()
                {
                    Number = Convert.ToInt32(record[m_nodeNumberHeader]),
                    Id = record[m_nodeIdHeader],
                    BaseKv = Convert.ToDouble(record[m_nodeBaseKvHeader]),
                    BaseKvId = Convert.ToDouble(record[m_nodeBaseKvIdHeader]),
                    CompanyName = record[m_nodeCompanyHeader],
                    DivisionName = record[m_nodeDivisionHeader],
                    StationName = record[m_nodeStationHeader],
                    BusNumber = Convert.ToInt32(record[m_nodeBusNumberHeader])
                });
            }
            return nodes;
        }

        public static List<Area> ReadAreaFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<Area> areas = new List<Area>();

            foreach (Dictionary<string, string> record in records)
            {
                areas.Add(new Area()
                {
                    Number = Convert.ToInt32(record[m_areaNumberHeader]),
                    Name = record[m_areaNameHeader]
                });
            }
            return areas;
        }

        public static List<Station> ReadStationFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<Station> stations = new List<Station>();

            foreach (Dictionary<string, string> record in records)
            {
                stations.Add(new Station()
                {
                    Number = Convert.ToInt32(record[m_stationNumberHeader]),
                    Name = record[m_stationNameHeader]
                });
            }
            return stations;
        }

        public static List<Division> ReadDivisionFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<Division> divisions = new List<Division>();

            foreach (Dictionary<string, string> record in records)
            {
                divisions.Add(new Division()
                {
                    Number = Convert.ToInt32(record[m_divisionNumberHeader]),
                    Name = record[m_divisionNameHeader],
                    AreaNumber = Convert.ToInt32(record[m_divisionAreaNumberHeader])
                });
            }
            return divisions;
        }

        public static List<Company> ReadCompanyFile(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = ReadAndParameterizeAttributes(hdbExportFile);

            List<Company> companies = new List<Company>();

            foreach (Dictionary<string, string> record in records)
            {
                companies.Add(new Company()
                {
                    Number = Convert.ToInt32(record[m_companyNumberHeader]),
                    Name = record[m_companyNameHeader]
                });
            }
            return companies;
        }


        public static List<Dictionary<string, string>> ReadAndParameterizeAttributes(string hdbExportFile)
        {
            List<Dictionary<string, string>> records = new List<Dictionary<string, string>>();
            List<string> headers = new List<string>();
            string line;
            StreamReader patternOutputFile = new StreamReader(hdbExportFile);
            while ((line = patternOutputFile.ReadLine()) != null)
            {
                if (line.Contains(m_recordDeclarative))
                {
                    List<string> headerKeywords = line.Split(',').ToList();
                    foreach (string keyword in headerKeywords)
                    {
                        headers.Add(keyword.Trim());
                    }
                }
                else
                {
                    Dictionary<string, string> record = new Dictionary<string, string>();
                    string[] attributes = line.Split(',');
                    foreach (string header in headers)
                    {
                        if (headers.IndexOf(header) != 0)
                        {
                            record.Add(header, attributes[headers.IndexOf(header)].Trim().TrimEnd('\'').TrimStart('\''));
                        }
                    }
                    records.Add(record);
                }

            }

            patternOutputFile.Close();

            return records;
        }
    }
}
