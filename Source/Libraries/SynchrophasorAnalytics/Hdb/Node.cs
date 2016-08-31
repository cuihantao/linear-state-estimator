﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchrophasor.Hdb
{
    public class Node
    {
        private int m_number;
        private string m_id;
        private double m_baseKv;
        private string m_companyName;
        private string m_divisionName;
        private string m_stationName;
        private int m_busNumber;

        public int Number
        {
            get
            {
                return m_number;
            }
            set
            {
                m_number = value;
            }
        }

        public string Id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }

        public double BaseKv
        {
            get
            {
                return m_baseKv;
            }
            set
            {
                m_baseKv = value;
            }
        }

        public string CompanyName
        {
            get
            {
                return m_companyName;
            }
            set
            {
                m_companyName = value;
            }
        }

        public string DivisionName
        {
            get
            {
                return m_divisionName;
            }
            set
            {
                m_divisionName = value;
            }
        }

        public string StationName
        {
            get
            {
                return m_stationName;
            }
            set
            {
                m_stationName = value;
            }
        }

        public int BusNumber
        {
            get
            {
                return m_busNumber;
            }
            set
            {
                m_busNumber = value;
            }
        }

        public Node()
        {
        }

        public override string ToString()
        {
            return "Node:\n      Number:" + Convert.ToString(Number) + "\n          Id:" + Id + "\n     Company:" + CompanyName + "\n    Division:" + DivisionName + "\n     Station:" + StationName + "\n     Base KV:" + Convert.ToString(BaseKv) + "\n         Bus:" + Convert.ToString(BusNumber);
        }
    }
}