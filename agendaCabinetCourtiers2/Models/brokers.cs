//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace agendaCabinetCourtiers2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class brokers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public brokers()
        {
            this.appointments = new HashSet<appointments>();
        }
    
        public int idBroker { get; set; }

        [Required]
        [Display(Name = "Nom")]
        [StringLength(50, ErrorMessage = "Le champs \"lastname\" ne doit pas exceder 50 caractères.")]
        public string lastname { get; set; }

        [Required]
        [Display(Name = "Prénom")]
        [StringLength(50, ErrorMessage = "Le champs \"firstname\" ne doit pas exceder 50 caractères.")]
        public string firstname { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^.+\@.+\..+$", ErrorMessage = "Veuillez saisir une adresse mail valide: exemple@exemple.com")]
        public string mail { get; set; }

        [Required]
        [Display(Name = "Téléphone")]
        [RegularExpression(@"^(?:(?:\+|00)33|0)\s*[1-9](?:[\s.-]*\d{2}){4}$", ErrorMessage = "Le numéro de téléphone n'est pas valide.")]
        public string phoneNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appointments> appointments { get; set; }
    }
}
