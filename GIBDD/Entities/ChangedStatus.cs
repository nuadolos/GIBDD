//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GIBDD.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChangedStatus
    {
        public int Id { get; set; }
        public int LicenseId { get; set; }
        public int StatusId { get; set; }
        public string Comment { get; set; }
    
        public virtual Categories Categories { get; set; }
        public virtual License License { get; set; }
        public virtual Status Status { get; set; }
    }
}
