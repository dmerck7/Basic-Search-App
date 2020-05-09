using System;

namespace BasicSearchApp.Entities
{
    /// <summary>
    /// Created 5/8/2020
    /// Author - David Merck
    /// 
    /// Definition of a Enity
    /// All enities inherit its properites
    /// </summary>
    public class Entity
    {
        public long Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
