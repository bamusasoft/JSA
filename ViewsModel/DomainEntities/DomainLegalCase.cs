using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jsa.DomainModel;

namespace Jsa.ViewsModel.DomainEntities
{
    public class DomainLegalCase
    {
        public DomainLegalCase(int caseNo, string regDate, DateTime gregDate, string defendant, string description, int statusId,CaseStatus status, IList<CaseAppointment> appointments, IList<CaseFollowing> followings )
        {
            CaseNo = caseNo;
            RegisteredAt = regDate;
            GregDate = gregDate;
            Defendant = defendant;
            Description = description;
            StatusId = statusId;
            CaseStatus = status;
            CaseAppointments = new ObservableCollection<CaseAppointment>(appointments);
            CaseFollowings = new ObservableCollection<CaseFollowing>(followings);
        }

        public int CaseNo { get; set; }
        public string RegisteredAt { get; set; }

        public DateTime GregDate { get; set; }
        public string Defendant { get; set; }
        public string Description { get; set; }

        public int StatusId { get; set; }

        public virtual ObservableCollection<CaseAppointment> CaseAppointments { get; set; }

        public virtual ObservableCollection<CaseFollowing> CaseFollowings { get; set; }

        public virtual CaseStatus CaseStatus { get; set; }

        public string NextAppointmentDate
        {
            get
            {
                if (CaseAppointments == null) return null;
                return CaseAppointments.OrderBy(x => x.AppointmentDate).First().AppointmentDate;
            }
        }
    }
}
