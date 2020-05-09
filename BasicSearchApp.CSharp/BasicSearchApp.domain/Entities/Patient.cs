using System.Collections.Generic;

namespace BasicSearchApp.Entities
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Definition of a Patient
    /// Inherits from Enity
    /// </summary>
    public class Patient : Entity
    {
        public string Name { get; set; }

        public IList<Document> Documents { get; set; }
    }
}
