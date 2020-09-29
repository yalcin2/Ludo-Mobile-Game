using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MusicValidation
    {
        
        [Required(AllowEmptyStrings =false, ErrorMessage ="Please Input Name")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Input Price")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Genre")]
        public int Genre_fk { get; set; }

        [ForeignKey("User_fk")]
        public Guid User_fk { get; set; }
    }

    [MetadataType(typeof(MusicValidation))]
    public partial class Music
    {
    }
}
