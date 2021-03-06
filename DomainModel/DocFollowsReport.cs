﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jsa.DomainModel
{
    public class DocFollowsReport
    {
        public string DocId { get; set; }
        public string Subject { get; set; }
        public string RefId { get; set; }
        public string DocDate { get; set; }
        public string DocPath { get; set; }
        public DocRecordStatus DocStatus { get; set; }
        public int SecurityLevel { get; set; }
        public string Destination { get; set; }

        public string DocFollowId { get; set; }
        public string FollowDate { get; set; }
        public string FollowContent { get; set; }
        public string FollowPath { get; set; }

        public string StatusArabic
        {
            get
            {
                switch (DocStatus)
                {
                    case DocRecordStatus.Open:
                        return "تحت الإجراء";
                    case DocRecordStatus.Closed:
                        return "منتهية";
                    case DocRecordStatus.Hold:
                        return "متوقفة";
                    default:
                        throw new NotSupportedException("Status is not supprted");
                }
            }
        }
    }
}
