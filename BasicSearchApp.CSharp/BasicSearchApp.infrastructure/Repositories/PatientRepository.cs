using BasicSearchApp.data.Context;
using BasicSearchApp.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BasicSearchApp.Repositories
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Basic repo functions abstracting the data source
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        public PatientRepository()
        {
        }

        public Patient GetByID(long id)
        {
            return DbContext.Patients.Where(x => x.Id == id).SingleOrDefault();
        }

        public IEnumerable<Patient> GetAll()
        {
            return DbContext.Patients;
        }

        public void Create(Patient patient) {
            DbContext.Patients.Add(patient);
        }

        public void Update(Patient patient)
        {
            int index = DbContext.Patients.ToList().FindIndex(m => m.Id == patient.Id);
            if (index >= 0)
                DbContext.Patients.ToList()[index] = patient;
        }

        public void Delete(Patient patient)
        {
            DbContext.Patients.Remove(patient);
        }


    }
}
