using Microsoft.Extensions.Logging;
using Niramaya.Database;
using Niramaya.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Niramaya.Business
{
    public class PatientBusiness : PatientViewModel
    {
        private readonly ILogger<PatientBusiness> _logger;

        PatientDAL patientDAL= new PatientDAL();
        public PatientBusiness()
        {
           

        }

        public bool AddPatientService(PatientViewModel patientViewModel, ArrayList inputParameterBusiness)
        {
            return patientDAL.AddPatientData(patientViewModel, inputParameterBusiness);
        }

        public DataSet SearchPatientService(PatientViewModel patientViewModel, ArrayList inputParameterBusiness, int pageindex = 1)
        {
            return patientDAL.SearchPatientData(patientViewModel, inputParameterBusiness, pageindex);
        }

        public int GetPatientDataCountService(PatientViewModel patientViewModel, ArrayList inputParameterBusiness)
        {
            return patientDAL.GetPatientDataCount(patientViewModel, inputParameterBusiness);
        }
    }
}
