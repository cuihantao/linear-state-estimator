﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchrophasor.Hdb
{
    public class Company
    {
        private int m_number;
        private string m_name;

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

        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }

        public Company()
        {
        }

        public override string ToString()
        {
            return "Company:\n  Number:" + Convert.ToString(Number) + "\n    Name:" + Name;
        }
    }
}