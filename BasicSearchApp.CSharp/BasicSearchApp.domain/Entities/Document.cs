using System;
using System.Collections.Generic;

namespace BasicSearchApp.Entities
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Definition of a Document Header
    /// Content is loaded upon request based on filename
    /// PatientId links ocument to patient
    /// Inherits from Enity
    /// </summary>
    public class Document : Entity
    {
        public long PatientId { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Filename { get; set; }
    }
}
