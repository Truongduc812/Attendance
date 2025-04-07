using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UDF.TrackDeviceService.DTOs
{
    public class CreateDatabaseDTO
    {
        public int UsersIdSeq { get; set; }

        public bool IsValid(ref string errorMessage)
        {
            if (UsersIdSeq <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
