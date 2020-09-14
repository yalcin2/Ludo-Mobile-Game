using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    class PermissionValidation
    {
        [Required(AllowEmptyStrings = false)]
        public string Permission1 { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Music")]
        public int Music_fk { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select A User")]
        public Guid User_fk { get; set; }

        [MetadataType(typeof(PermissionValidation))]
        public partial class Permission
        {
        }
    }
}
